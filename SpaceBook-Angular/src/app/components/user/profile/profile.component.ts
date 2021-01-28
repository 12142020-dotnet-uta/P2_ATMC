import { Component, OnInit } from '@angular/core';

import { Router, ActivatedRoute } from '@angular/router';
import { Location } from '@angular/common';
import { User } from '../../../interfaces/user'
import { Follow } from '../../../interfaces/follow'
import { Picture } from '../../../interfaces/picture'
import { UserProfileService } from '../../../services/user-profile.service'
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css']
})
export class ProfileComponent implements OnInit {
  
  public user: User = new User();
  loggedIn: User = new User();

  followers: User[] = new Array<User>();

  followerUserNames: string[] = new Array<string>();
  following: User[] = new Array<User>();

  favorites: Picture[] = new Array<Picture>();
  username:string;

  constructor(private _userProfileService: UserProfileService, private router: Router, private route: ActivatedRoute) { 
    
  }

  ngOnInit(): void {
      this.route.params.subscribe( params => 
      {
         this.username = params["username"]; this.getUser(this.username);
      });
      this.getLoggedIn();
  }


  getLoggedIn(): void{
    this._userProfileService.getLoggedIn().subscribe(loggedIn => {this.loggedIn = loggedIn});
  }

  getUser(username: string): void {
    this._userProfileService.getUser(username).subscribe(user => 
        {this.user = user; this.getFollow(user.id); this.getFavorites(user.id)});
  }

  getFollow(id: string): void{
    this._userProfileService.getFollowers(id).subscribe(followers => {this.followers = followers; this.followers.forEach(follower => this.followerUserNames.push(follower.userName)) });
    this._userProfileService.getFollowed(id).subscribe(followed=> {this.following = followed; this} );
  }

  deleteFollow(id:string):void{
    this._userProfileService.deleteFollow(id).subscribe();
    let deleteuserindex = this.followers.indexOf(this.loggedIn);
    let loggedinusernameindex = this.followerUserNames.indexOf(this.loggedIn.userName);
    //console.log(deleteuserindex);
    console.log(loggedinusernameindex);
    window.location.reload();
  }

  postFollow(id:string, loggedInId: string){
    this._userProfileService.postFollow(id, loggedInId).subscribe();
    this.followers.push(this.loggedIn);
    this.followerUserNames.push(this.loggedIn.userName);
  }

  getFavorites(id: string): void{
    this._userProfileService.getFavorites(id).subscribe(favorites => this.favorites = favorites);
  }
    
      
}
