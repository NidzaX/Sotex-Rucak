import { Routes } from '@angular/router';
import { ErrorComponent } from './modules/components/error/error.component';
import { UserOrdersComponent } from './modules/components/dashboard/menu/menu-items/user-orders/user-orders.component';
import { OrderSuccessComponent } from './modules/components/dashboard/menu/menu-items/review/order-success/order-success.component';
import { MenuItemsComponent } from './modules/components/dashboard/menu/menu-items/menu-items.component';
import { ReviewComponent } from './modules/components/dashboard/menu/menu-items/review/review.component';
import { DashboardComponent } from './modules/components/dashboard/dashboard.component';
import { LoginComponent } from './modules/components/auth/login/login.component';
import { reviewAccessGuard } from './core/guards/review-access.guard';
import { authGuardGuard } from './core/guards/auth.guard';
import { orderSuccessAccessGuard } from './core/guards/order-success-access.guard'; // Import the guard
import { MenuComponent } from './modules/components/dashboard/menu/menu.component';

export const routes: Routes = [
  { path: 'login', component: LoginComponent },
  { 
    path: 'dashboard', 
    component: DashboardComponent, 
    canActivate: [authGuardGuard], 
    children: [
      { 
        path: 'menu', 
        component: MenuComponent,
        canActivate: [authGuardGuard], 
        children: [
          { path: 'menu-items', component: MenuItemsComponent }, 
          {
            path: 'menu-items/review',
            component: ReviewComponent,
            canActivate: [reviewAccessGuard]
          },
          { 
            path: 'menu-items/review/order-success', 
            component: OrderSuccessComponent, 
            canActivate: [orderSuccessAccessGuard] 
          },
          { path: 'user-orders', component: UserOrdersComponent }, 
          { path: '', redirectTo: 'menu-items', pathMatch: 'full' } 
        ]
      },
      { path: '', redirectTo: 'menu', pathMatch: 'full' } 
    ]
  },
  { path: 'error', component: ErrorComponent },
  { path: '', redirectTo: 'login', pathMatch: 'full' } 
];
