import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DmButtonComponent } from './dm-button.component';

describe('DmButtonComponent', () => {
  let component: DmButtonComponent;
  let fixture: ComponentFixture<DmButtonComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ DmButtonComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(DmButtonComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
