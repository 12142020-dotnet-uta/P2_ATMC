import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-picture-detail',
  templateUrl: './picture-detail.component.html',
  styleUrls: ['./picture-detail.component.css']
})
export class PictureDetailComponent implements OnInit {

  currentRate:number = 0;

  
  constructor() { }
  ngOnInit(): void {
  }

}
