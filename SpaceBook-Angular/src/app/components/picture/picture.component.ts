import { Component, OnInit, Input } from '@angular/core';
import { Picture } from 'src/app/interfaces/picture';

@Component({
  selector: 'app-picture',
  templateUrl: './picture.component.html',
  styleUrls: ['./picture.component.css']
})
export class PictureComponent implements OnInit {
  @Input() picture: Picture;
  constructor() { }

  ngOnInit(): void {
    
  }
  currentRate = 0;

}
