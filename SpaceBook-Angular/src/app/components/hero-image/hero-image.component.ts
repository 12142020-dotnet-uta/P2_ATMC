import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { HeroimageService } from '../../services/heroimage.service';
import { HeroImg } from '../../interfaces/hero-image';

@Component({
  selector: 'app-hero-image',
  templateUrl: './hero-image.component.html',
  styleUrls: ['./hero-image.component.css']
})
export class HeroImageComponent implements OnInit {
s

  photos: HeroImg[] = [];

  constructor(private heroService: HeroimageService) {
    this.heroService.getDailyPhoto().subscribe((res : HeroImg[])=>{
      this.photos = res;
    });

   }

  ngOnInit(): void {
  }

}
