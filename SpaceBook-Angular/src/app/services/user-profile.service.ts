import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';

import { Observable, of } from 'rxjs';
import {User} from '../interfaces/user';
import {Follow} from '../interfaces/follow';
import {Picture} from '../interfaces/picture';
import { stringify } from '@angular/compiler/src/util';
import { DialogUserEdit } from '../interfaces/dialog-user-edit';

@Injectable({
  providedIn: 'root'
})
export class UserProfileService {

  private usersUrl = 'api/Users';  // URL to web api
  private followUrl = 'api/Users/id'

  httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json' })
  };

  constructor(
    private http: HttpClient) { }

    getLoggedIn(): Observable<User>{
      return this.http.get<User>(`${this.usersUrl}/User`);
    }

    getUser(username?: string): Observable<User> {

      if(username == undefined){
        return this.http.get<User>(`${this.usersUrl}/User`);
      }
      return this.http.get<User>(`${this.usersUrl}/username/${username}`);
    }

    getFollowers(id: string): Observable<User[]>{
      return this.http.get<User[]>(`api/Users/Id/${id}/Followers`);
    }

    getFollowed(id: string): Observable<User[]>{
      return this.http.get<User[]>(`api/Users/Id/${id}/Followed`);
    }

    postFollow(id: string, loggedInId: string): Observable<any>{
      return this.http.post<any>(`api/Users/Id/${id}/Follow`, loggedInId);
    }

    deleteFollow(id:string): Observable<any>{
      return this.http.delete<any>(`api/Users/Id/${id}/Follow`);
    }

    getFavorites(id: string): Observable<Picture[]>{
      return this.http.get<Picture[]>(`api/Users/Id/${id}/Favorites`);
    }

    removeFavorite(userId: string, pictureId: number): Observable<any>{
      return this.http.request('delete', `api/Users/Id/${userId}/Favorites`, { body: pictureId });}

    putUser(editUser: DialogUserEdit): Observable<User>{
      return this.http.put<User>(`api/Users/Id/${editUser.id}`,editUser);
    }


}
