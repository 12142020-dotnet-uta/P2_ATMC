import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';


@Injectable({
  providedIn: 'root'
})
export class PictureService {

  private UrlString: string = "/api/Pictures"


  constructor(private http: HttpClient) { }

  getPicture(){
    return this.http.get(this.UrlString);
  }

}
