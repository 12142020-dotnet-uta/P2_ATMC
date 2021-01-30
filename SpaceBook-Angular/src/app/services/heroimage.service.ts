import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Picture } from '../interfaces/picture';


@Injectable({
  providedIn: 'root'
})
export class HeroimageService {

  private UrlString: string = "/api/Pictures/daily";

  constructor(
    private http: HttpClient) { }

  getDailyPhoto(): Observable<Picture> {
    return this.http.get<Picture>(this.UrlString);
  }


}
