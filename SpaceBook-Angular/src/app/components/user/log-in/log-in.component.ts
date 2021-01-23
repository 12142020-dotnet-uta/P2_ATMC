import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-log-in',
  templateUrl: './log-in.component.html',
  styleUrls: ['./log-in.component.css']
})
export class LogInComponent implements OnInit {

  constructor() { }

  ngOnInit(): void {
  }

  userLogIn = true;
  userRegister = false;

  /* The user object that will be used over here */

  onChangingSection()
  {
    this.userLogIn = !this.userLogIn;
    this.userRegister = !this.userRegister;
  
  }


  RegisteringUser(){

    //
  }

  LogIngUser()
  {

  }

}
