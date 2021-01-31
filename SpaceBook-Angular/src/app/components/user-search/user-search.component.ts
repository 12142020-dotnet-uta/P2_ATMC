import { Component, OnInit } from '@angular/core';
import { User } from 'src/app/interfaces/user';
import { UserProfileService } from 'src/app/services/user-profile.service';

@Component({
  selector: 'app-user-search',
  templateUrl: './user-search.component.html',
  styleUrls: ['./user-search.component.css']
})
export class UserSearchComponent implements OnInit {
  searchString: string;
  searchResult: User[];
  constructor(private _userProfileService: UserProfileService) { }

  ngOnInit(): void {
  }

  async getUsersSearch(){
    this.searchResult = await this._userProfileService.searchUsers(this.searchString).toPromise();
  }
}
