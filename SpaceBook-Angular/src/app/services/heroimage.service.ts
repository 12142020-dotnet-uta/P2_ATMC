import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';


@Injectable({
  providedIn: 'root'
})
export class HeroimageService {

  private UrlString: string = "/api/Pictures/daily"

  // httpOptions = {
  //   headers: new HttpHeaders({ 'Content-Type': 'application/json'})
  // };

  constructor(
    private http: HttpClient) { }

    getDailyPhoto(){
      return this.http.get(this.UrlString);
    }



}
