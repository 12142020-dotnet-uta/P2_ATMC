import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { UserPictureViewModel } from 'src/app/interfaces/user-picture-view-model';
import { PictureService } from 'src/app/services/picture.service';
import { Picture } from "../../../interfaces/picture";
import { UploadPictureDialogComponent } from './upload-picture-dialog/upload-picture-dialog.component';

// Maximum file size allowed to be uploaded = 1MB
const MAX_SIZE: number = 1048576;

@Component({
  selector: 'app-upload-picture',
  templateUrl: './upload-picture.component.html',
  styleUrls: ['./upload-picture.component.css']
})
export class UploadPictureComponent implements OnInit {

  pictureToUpload: UserPictureViewModel = new UserPictureViewModel();

  // strTitle:string;
  // strImageURL:string;
  // strDescription:string;
  // Date:Date;

  fileToUpload:File;
  lblImage:string = "Image file";
  isPictureUploaded:boolean = false;

  constructor( private _pictureService: PictureService, private dialog:MatDialog ) { }

  ngOnInit(): void {
  }

  uploadFile = (files) => {
    if(files.length === 0)
     return;

    this.fileToUpload = <File>files[0];
    // console.log(this.fileToUpload, this.fileToUpload.name.split(".")[1])

    let fileExtension:string = this.fileToUpload.name.split(".")[1]

    if( fileExtension == "jpg" || fileExtension == "jpeg" || fileExtension == "png"  ) // Accepts other pictures formats.
    {
      // Don't allow file sizes over the MAX_SIZE
      if( this.fileToUpload.size < MAX_SIZE )
      {
         this.lblImage = this.fileToUpload.name;
         this.pictureToUpload.fileExtension = fileExtension;
      }
      else
      {
        this.openDialog("This file is too big, add other smaller picture!")
      }
    }
    else
    {
      this.openDialog("This file is not valid!")
    }


  }

  UploadUserPicture() : void{
    console.log("upload");
    let reader = new FileReader();

    reader.onload = () => {

      // let PictureViewModel: any;
      // PictureViewModel.
      // let strImageBase64 = reader.result.toString();
      this.pictureToUpload.fileAsBase64 = reader.result.toString();

      console.log(this.pictureToUpload);

      // reader.readAsArrayBuffer
      // Send to the API Picture endpoint.

      this._pictureService.PostUserPicture(this.pictureToUpload)
        .subscribe( result => {

          console.log(result);
          if ( result )
          {
            this.openDialog("Picture was added successfully")
            this.isPictureUploaded = true;
          }
          
        },
        error => {

          this.openDialog(`there was an error, ${error.statusText}`);
          // alert("There was an error, " + error.statusText)
        });
    };
    
    reader.readAsDataURL(this.fileToUpload);

  }

  openDialog(message:string):void {
    const dialogRef = this.dialog.open(UploadPictureDialogComponent, {
      width: '300px',
      data: { message: message}
    });

    dialogRef.afterClosed().subscribe(result => {
      console.log('The dialog was closed');
    });
  }

}
