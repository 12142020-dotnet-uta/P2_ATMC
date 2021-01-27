import { TestBed } from '@angular/core/testing';

import { BearerAuthInterceptor } from './bearer-auth.interceptor';

describe('BearerAuthInterceptor', () => {
  beforeEach(() => TestBed.configureTestingModule({
    providers: [
      BearerAuthInterceptor
      ]
  }));

  it('should be created', () => {
    const interceptor: BearerAuthInterceptor = TestBed.inject(BearerAuthInterceptor);
    expect(interceptor).toBeTruthy();
  });
});
