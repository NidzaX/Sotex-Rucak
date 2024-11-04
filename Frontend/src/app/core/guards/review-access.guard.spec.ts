import { TestBed } from '@angular/core/testing';
import { CanActivateFn } from '@angular/router';

import { reviewAccessGuard } from './review-access.guard';

describe('reviewAccessGuard', () => {
  const executeGuard: CanActivateFn = (...guardParameters) => 
      TestBed.runInInjectionContext(() => reviewAccessGuard(...guardParameters));

  beforeEach(() => {
    TestBed.configureTestingModule({});
  });

  it('should be created', () => {
    expect(executeGuard).toBeTruthy();
  });
});
