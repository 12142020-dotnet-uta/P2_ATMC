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

  constructor(private _pictureService:PictureService, private router:Router) { }

  ngOnInit(): void {
    // this.isPicture =this.picture.mediaType==MediaType.Image
    // console.log('I am a picture with id: '+this.picture.pictureID)

  }
  goToPictureDetails(){
    this.router.navigateByUrl('/picture/'+this.picture.pictureID);
    console.log('going to picture details page for '+this.picture.pictureID);
  }
  currentRate = 0;

  AddToFavorites():void {
    //Add the picture to your favorites...
  }
}
