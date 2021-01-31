import { ComponentFixture, TestBed } from '@angular/core/testing';
import { of } from 'rxjs';
import { MediaType } from 'src/app/interfaces/media-type';
import { PictureService } from 'src/app/services/picture.service';
import {  HttpClientModule   } from "@angular/common/http";

import { Picture } from 'src/app/interfaces/picture';
import { PictureDetailComponent } from './picture-detail.component';
import { ActivatedRoute } from '@angular/router';

describe('PictureDetailComponent', () => {
  let component: PictureDetailComponent;
  let fixture: ComponentFixture<PictureDetailComponent>;
  let mockPicture;
  let mockGeneralPictureRating;
  let mockUserRating;
  let mockPictureService;
  let mockUpdatePictureUserRating;
  let mockAddPictureUserRating;

  let picture = {
    pictureID :5,
    isUserPicture : false,
    title: 'My best picture',
    description: 'This is the best description for my best photo',
    imageURL : 'imageUrl',
    imageHDURL: 'imagHDUrl',
    mediaType: MediaType.Image,
    date: new Date(2021,1,30)
  };
  let globalPictureRating = 3.9;
  let userPictureRating = 5;

  beforeEach(async () => {
    mockPictureService =jasmine.createSpyObj('PictureService',[ 'getPictureDetails', 'getPictureGeneralRating', 'getPictureUserRating', 'putPictureUserRating', 'postPictureUserRating' ])
    mockPicture = mockPictureService.getPictureDetails.and.returnValue(of(picture));
    mockGeneralPictureRating = mockPictureService.getPictureGeneralRating.and.returnValue(of(globalPictureRating));
    mockUserRating = mockPictureService.getPictureUserRating.and.returnValue(of(userPictureRating));
    mockUpdatePictureUserRating = mockPictureService.putPictureUserRating.and.returnValue(of(userPictureRating));
    mockAddPictureUserRating = mockPictureService.postPictureUserRating.and.returnValue(of(userPictureRating));
    await TestBed.configureTestingModule({
      declarations: [ PictureDetailComponent ],
      imports: [HttpClientModule],
      providers: [{provide: PictureService, useValue: mockPictureService},
        {
          provide: ActivatedRoute,
          useValue: {
            params: of({
              id:  5, // represents the PictureID
            }),
          }
        }
      ]
    })
    .compileComponents(); 

    fixture = TestBed.createComponent(PictureDetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should call ngOnInit and call the get picture Detail', () => {
      component.ngOnInit();

      expect(mockUserRating.calls.any()).toBeTrue();
      expect(component.picture.title).toBe('My best picture');
  })

  it('should call getPicture() and update the picture in component', () => {
      component.picture = new Picture();
      component.picture.title = 'my default title';
      expect(component.picture.title).toBe('my default title');

      component.getPicture(5);

      expect(component.picture.title).toBe('My best picture');

  });

  it('should get the value of the global rating from getPictureGeneralRating( picId)', () => {
    component.currentRate = 1
    expect(component.currentRate).toBe(1);

    component.getPictureGeneralRating(5)

    expect(component.currentRate).toBe(3.9);
    expect(mockGeneralPictureRating.calls.any()).toBe(true);
  })

  it('should get the value of the user rating from getPictureUserRating(picId)', () => {

    component.currentRate = 4
    expect(component.currentRate).toBe(4);

    component.getPictureUserRating(5)

    expect(component.currentRate).toBe(5);
    expect(component.userAlreadyRated).toBe(true);
    expect(mockUserRating.calls.any()).toBe(true);
  })

  it('should Add or Update the rating using the AddRatingToPicture()', () => {
    component.currentRate = 3.25;
    expect(component.currentRate).toBe(3.25);
    component.userAlreadyRated = true

    component.AddRatingToPicture();

    expect(mockUpdatePictureUserRating.calls.any()).toBeTrue();

    component.userAlreadyRated = false;

    component.AddRatingToPicture();

    expect(mockAddPictureUserRating.calls.any()).toBeTrue();
    
  })
});
