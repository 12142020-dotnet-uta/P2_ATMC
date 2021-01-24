import { Component, OnInit } from '@angular/core';
import { Picture } from "../../../interfaces/picture";

@Component({
  selector: 'app-upload-picture',
  templateUrl: './upload-picture.component.html',
  styleUrls: ['./upload-picture.component.css']
})
export class UploadPictureComponent implements OnInit {

  strTitle:string;
  strImageURL:string;
  strDescription:string;
  Date:Date;

  fileToUpload:File;

  constructor() { }

  ngOnInit(): void {
  }

  uploadFile = (files) => {
    if(files.length === 0)
     return;

    this.fileToUpload = <File>files[0];
    console.log(this.fileToUpload, this.fileToUpload.name.split(".")[1])

    if(this.fileToUpload.name.split(".")[1] == "jpg")
    {
      //Upload the photo
    }


  }

}
