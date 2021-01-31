import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-picture-sub-comment',
  templateUrl: './picture-sub-comment.component.html',
  styleUrls: ['./picture-sub-comment.component.css']
})
export class PictureSubCommentComponent implements OnInit {
  @Input() comment:Comment

  constructor() { }

  ngOnInit(): void {
  }

}
