import { CommonModule } from '@angular/common';
import { HttpClient, HttpHeaderResponse, HttpHeaders } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-menu-items',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './menu-items.component.html',
  styleUrl: './menu-items.component.css'
})
export class MenuItemsComponent implements OnInit {
  menuItems: any = [];

  constructor(private http: HttpClient) {}

  ngOnInit() {
    this.fetchMenuItems();
  }

  fetchMenuItems() {
    const headers = new HttpHeaders({
      Authorization: `Bearer ${localStorage.getItem('authToken')}`
    });

    this.http
      .get('http://localhost:5105/api/menus/get-menu-items', { headers })
      .subscribe({
        next: (response) => {
          this.menuItems = response;
          console.log("Fetched menu items:", this.menuItems);
        },
        error: (error) => console.error("Error fetching menu items:", error)
      });
  }
}
