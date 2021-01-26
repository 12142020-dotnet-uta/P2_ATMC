import { Component, OnInit } from '@angular/core';
// import { PassThrough } from 'stream';
import { UserLogIn } from "../../../interfaces/user-log-in";
import { UserRegister } from "../../../interfaces/user-register";
import { UserAuthService } from "../../../services/user-auth.service";


@Component({
  selector: 'app-log-in',
  templateUrl: './log-in.component.html',
  styleUrls: ['./log-in.component.css']
})
export class LogInComponent implements OnInit {

  constructor( private _userAuthService: UserAuthService ) { }

  ngOnInit(): void {
    
  }

  boolLogIn = true;
  boolRegister = false;

  strFirstName:string;
  strLastName:string;
  strLogInUserName:string;
  strLogInPassword:string;
  strEmail: string;
  
  strUserName:string;
  strPassword:string;

  /* The user object that will be used over here */

  onChangingSection()
  {
    this.boolLogIn = !this.boolLogIn;
    this.boolRegister = !this.boolRegister;
  
  }


  RegisteringUser(event){

    event.preventDefault();
    //
    const FIRSTNAME = event.target.querySelector('#txtFirstName').value;
    const LASTNAME = event.target.querySelector('#txtLastName').value;
    const USERNAME = event.target.querySelector('#txtUserNameLog').value;
    const PASSWORD = event.target.querySelector('#txtPasswordLog').value;
    const EMAIL = event.target.querySelector('#txtEmail').value;
    
    let userRegister: UserRegister = {
      UserID : '',
      UserName : USERNAME,
      Password : PASSWORD,
      FirstName : FIRSTNAME,
      LastName : LASTNAME,
      Email: EMAIL
    }
    console.log( userRegister );
    
  }

  LogIngUser(event)
  {
    event.preventDefault();

    const USERNAME = event.target.querySelector('#txtUserNameLog').value;
    const PASSWORD = event.target.querySelector('#txtPasswordLog').value;


    let userLogIn: UserLogIn = {
      UserID : '',
      UserName : USERNAME,
      Password : PASSWORD,
      FirstName : this.strPassword,
      LastName : this.strLastName
    }

    console.log( userLogIn );

    let result :any = this._userAuthService.postLoginAutentication(userLogIn)
        .subscribe( ( userAutenticated ) => {
          console.log(userAutenticated);

          this._userAuthService.setSession(userAutenticated);

          console.log(localStorage.getItem('id_token'))

        } )
  }

}
