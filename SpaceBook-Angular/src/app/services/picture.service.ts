import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Picture } from '../interfaces/picture';
import { UserPictureViewModel } from "../interfaces/user-picture-view-model";

@Injectable({
  providedIn: 'root'
})
export class PictureService {

  constructor(private _http:HttpClient) { }

  getPictures():Observable<Picture[]>{
    return this._http.get<Picture[]>('api/pictures');
  }
  getPictureDetails(pictureId:number):Observable<Picture>{
    return this._http.get<Picture>('/api/pictures/'+pictureId);
  }

  PostUserPicture(userPicture: UserPictureViewModel ) : Observable<Picture> {
    return this._http.post<Picture>("url", userPicture);
  }
}