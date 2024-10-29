import { Routes } from '@angular/router';
import { LoginComponent } from './login/login.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { ErrorComponent } from './error/error.component';
import { MenuComponent } from './dashboard/menu/menu.component';

export const routes: Routes = [
    {path: 'login', component: LoginComponent},
    { path: 'dashboard', component: DashboardComponent, children: [
        { path: 'menu', component: MenuComponent },
        { path: '', redirectTo: 'dashboard', pathMatch: 'full' } 
      ]},
    {path: 'error', component: ErrorComponent},
    {path: '', component: LoginComponent},
];
