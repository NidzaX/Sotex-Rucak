import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { MenuComponent } from "./menu/menu.component";

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [RouterModule, CommonModule, MenuComponent],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.css'
})
export class DashboardComponent {
  isMenuAvailable: boolean = false;

  constructor(private router: Router) {}

  navigateToMenu() {
    this.router.navigate(['/dashboard/menu']);
  }
  
  onMenuStatusChanged(isAvailable: boolean) {
    this.isMenuAvailable = isAvailable;

    if (!this.isMenuAvailable) { 
      this.router.navigate(['/dashboard/menu']); 
  } else {
      this.router.navigate(['/dashboard/menu/menu-items']); 
   }
  }
}
