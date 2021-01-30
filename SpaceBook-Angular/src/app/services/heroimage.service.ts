import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { HeroImg } from '../interfaces/hero-image';


@Injectable({
  providedIn: 'root'
})
export class HeroimageService {

  private UrlString: string = "/api/Pictures/daily";

  constructor(
    private http: HttpClient) { }

  getDailyPhoto(): Observable<HeroImg[]> {
    return this.http.get<HeroImg[]>(this.UrlString);
  }


}
