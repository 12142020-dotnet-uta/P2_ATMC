import { ComponentFixture, TestBed } from '@angular/core/testing';
import {  HttpClientModule   } from "@angular/common/http";
import { UploadPictureComponent } from './upload-picture.component';
import { of } from 'rxjs';
import { PictureService } from 'src/app/services/picture.service';

describe('UploadPictureComponent', () => {
  let component: UploadPictureComponent;
  let fixture: ComponentFixture<UploadPictureComponent>;
  let mockPictureService;
  let mockPostUserPicture;
  let isUserUploaded = true;
  let userPictureVM = {
    title: 'newPicture',
    description: 'Description for my picture',
    fileAsBase64: 'data:image/jpeg;base64,/9j/4AAQSkZJRgABAgEAYABgAAD/4RCE',
    fileExtension: 'jpg'
  }



  beforeEach(async () => {
    mockPictureService = jasmine.createSpyObj('PictureService',[ 'PostUserPicture' ])
    mockPostUserPicture = mockPictureService.PostUserPicture.and.returnValue(of(isUserUploaded))
    await TestBed.configureTestingModule({
      declarations: [ UploadPictureComponent ],
      imports: [HttpClientModule],
      providers: [{provide: PictureService, useValue: mockPictureService}]
    })
    .compileComponents();
    fixture = TestBed.createComponent(UploadPictureComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });


  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should upload a picture using the UploadUserPicture()', async () => {
    component.pictureToUpload = userPictureVM;
    component.fileToUpload = new File(["data:image/jpeg;base64,/9j/4AAQS"], "filename", { type: 'image/jpeg' });
    expect(component.pictureToUpload.title).toBe('newPicture');

    component.UploadUserPicture();
    

    //........
    await expect(component.isPictureUploaded).toBeTrue();
  })
});
