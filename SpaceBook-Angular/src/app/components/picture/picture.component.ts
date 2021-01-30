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

  }
  goToPictureDetails(){
    this._pictureService.getPictureDetails(this.picture.pictureID).subscribe(x=>{this.router.navigateByUrl('/picture/'+x.pictureID);console.log('request returned picture '+x.title);})
    console.log('going to picture details page');
  }
  currentRate = 0;

  AddToFavorites():void {
    //Add the picture to your favorites...
  }
}
