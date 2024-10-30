import { Component, OnInit, Inject } from '@angular/core';
import { Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-menu-items',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './menu-items.component.html',
  styleUrls: ['./menu-items.component.css']
})
export class MenuItemsComponent implements OnInit {
  menuItems: any = [];

  // Inject HttpClient and Router
  constructor(private http: HttpClient, private router: Router) {}

  ngOnInit() {
    this.fetchMenuItems();
  }

  fetchMenuItems() {
    const headers = { Authorization: `Bearer ${localStorage.getItem('authToken')}` };
    this.http
      .get('http://localhost:5105/api/menus/get-menu-items', { headers })
      .subscribe({
        next: (response) => {
          this.menuItems = response;
          console.log('Fetched menu items:', this.menuItems);
        },
        error: (error) => console.error('Error fetching menu items:', error),
      });
  }

  reviewOrder() {
    this.router.navigate(['/dashboard/menu/menu-items/review']);
  }
}
