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
  currentRate = 0;

  constructor(private _pictureService:PictureService, private router:Router) { }

  ngOnInit(): void {
    //set picture
    this.isPicture = this.picture.mediaType==MediaType.Image;

  }
  goToPictureDetails(){
    this.router.navigateByUrl('/picture/'+this.picture.pictureID);
    console.log('going to picture details page for '+this.picture.pictureID);
  }

  AddToFavorites():void {
    //Add the picture to your favorites...
  }
}
