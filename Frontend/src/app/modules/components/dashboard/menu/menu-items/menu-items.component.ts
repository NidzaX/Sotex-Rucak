import { Component, OnInit, Inject } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { OrderService } from '../../../../../core/services/order.service';

@Component({
  selector: 'app-menu-items',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule],
  templateUrl: './menu-items.component.html',
  styleUrls: ['./menu-items.component.css']
})
export class MenuItemsComponent implements OnInit {
  menuItems: any = [];
  orders: any = [];
  hasOrders: boolean = false;

  // Inject HttpClient and Router
  constructor(private http: HttpClient, private router: Router, private orderService: OrderService) {}

  ngOnInit() {
    this.fetchMenuItems();
    this.fetchUserOrders();
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

  fetchUserOrders() {
    const headers = { Authorization: `Bearer ${localStorage.getItem('authToken')}` };
    this.http
      .get<any[]>('http://localhost:5105/api/orders/getUserOrders', { headers })
      .subscribe({
        next: (response) => {
          this.orders = response;
          this.hasOrders = this.orders.length > 0; 
        },
        error: (error) => console.error('Error fetching orders:', error)
      });
  }

  reviewOrder() {
    const order = {
      dishes: this.menuItems.dishes.filter((dish: any) => dish.quantity > 0),
      sideDishes: this.menuItems.sideDishes.filter((sideDish: any) => sideDish.quantity > 0)
    };

    this.orderService.setOrder(order);
    this.router.navigate(['/dashboard/menu/menu-items/review'], {state: {order}});
  }

  viewOrders() {
    this.router.navigate(['/dashboard/menu/user-orders']); 
  }
}
