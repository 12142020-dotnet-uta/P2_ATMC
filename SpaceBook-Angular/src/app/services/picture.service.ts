import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { PagedResponse } from '../interfaces/paged-response';
import { Picture } from '../interfaces/picture';
import { Rating } from '../interfaces/rating';

import { UserPictureViewModel } from "../interfaces/user-picture-view-model";



@Injectable({
  providedIn: 'root'
})
export class PictureService {

  constructor(private _http:HttpClient) { }

  getPictures(page:number,pageSize:number):Observable<PagedResponse>{
    return this._http.get<PagedResponse>('api/pictures?pageNumber='+page+'&pageSize='+pageSize);
  }
  getPictureDetails(pictureId:number):Observable<Picture>{
    console.log('sending a request for picture '+pictureId)
    return this._http.get<Picture>('/api/pictures/'+pictureId);
  }


  PostUserPicture(userPicture: UserPictureViewModel ) : Observable<boolean> {
    return this._http.post<boolean>("/api/pictures", userPicture);
  }

  getPictureGeneralRating(pictureid: number):Observable<number>{
    return this._http.get<number>(`api/pictures/${pictureid}/Ratings`);
  }

  getPictureUserRating(pictureid:number):Observable<number>{
    return this._http.get<number>(`api/pictures/${pictureid}/Ratings/User`)
  }

  postPictureUserRating(pictureId: number, rating:number): Observable<number>
  {
    return this._http.post<number>(`api/pictures/${pictureId}/Ratings`, rating);
  }

  putPictureUserRating(pictureId:number, rating:number): Observable<number>
  {
    return this._http.put<number>(`api/pictures/${pictureId}/Ratings`,rating);
  }

}


