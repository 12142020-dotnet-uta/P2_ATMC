import { Component, OnInit } from '@angular/core';

import { ActivatedRoute } from '@angular/router';
import { Location } from '@angular/common';
import { User } from '../../../interfaces/user'
import { UserProfileService } from '../../../services/user-profile.service'
import { Subscription } from 'rxjs';
import { MatDialog } from '@angular/material/dialog';
import { EditUserDialogComponent } from './edit-user-dialog/edit-user-dialog.component';
import { DialogUserEdit } from '../../../interfaces/dialog-user-edit';

@Component({
  selector: 'app-my-profile',
  templateUrl: './my-profile.component.html',
  styleUrls: ['./my-profile.component.css']
})
export class MyProfileComponent implements OnInit {
  user: User;
  editUser: DialogUserEdit;
  constructor(private _userProfileService: UserProfileService, private route: ActivatedRoute, private dialog: MatDialog) { 
  }

  ngOnInit(): void {
    this.getUser();
  }
  
  getUser(): void {
    const username: string = this.route.snapshot.paramMap.get('username');

    this._userProfileService.getUser(username)
      .subscribe(user => this.user = user);
  }

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
      console.log('The dialog was closed');

      this.editUser = result;
      
      //Edit the user:
      this._userProfileService.putUser(this.editUser).subscribe( result =>{
        
        if(result)
        {
          alert("User updaded!");
          this.getUser()
        }
        else{
          alert("Invalid information.");
        }

      } )
      
    });
  }
  
}
