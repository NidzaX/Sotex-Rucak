import { Routes } from '@angular/router';
import { LoginComponent } from './login/login.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { ErrorComponent } from './error/error.component';
import { MenuComponent } from './dashboard/menu/menu.component';
import { MenuItemsComponent } from './dashboard/menu/menu-items/menu-items.component';

export const routes: Routes = [
    {path: 'login', component: LoginComponent},
    { path: 'dashboard', component: DashboardComponent, children: [
        { path: 'menu', component: MenuComponent, children: [
            { path: 'menu-items', component: MenuItemsComponent },
            { path: '', redirectTo: 'dashboard', pathMatch: 'full' } 
        ] },
        { path: '', redirectTo: 'dashboard', pathMatch: 'full' } 
      ]},
    {path: 'error', component: ErrorComponent},
    {path: '', component: LoginComponent},
];
