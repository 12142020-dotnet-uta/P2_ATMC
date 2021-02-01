import { Component, Input, OnInit } from '@angular/core';
import { ActivatedRoute} from '@angular/router';
import { Picture } from 'src/app/interfaces/picture';
import { PictureService } from 'src/app/services/picture.service';
import { PictureComponent } from '../picture.component';
import { PictureComment } from 'src/app/interfaces/picture-comment';
import { UserProfileService } from 'src/app/services/user-profile.service';
import { Favorite } from 'src/app/interfaces/favorite';
import { User } from 'src/app/interfaces/user';
import { MatDialog } from '@angular/material/dialog';
import { PictureRatingDialogComponent } from '../picture-rating-dialog/picture-rating-dialog.component';
import { DomSanitizer } from '@angular/platform-browser';
import { MediaType } from 'src/app/interfaces/media-type';

@Component({
  selector: 'app-picture-detail',
  templateUrl: './picture-detail.component.html',
  styleUrls: ['./picture-detail.component.css']
})
export class PictureDetailComponent implements OnInit {
  picture:Picture;

  currentCommentText:string
  currentlyCommenting:boolean=false

  allComments:PictureComment[]=[]
  favorites:Favorite[]=[]

  currentRate:number = 0;
  userAlreadyRated:boolean = false;
  safeURL:any;
  loggedIn:User;

  constructor(private sanitizer: DomSanitizer, private _pictureService:PictureService, private _userProfileService:UserProfileService, private route:ActivatedRoute, private _dialog: MatDialog) { }
  ngOnInit(): void {
    this.route.params.subscribe( params =>
      {
         this.getPicture(params["id"]);
         console.log('got '+params["id"]+' from the uri');

         this.getPictureUserRating(params['id']);
         this.getCommentsForPicture(params['id']);
         this.getFavorites(params['id']);
         
          
        });

    this.getLoggedIn();
    

  }

  getPicture(picId:number):void{
    this._pictureService.getPictureDetails(picId).subscribe(x=>{this.picture = x; if(this.picture.mediaType == MediaType.video){
      console.log("entering video if statement")
      this.safeURL = this.sanitizer.bypassSecurityTrustResourceUrl(this.picture.imageURL);
      console.log(`url is: ${this.safeURL}`);
  }});
  }

  getFavorites(picId:number){
    this.favorites = [];
    this._pictureService.getFavorites(picId).subscribe(x=>{this.favorites = x;});
  }

  //#region User Rating
  getPictureUserRating(picId: number) :void{
    this._pictureService.getPictureUserRating(picId)
      .subscribe( dataOnSuccess => { this.currentRate = dataOnSuccess; this.userAlreadyRated= true; },
         () => {
        //If there is no userRating, load the general rating of picture
        console.log("I have not rated this picture.")
        this.getPictureGeneralRating(picId);
      });
  }

  getPictureGeneralRating(picId: number) :void
  {
    console.log("Entering getPictureGeneralRating");
    this._pictureService.getPictureGeneralRating(picId)
      .subscribe( dataOnSuccess => { this.currentRate = dataOnSuccess; });
  }

  AddRatingToPicture():void {
    console.log("Rating picture", this.currentRate);

    //Add a confirmation if the user have already rated the picture
    if ( this.userAlreadyRated )
    {
      console.log("Just update the rating");

      this._pictureService.putPictureUserRating(this.picture.pictureID, this.currentRate)
        .subscribe( dataOnSuccess => {
          // alert("Rating updated successfully");
          this.openDialog("Rating updated successfully");
          this.currentRate = dataOnSuccess;
        }, dataOnError => {
          //error handling
        });

    }
    else{
      //Add rating to picture...
      console.log(this.picture.pictureID, this.currentRate);
      this._pictureService.postPictureUserRating(this.picture.pictureID,this.currentRate)
        .subscribe( dataOnSuccess => {
          // alert("Rating added successfully");
          this.openDialog("Rating added successfully");
          this.currentRate = dataOnSuccess;
        }, dataOnError => {
          //error handling
        })
    }

  }

  openDialog(message: string):void{
    const dialogRef = this._dialog.open(PictureRatingDialogComponent, {
      width: '400px',
      data: {message: message}
    });

    dialogRef.afterClosed().subscribe(result => {
      console.log('The dialog was closed');
    });

  }
  //#endregion

  //#region Comments
  getCommentsForPicture(picId:number){
    this._pictureService.getPictureComments(picId).subscribe(x=>{this.allComments=x;})
  }
  createComment(){
    this.currentlyCommenting=false;
    this._pictureService.postPictureComment(this.picture.pictureID,this.currentCommentText).subscribe(x=>this.getCommentsForPicture(this.picture.pictureID));
  }
  startCommenting(){
    this.currentlyCommenting=true;
  }

  getLoggedIn(): void{
    this._userProfileService.getLoggedIn().subscribe(loggedIn => {this.loggedIn = loggedIn});
  }

  postFavorite(userId:string, picId:number){
    this._userProfileService.postFavorite(userId, picId).subscribe(x=>this.getFavorites(this.picture.pictureID));
    let favorite: Favorite;
    this.favorites.push(favorite);
  }

  //#endregion

}
