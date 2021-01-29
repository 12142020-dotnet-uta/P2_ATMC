import { Component, Input, OnInit } from '@angular/core';
import { PagedResponse } from 'src/app/interfaces/paged-response';
import { Picture } from 'src/app/interfaces/picture';
import { PictureService } from 'src/app/services/picture.service';

@Component({
  selector: 'app-gallery',
  templateUrl: './gallery.component.html',
  styleUrls: ['./gallery.component.css']
})
export class GalleryComponent implements OnInit {
  @Input() pictures:Picture[]
  pagedResponse:PagedResponse;

  pictureRows: Picture[][];
  numRows:number = 5;
  numCols:number = 4;
  currentPage:number;
  constructor(private _pictureService:PictureService) { }


  ngOnInit(): void {

    if(this.pictures){
      this.makePictureArrays(this.pictures);
    }
    else{
      this.getPictures(1);
    }
  }
  getPictures(page:number):void{
    this._pictureService.getPictures(page,this.numRows*this.numCols).subscribe(x=>{this.updateFromPagedResponse(x);console.log('response total pages: '+x.totalPages)})
  }
  updateFromPagedResponse(response:PagedResponse){
    this.pagedResponse=response;
    this.pictures=response.data;
    this.makePictureArrays(this.pictures);

  }
  makePictureArrays(pics:Picture[]):void{
    this.pictureRows = [];
    // this.pictureRows = new Array(this.numRows,this.numCols) //[this.numRows][this.numCols];
    for(let row=0;row<this.numRows;row++){
      let curRow = []
      for(let col=0;col<this.numCols;col++){
        curRow[col]=pics[row*this.numCols+col]
        // this.pictureRows[row] = new Array().fill(pics,row*this.numCols,(row*this.numCols)+this.numCols-1)

      }
      this.pictureRows[row]=curRow;
    }


  }
  GetDetails(pic:Picture): void{
    console.log('a picture was clicked')
  }
  // currentRate = 0;
}
