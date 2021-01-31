import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { switchMap } from 'rxjs/operators'



import { Observable, of, timer } from 'rxjs';
import {User} from '../interfaces/user';
import {Follow} from '../interfaces/follow';
import {Picture} from '../interfaces/picture';
import { stringify } from '@angular/compiler/src/util';
import { DialogUserEdit } from '../interfaces/dialog-user-edit';
import { Message } from '../interfaces/message';
import { DialogMessageUser } from '../interfaces/dialog-message-user';

@Injectable({
  providedIn: 'root'
})
export class MessageService {

  private messageUrl = 'api/Messages';  // URL to web api

  httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json' })
  };

  constructor(
    private http: HttpClient) { }

    getMessages(): Observable<Message>{
      return this.http.get<Message>(`${this.messageUrl}`);
    }

    getUsersInConversation(): Observable<User[]> {

      return this.http.get<User[]>(`${this.messageUrl}/users`);
    }



    getMessagesBetweenUser(userId: string): Observable<Message[]>{

      return  this.http.get<Message[]>(`${this.messageUrl}/User/${userId}`);
    }

    postMessageToUser(userId: string, message: string): Observable<any>{
      return this.http.post<any>(`${this.messageUrl}/User/${userId}`, `"${message}"`, this.httpOptions);

    }




}
