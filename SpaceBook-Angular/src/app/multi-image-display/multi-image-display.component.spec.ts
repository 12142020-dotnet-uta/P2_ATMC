import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MultiImageDisplayComponent } from './multi-image-display.component';

describe('MultiImageDisplayComponent', () => {
  let component: MultiImageDisplayComponent;
  let fixture: ComponentFixture<MultiImageDisplayComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ MultiImageDisplayComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(MultiImageDisplayComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
