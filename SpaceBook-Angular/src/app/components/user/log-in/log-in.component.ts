import { Component, OnInit } from '@angular/core';
import { UserLogIn } from "../../../interfaces/user-log-in";
import { UserRegister } from "../../../interfaces/user-register";



@Component({
  selector: 'app-log-in',
  templateUrl: './log-in.component.html',
  styleUrls: ['./log-in.component.css']
})
export class LogInComponent implements OnInit {

  constructor() { }

  ngOnInit(): void {
    
  }

  boolLogIn = true;
  boolRegister = false;
  userLogIn: UserLogIn;
  userRegister: UserRegister;

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


  RegisteringUser(){

    //
    this.userRegister = {
      UserID : '',
      UserName : this.strUserName,
      Password : this.strPassword,
      FirstName : this.strPassword,
      LastName : this.strLastName,
      Email: this.strEmail
    }
    console.log(this.userLogIn);
    
  }

  LogIngUser()
  {

    this.userLogIn= {
      UserID : '',
      UserName : this.strUserName,
      Password : this.strPassword,
      FirstName : this.strPassword,
      LastName : this.strLastName
    }

    console.log(this.userRegister);
  }

}
