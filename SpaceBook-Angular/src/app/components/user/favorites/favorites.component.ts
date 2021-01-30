import { Component, Input, OnInit } from '@angular/core';
import { Picture } from 'src/app/interfaces/picture';
import { User } from 'src/app/interfaces/user';
import { UserProfileService } from 'src/app/services/user-profile.service'

@Component({
  selector: 'app-favorites',
  templateUrl: './favorites.component.html',
  styleUrls: ['./favorites.component.css']
})
export class FavoritesComponent implements OnInit {
  @Input() pictures: Picture[];
  @Input() loggedIn: User;
  pictureIds: number[] = new Array<number>();
  constructor(private _userProfileService: UserProfileService) { }

  ngOnInit(): void {
  }

  removeFavorite(picture: Picture){
    console.log(this.loggedIn.id);
    this._userProfileService.removeFavorite(this.loggedIn.id, picture.pictureID).subscribe();
    let index = this.pictures.indexOf(picture);
    this.pictures.splice(index, 1);

  }

}
