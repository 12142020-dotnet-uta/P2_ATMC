import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MessageUserDialogComponent } from './message-user-dialog.component';

describe('MessageUserDialogComponent', () => {
  let component: MessageUserDialogComponent;
  let fixture: ComponentFixture<MessageUserDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ MessageUserDialogComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(MessageUserDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
