import { Component, OnInit } from '@angular/core';

import { ActivatedRoute } from '@angular/router';
import { Location } from '@angular/common';
import { User } from '../../../interfaces/user'
import { UserProfileService } from '../../../services/user-profile.service'
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-my-profile',
  templateUrl: './my-profile.component.html',
  styleUrls: ['./my-profile.component.css']
})
export class MyProfileComponent implements OnInit {
  user: User;
  constructor(private _userProfileService: UserProfileService, private route: ActivatedRoute) { 
  }

  ngOnInit(): void {
    this.getUser();
  }
  
  getUser(): void {
    const username: string = this.route.snapshot.paramMap.get('username');

    this._userProfileService.getUser(username)
      .subscribe(user => this.user = user);
  }
  
}
