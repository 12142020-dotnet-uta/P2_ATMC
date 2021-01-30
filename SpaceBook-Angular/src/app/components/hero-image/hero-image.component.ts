import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { HeroimageService } from '../../services/heroimage.service';
import { HeroImg } from '../../interfaces/hero-image';
import { HttpClient } from '@angular/common/http';
import { Picture } from 'src/app/interfaces/picture';

@Component({
  selector: 'app-hero-image',
  templateUrl: './hero-image.component.html',
  styleUrls: ['./hero-image.component.css']
})
export class HeroImageComponent implements OnInit {




  photos : Picture;



  constructor(private heroService: HeroimageService) { }

  ngOnInit(){
    this.heroService.getDailyPhoto().subscribe(data => this.photos = data);

    }


}
