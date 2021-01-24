import { TestBed } from '@angular/core/testing';

import { HeroimageService } from './heroimage.service';

describe('HeroimageService', () => {
  let service: HeroimageService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(HeroimageService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
