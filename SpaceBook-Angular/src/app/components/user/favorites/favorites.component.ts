import { Component, Input, OnInit } from '@angular/core';
import { Picture } from 'src/app/interfaces/picture';

@Component({
  selector: 'app-favorites',
  templateUrl: './favorites.component.html',
  styleUrls: ['./favorites.component.css']
})
export class FavoritesComponent implements OnInit {
  @Input() pictures: Picture[];
  constructor() { }

  ngOnInit(): void {
  }

}
