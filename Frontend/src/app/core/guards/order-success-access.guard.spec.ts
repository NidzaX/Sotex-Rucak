import { TestBed } from '@angular/core/testing';
import { CanActivateFn } from '@angular/router';

import { orderSuccessAccessGuard } from './order-success-access.guard';

describe('orderSuccessAccessGuard', () => {
  const executeGuard: CanActivateFn = (...guardParameters) => 
      TestBed.runInInjectionContext(() => orderSuccessAccessGuard(...guardParameters));

  beforeEach(() => {
    TestBed.configureTestingModule({});
  });

  it('should be created', () => {
    expect(executeGuard).toBeTruthy();
  });
});
