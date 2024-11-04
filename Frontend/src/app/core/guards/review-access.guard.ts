import { CanActivateFn } from '@angular/router';

export const reviewAccessGuard: CanActivateFn = (route, state) => {
  return true;
};
