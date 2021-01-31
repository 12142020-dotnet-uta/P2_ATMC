import { Component, Input, OnInit } from '@angular/core';
import { PictureService } from 'src/app/services/picture.service';
import {PictureComment} from 'src/app/interfaces/picture-comment'
import { Picture } from 'src/app/interfaces/picture';
import { User } from 'src/app/interfaces/user';
import { UserProfileService } from 'src/app/services/user-profile.service';

@Component({
  selector: 'app-picture-comment',
  templateUrl: './picture-comment.component.html',
  styleUrls: ['./picture-comment.component.css']
})
export class PictureCommentComponent implements OnInit {

  @Input() picture:Picture;

  @Input() comment:PictureComment;

  user:User;

  childComments:PictureComment[]

  //edit text
  currentReplyText:string

  currentlyReplying:boolean = false

  constructor(private _pictureService:PictureService, private _userService:UserProfileService) { }

  ngOnInit(): void {
    this.getCommentUser();
    this.getCommentsForComment();
  }
  getCommentUser(){
    if(this.comment){
      this._userService.getUserById(this.comment.userCommentedId).subscribe(x=>{this.user = x});
    }
  }
  getCommentsForComment(){
    this._pictureService.getCommentChildren(this.picture.pictureID,this.comment.commentID).subscribe(x=>this.childComments=x)
  }
  createComment(){
    this.currentlyReplying =false;
    this._pictureService.postCommentComment(this.picture.pictureID,this.comment.commentID,this.currentReplyText).subscribe(x=>this.getCommentsForComment())
  }
  startReplying(){
    this.currentlyReplying = true;
  }

}
