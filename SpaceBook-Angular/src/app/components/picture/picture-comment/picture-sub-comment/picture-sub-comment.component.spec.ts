import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PictureSubCommentComponent } from './picture-sub-comment.component';

describe('PictureSubCommentComponent', () => {
  let component: PictureSubCommentComponent;
  let fixture: ComponentFixture<PictureSubCommentComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ PictureSubCommentComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(PictureSubCommentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
