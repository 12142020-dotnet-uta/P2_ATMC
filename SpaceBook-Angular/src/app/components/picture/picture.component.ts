import { Component, OnInit, Input } from '@angular/core';
import { Picture } from 'src/app/interfaces/picture';

import { MediaType } from 'src/app/interfaces/media-type'
import { PictureService } from 'src/app/services/picture.service';
import { Router, RouterLink } from '@angular/router';
import { PictureDetailComponent } from './picture-detail/picture-detail.component';
import { isImportDeclaration } from 'typescript';
import { MatDialog } from '@angular/material/dialog';
import { PictureRatingDialogComponent } from './picture-rating-dialog/picture-rating-dialog.component';

@Component({
  selector: 'app-picture',
  templateUrl: './picture.component.html',
  styleUrls: ['./picture.component.css']
})
export class PictureComponent implements OnInit {

  @Input() picture: Picture;
  isPicture:boolean;
  userAlreadyRated:boolean = false;
  currentRate = 0;

  pictureURL:string;

  constructor(private _pictureService:PictureService, private router:Router, private _dialog: MatDialog) { }

  ngOnInit(): void {
    //set picture

    this.getPictureGeneralRating()
    this.checkPicture()



  }
  goToPictureDetails(){
    this.router.navigateByUrl('/picture/'+this.picture.pictureID);
    console.log('going to picture details page for '+this.picture.pictureID);
  }
  checkPicture(){
    this.isPicture = this.picture.mediaType==MediaType.image;
    this.pictureURL = this.picture.imageURL;
    if(!this.isPicture){
      this.getVideoThumbnail()
    }

  }
  getVideoThumbnail(){
    //pictureURL should be set
    let pictureId:string;
    if(this.pictureURL.startsWith('https://www.youtube.com/embed/')){
      pictureId = this.pictureURL.replace('https://www.youtube.com/embed/','')
      pictureId = pictureId.replace('?rel=0','')
    }
    if(pictureId)
    //youtube thumbnail
    this.pictureURL=`https://img.youtube.com/vi/${pictureId}/0.jpg`

  }

  getPictureGeneralRating() :void
  {
    this._pictureService.getPictureGeneralRating(this.picture.pictureID)
      .subscribe( dataOnSuccess => {
        this.currentRate = dataOnSuccess;
        this.userAlreadyRated = true;
      },
      dataOnError => {
        // console.log(dataOnError);
      } );
  }

  AddToFavorites():void {
    //Add the picture to your favorites...
  }

  AddRatingToPicture():void {
    //Add a confirmation if the user have already rated the picture
    if ( this.userAlreadyRated )
    {
      console.log("Just update the rating");
      this._pictureService.putPictureUserRating(this.picture.pictureID, this.currentRate)
        .subscribe( dataOnSuccess => {
          // alert("Rating updated successfully");
          this.openDialog('Rating updated successfully');
          this.currentRate = dataOnSuccess;
        }, dataOnError => {
          //error handling
          console.log(dataOnError);
        });
    }
    else{
      //Add rating to picture...
      console.log(this.picture.pictureID, this.currentRate);
      this._pictureService.postPictureUserRating(this.picture.pictureID,this.currentRate)
        .subscribe( dataOnSuccess => {
          // alert("Rating added successfully");
          this.openDialog('Rating added successfully')
          this.currentRate = dataOnSuccess;
        }, dataOnError => {
          //error handling
          console.log(dataOnError);
        });
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

}

