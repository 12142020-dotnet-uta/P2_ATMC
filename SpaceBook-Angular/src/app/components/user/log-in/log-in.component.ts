import { Component, OnInit } from '@angular/core';
// import { PassThrough } from 'stream';
import { UserLogIn } from "../../../interfaces/user-log-in";
import { UserRegister } from "../../../interfaces/user-register";
import { UserAuthService } from "../../../services/user-auth.service";
import { Router } from "@angular/router"


@Component({
  selector: 'app-log-in',
  templateUrl: './log-in.component.html',
  styleUrls: ['./log-in.component.css']
})
export class LogInComponent implements OnInit {
  public userRegister: UserRegister = new UserRegister();
  public userLogin: UserLogIn = new UserLogIn();
  constructor( private _userAuthService: UserAuthService, private router: Router ) { }

  ngOnInit(): void {

  }

  boolLogIn = true;
  boolRegister = false;

  // strFirstName:string;
  // strLastName:string;
  // strLogInUserName:string;
  // strLogInPassword:string;
  // strEmail: string;
  
  // strUserName:string;
  // strPassword:string;

  /* The user object that will be used over here */

  onChangingSection()
  {
    this.boolLogIn = !this.boolLogIn;
    this.boolRegister = !this.boolRegister;
  
  }


  RegisteringUser(event){

    event.preventDefault();
    //
    // const FIRSTNAME = event.target.querySelector('#txtFirstName').value;
    // const LASTNAME = event.target.querySelector('#txtLastName').value;
    // const USERNAME = event.target.querySelector('#txtUserName').value;
    // const PASSWORD = event.target.querySelector('#txtPassword').value;
    // const EMAIL = event.target.querySelector('#txtEmail').value;
    
    // let userRegister: UserRegister = {
    //   firstName : FIRSTNAME,
    //   lastName : LASTNAME,
    //   username : USERNAME,
    //   email: EMAIL,
    //   password : PASSWORD,
    // }
    this.router.navigateByUrl('/authentication').then(() => {
      window.location.reload()});
    this._userAuthService.postRegisterAutentication(this.userRegister).subscribe();
    console.log( this.userRegister );
    
  }

  LogIngUser(event)
  {
    event.preventDefault();

    // const USERNAME = event.target.querySelector('#txtUserNameLog').value;
    // const PASSWORD = event.target.querySelector('#txtPasswordLog').value;


    // let userLogIn: UserLogIn = {
    //   username : USERNAME,
    //   password : PASSWORD
    // }

    console.log( this.userLogin );
    this.router.navigate(['']).then(() => {
      window.location.reload()});
    let result :any = this._userAuthService.postLoginAutentication(this.userLogin)
        .subscribe( ( userAutenticated ) => {
          console.log(userAutenticated);

          this._userAuthService.setSession(userAutenticated);

          console.log(localStorage.getItem('id_token'))

        } )
  }

}
