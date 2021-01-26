import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { PictureService } from '../../services/picture.service';
import { Picture } from '../../interfaces/picture';

@Component({
  selector: 'picture',
  templateUrl: './picture.component.html',
  styleUrls: ['./picture.component.css']
})
export class PictureComponent implements OnInit {

  private picture: Picture[] = [];

  constructor(private picService: PictureService){ 
  this.picService.getPicture().subscribe((res: Picture[]) => {

  });

  }

  ngOnInit(): void {
  }
  currentRate = 0;

}
