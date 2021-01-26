import { TestBed } from '@angular/core/testing';

import { MultiImageDisplayService } from './multi-image-display.service';

describe('MultiImageDisplayService', () => {
  let service: MultiImageDisplayService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(MultiImageDisplayService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
