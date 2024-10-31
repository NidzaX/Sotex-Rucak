import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { OrderService } from '../order.service';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';

@Component({
  selector: 'app-user-orders',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './user-orders.component.html',
  styleUrl: './user-orders.component.css'
})
export class UserOrdersComponent implements OnInit{
  orders: any = [];
  hasOrders: boolean = false;

  constructor(private http: HttpClient, private orderService: OrderService, private router: Router) {}

  ngOnInit(): void {
    this.fetchUserOrders();
  }

  fetchUserOrders() {
    const headers = {Authorization: `Bearer ${localStorage.getItem('authToken')}`};
    this.http
    .get('http://localhost:5105/api/orders/getUserOrders', { headers })
      .subscribe({
      next: (response) => {
        this.orders = response;
        console.log('Fetched orders:', this.orders);
        this.hasOrders = this.orders.length > 0;
      },
      error: (error) => console.error('Error fetching orders:', error)
    })
  }

  cancelOrder(orderId: string) {
    const headers = {Authorization: `Bearer ${localStorage.getItem('authToken')}`};
    this.http
    this.http.post(`http://localhost:5105/api/orders/cancelOrder?orderId=${orderId}`, {}, { headers })
      .subscribe({
        next: (response) => {
          console.log('Order cancelled:', response);
          this.fetchUserOrders();
        },
        error: (error) => console.error('Error fetching orders:', error)
      })
  }

  viewOrders() {
    this.router.navigate(['dashboard/menu/menu-items/user-orders']);
  }
}
