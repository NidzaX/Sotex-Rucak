import { CanActivateFn, Router } from '@angular/router';
import { OrderService } from '../services/order.service';
import { inject } from '@angular/core';

export const reviewAccessGuard: CanActivateFn = (route, state) => {
  const orderService = inject(OrderService);
  const router = inject(Router);

  if(orderService.isOrderSubmitted()) {
    router.navigate(['/dashboard/menu/menu-items/review/order-success']);
    return false;
  }

  return true;
};
