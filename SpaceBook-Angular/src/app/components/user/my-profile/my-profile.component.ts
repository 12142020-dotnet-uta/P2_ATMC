import { Component, OnInit, Inject } from '@angular/core';
import { MatDialog, MAT_DIALOG_DATA} from '@angular/material/dialog';
import { ActivatedRoute } from '@angular/router';
import { User } from '../../../interfaces/user'
import { UserProfileService } from '../../../services/user-profile.service'
import { Picture } from 'src/app/interfaces/picture';


@Component({
  selector: 'app-my-profile',
  templateUrl: './my-profile.component.html',
  styleUrls: ['./my-profile.component.css']
})
export class MyProfileComponent implements OnInit {
  user: User;
  public followers: User[] = new Array<User>();

  public followerUserNames: string[];
  public followed: User[] = new Array<User>();

  public favorites: Picture[] = new Array<Picture>();
  
  constructor(private _userProfileService: UserProfileService, private route: ActivatedRoute) { 
  }

  async ngOnInit(){
   this.user = await this.getLoggedIn();
   this.followers = await this.getFollowers(this.user.id);
   this.followed = await this.getFollowed(this.user.id);
   this.favorites = await this.getFavorites(this.user.id);
  }
  
  getLoggedIn(){
    return this._userProfileService.getLoggedIn().toPromise();
  }


  getFollowers(id: string){
    return this._userProfileService.getFollowers(id).toPromise();
  }

  getFollowed(id: string){
    return this._userProfileService.getFollowed(id).toPromise();
  }

  getFavorites(id: string){
    return this._userProfileService.getFavorites(id).toPromise();
  }

  // openEditUser(){
  //   this.dialog.open(EditUserDialog, {
  //     data: {
  //       animal: 'panda'
  //     }
  //   });
  // }

}
