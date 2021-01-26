import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { UserLogIn } from "../interfaces/user-log-in";
import { UserRegister } from "../interfaces/user-register";
import { HttpClient, HttpHeaders } from "@angular/common/http";


@Injectable({
  providedIn: 'root'
})
export class UserAuthService {

  constructor( private _http:HttpClient) { }

  postLoginAutentication(userLogin : UserLogIn) : Observable<UserLogIn> {
    // let headers = new HttpHeaders ({
    //   'Content-Type': 'application/json',
    //   '"Access-Control-Allow-Origin' : '*'
    // })

    // let options = {headers: headers}

    return this._http.post<UserLogIn>("https://localhost:44398/api/Authenticate/login", {
      UserName: userLogin.UserName,
      Password: userLogin.Password  
    } ) 
  }
  postRegisterAutentication(userRegister: UserRegister) : Observable<UserRegister> {
    
    return this._http.post<UserRegister>("localhost:44398/api/Authenticate/register",userRegister)
  }
  //Passw0rd_1
}
