import { Routes } from '@angular/router';
import { ErrorComponent } from './modules/components/error/error.component';
import { UserOrdersComponent } from './modules/components/dashboard/menu/menu-items/user-orders/user-orders.component';
import { OrderSuccessComponent } from './modules/components/dashboard/menu/menu-items/review/order-success/order-success.component';
import { MenuItemsComponent } from './modules/components/dashboard/menu/menu-items/menu-items.component';
import { ReviewComponent } from './modules/components/dashboard/menu/menu-items/review/review.component';
import { MenuComponent } from './modules/components/dashboard/menu/menu.component';
import { DashboardComponent } from './modules/components/dashboard/dashboard.component';
import { LoginComponent } from './modules/components/auth/login/login.component';


export const routes: Routes = [
    { path: 'login', component: LoginComponent },
    { path: 'dashboard', component: DashboardComponent, children: [
        { path: 'menu', component: MenuComponent, children: [
            { path: 'menu-items', component: MenuItemsComponent }, 
            { path: 'menu-items/review', component: ReviewComponent }, 
            { path: 'menu-items/review/order-success', component: OrderSuccessComponent },
            { path: 'user-orders', component: UserOrdersComponent }, 
            { path: '', redirectTo: 'menu-items', pathMatch: 'full' } 
        ] },
        { path: '', redirectTo: 'menu', pathMatch: 'full' } 
    ]},
    { path: 'error', component: ErrorComponent },
    { path: '', redirectTo: 'login', pathMatch: 'full' } 
];
