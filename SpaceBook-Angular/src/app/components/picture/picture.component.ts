import { Component, OnInit, Input } from '@angular/core';
import { Picture } from 'src/app/interfaces/picture';

import { MediaType } from 'src/app/interfaces/media-type'
import { PictureService } from 'src/app/services/picture.service';
import { Router, RouterLink } from '@angular/router';
import { PictureDetailComponent } from './picture-detail/picture-detail.component';
import { isImportDeclaration } from 'typescript';

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

  constructor(private _pictureService:PictureService, private router:Router) { }

  ngOnInit(): void {
    //set picture
    this.isPicture = this.picture.mediaType==MediaType.Image;
    this.getPictureGeneralRating()
  }
  goToPictureDetails(){
    this.router.navigateByUrl('/picture/'+this.picture.pictureID);
    console.log('going to picture details page for '+this.picture.pictureID);
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
        });
    }
  } 

}
