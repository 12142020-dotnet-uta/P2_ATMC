import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { DialogMessageUser } from 'src/app/interfaces/dialog-message-user';

@Component({
  selector: 'app-message-user-dialog',
  templateUrl: './message-user-dialog.component.html',
  styleUrls: ['./message-user-dialog.component.css']
})
export class MessageUserDialogComponent implements OnInit {

  constructor(public dialogRef: MatDialogRef<MessageUserDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: DialogMessageUser) { }

  ngOnInit(): void {
  }

  onNoClick(): void {
    this.dialogRef.close();
  }
}
