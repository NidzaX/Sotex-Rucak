import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { OrderService } from '../../../../../../core/services/order.service';
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
}
