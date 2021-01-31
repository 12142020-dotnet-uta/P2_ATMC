import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';

@Component({
  selector: 'app-picture-rating-dialog',
  templateUrl: './picture-rating-dialog.component.html',
  styleUrls: ['./picture-rating-dialog.component.css']
})
export class PictureRatingDialogComponent  {

  constructor(public dialogRef: MatDialogRef<PictureRatingDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any) { }

    onNoClick(): void {
      this.dialogRef.close();
    }
}
