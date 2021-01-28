import { Component, Input, OnInit } from '@angular/core';
import { Picture } from 'src/app/interfaces/picture';

@Component({
  selector: 'app-gallery',
  templateUrl: './gallery.component.html',
  styleUrls: ['./gallery.component.css']
})
export class GalleryComponent implements OnInit {
  @Input() pictures: Array<Picture>;
  constructor() { }

  
  ngOnInit(): void {
  }
  // currentRate = 0;
}
