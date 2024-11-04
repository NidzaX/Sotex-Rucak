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
  orders: any[] = [];

  constructor(private orderService: OrderService, private router: Router) {}

  ngOnInit(): void {
    this.fetchUserOrders();
  }

  fetchUserOrders() {
   this.orderService.getUserOrders().subscribe({
    next: (response) => {
      this.orders = response;
      console.log('Fetched users:', this.orders);
    },
    error: (error) => console.error('Error fetching orders:', error)
   });
  }

  cancelOrder(orderId: string) {
    this.orderService.cancelOrder(orderId).subscribe({
      next: (response) => {
        console.log('Order cancelled', response);
        this.fetchUserOrders();
      },
      error: (error) => console.error('Error cancelling order:', error)
    });
  }

  isOrderInactive(order: any): boolean {
    return order.isCancelled || new Date(order.validUntil) < new Date();
  }
}
