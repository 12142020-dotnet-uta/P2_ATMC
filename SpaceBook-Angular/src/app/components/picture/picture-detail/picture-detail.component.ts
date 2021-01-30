import { Component, Input, OnInit } from '@angular/core';
import { ActivatedRoute} from '@angular/router';
import { Picture } from 'src/app/interfaces/picture';
import { PictureService } from 'src/app/services/picture.service';

@Component({
  selector: 'app-picture-detail',
  templateUrl: './picture-detail.component.html',
  styleUrls: ['./picture-detail.component.css']
})
export class PictureDetailComponent implements OnInit {
  picture:Picture;


  currentRate:number = 0;
  userAlreadyRated:boolean = false;

  constructor(private _pictureService:PictureService, private route:ActivatedRoute) { }
  ngOnInit(): void {
    this.route.params.subscribe( params =>
      {
         this.getPicture(params["id"]);
         console.log('got '+params["id"]+' from the uri');

         this.getPictureUserRating(params['id']);
        });
        
  }
  getPicture(picId:number):void{
    this._pictureService.getPictureDetails(picId).subscribe(x=>{this.picture = x;console.log('returned picture with id: ',x)});
  }
  
  getPictureUserRating(picId: number) :void{
    this._pictureService.getPictureUserRating(picId)
      .subscribe( dataOnSuccess => { this.currentRate = dataOnSuccess; this.userAlreadyRated= true; },
         () => {
        //If there is no userRating, load the general rating of picture
        console.log("error")
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
          alert("Rating updated successfully");
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
          alert("Rating added successfully");
          this.currentRate = dataOnSuccess;
        }, dataOnError => {
          //error handling
        })
    }

  } 


}
