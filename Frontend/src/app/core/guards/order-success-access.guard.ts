import { CanActivateFn, Router } from '@angular/router';
import { OrderService } from '../services/order.service';
import { inject } from '@angular/core';

export const orderSuccessAccessGuard: CanActivateFn = (route, state) => {
  const orderService = inject(OrderService);
  const router = inject(Router);

  const order = orderService.getOrder();

  if(order && (order.dishes.length > 0 || order.sideDishes.length > 0)) {
    return true;
  } else {
    router.navigate(['dashboard/menu/menu-items'])
    return false;
  }
};
