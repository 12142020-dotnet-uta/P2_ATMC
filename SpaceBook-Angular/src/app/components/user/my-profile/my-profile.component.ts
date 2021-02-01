import { Component, OnInit, Inject } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { User } from '../../../interfaces/user'
import { UserProfileService } from '../../../services/user-profile.service'
import { Picture } from 'src/app/interfaces/picture';
import { Subscription } from 'rxjs';
import { EditUserDialogComponent } from './edit-user-dialog/edit-user-dialog.component';
import { DialogUserEdit } from '../../../interfaces/dialog-user-edit';
import { MatDialog } from '@angular/material/dialog';
import { FollowingDialogComponent } from '../following-dialog/following-dialog.component';
import { FollowersDialogComponent } from '../followers-dialog/followers-dialog.component';
import { UploadPictureDialogComponent } from '../../picture/upload-picture/upload-picture-dialog/upload-picture-dialog.component';

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
  
  editUser: DialogUserEdit;
  constructor(private _userProfileService: UserProfileService, private route: ActivatedRoute, private dialog: MatDialog) {
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

  openDialog() : void{
    const dialogRef = this.dialog.open(EditUserDialogComponent, {
      width: '500px',
      data: {
        firstName: this.user.firstName, 
        lastName: this.user.lastName, 
        id: this.user.id, 
        email: this.user.email,
        oldPassword: '',
        newPassword: '',
      },
      restoreFocus:false
    });

    dialogRef.afterClosed().subscribe(result => {

      this.editUser = result;
      
      if (result !== undefined)
      //Edit the user:
        this._userProfileService.putUser(this.editUser).subscribe( result =>{
          
          if(result)
          {
            // alert("User updaded!");
            this.openDialogText("User updaded!");
            this.getLoggedIn().then( (result) => {
              console.log(result);
              this.user = result;
            })
          }
          else{
            this.openDialogText("Invalid information.");
            // alert("Invalid information.");
          }

        } )
      
    });
  }

  //display the Component Dialog
  openDialogText(message:string): void{
    const dialogRef = this.dialog.open(UploadPictureDialogComponent, {
      width: '400px',
      data: { message: message },
    });
  }

  openFollowingDialog() : void{
    const dialogRef = this.dialog.open(FollowingDialogComponent, {
      width: '500px',
      data: {
        user: this.user,
        following: this.followed
      },
      restoreFocus:false
    });
  }

  openFollowerDialog() : void{
    const dialogRef = this.dialog.open(FollowersDialogComponent, {
      width: '500px',
      data: {
        user: this.user,
        followers: this.followers
      },
      restoreFocus:false
    });
  }
  
}
