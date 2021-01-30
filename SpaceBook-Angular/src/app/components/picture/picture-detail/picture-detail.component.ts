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

  constructor(private pictureService:PictureService, private route:ActivatedRoute) { }
  ngOnInit(): void {
    this.route.params.subscribe( params =>
      {
         this.getPicture(params["id"]);
         console.log('got '+params["id"]+' from the uri')
      });
  }
  getPicture(picId:number):void{
    this.pictureService.getPictureDetails(picId).subscribe(x=>{this.picture = x;console.log('returned picture with id: ',x)});
  }
}
