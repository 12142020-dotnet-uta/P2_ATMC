import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { DialogFollowing } from 'src/app/interfaces/dialog-following';

@Component({
  selector: 'app-followers-dialog',
  templateUrl: './followers-dialog.component.html',
  styleUrls: ['./followers-dialog.component.css']
})
export class FollowersDialogComponent implements OnInit {

  constructor(public dialogRef: MatDialogRef<FollowersDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: DialogFollowing) { }

  ngOnInit(): void {
  }

  onNoClick(): void {
    this.dialogRef.close();
  }


}
