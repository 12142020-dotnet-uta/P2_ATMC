import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { DialogFollowing } from 'src/app/interfaces/dialog-following';

@Component({
  selector: 'app-following-dialog',
  templateUrl: './following-dialog.component.html',
  styleUrls: ['./following-dialog.component.css']
})
export class FollowingDialogComponent implements OnInit {

  constructor(public dialogRef: MatDialogRef<FollowingDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: DialogFollowing) { }

  ngOnInit(): void {
  }

  onNoClick(): void {
    this.dialogRef.close();
  }

}
