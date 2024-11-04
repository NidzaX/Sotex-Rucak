import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from '../services/auth.service';
import { inject } from '@angular/core';

export const authGuardGuard: CanActivateFn = (route, state) => {
  const authService = inject(AuthService);
  const router = inject(Router);

  const isLoggedIn = authService.isAuthenticated();

  if (!isLoggedIn) {
    console.log('User is not authenticated, redirecting to login');
    router.navigate(['/login']);
}
  
  return isLoggedIn
};
