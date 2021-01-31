import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';

@Component({
  selector: 'app-upload-picture-dialog',
  templateUrl: './upload-picture-dialog.component.html',
  styleUrls: ['./upload-picture-dialog.component.css']
})
export class UploadPictureDialogComponent  {

  constructor(public dialogRef: MatDialogRef<UploadPictureDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any) { }

    onNoClick(): void {
      this.dialogRef.close();
    }
}
