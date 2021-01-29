import { Component,OnInit } from '@angular/core';
import { User } from '../app/interfaces/user'
import { UserProfileService } from '../app/services/user-profile.service'
import { Router } from '@angular/router'

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'SpaceBook';
  activeId:number;
  isCollapsed = false;

  loggedIn: User = new User();


  constructor(private _userProfileService: UserProfileService, private router: Router) { 
    
  }

  ngOnInit(): void {
      this.getLoggedIn();
  }

  getLoggedIn(): void{
    this._userProfileService.getLoggedIn().subscribe(loggedIn => {this.loggedIn = loggedIn});
  }

  logout(){
    localStorage.removeItem('id_token');
    localStorage.removeItem("expires_at");
    this.router.navigate(['']).then(() => {
      window.location.reload()});
  }
}
