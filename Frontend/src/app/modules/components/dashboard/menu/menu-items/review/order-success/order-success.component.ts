import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { OrderService } from '../../../../../../../core/services/order.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-order-success',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './order-success.component.html',
  styleUrl: './order-success.component.css'
})
export class OrderSuccessComponent implements OnInit{
  order: any;

  constructor(public router: Router, private orderService: OrderService) {}

  ngOnInit(): void {
    this.order = this.orderService.getOrder();

    if(!this.order || (!this.order.dishes && !this.order.sideDishes)) {
      console.warn('No order found, redirecting to menu items.');
      this.router.navigate(['/dashboard/menu/menu-items']);
    } else {
      this.orderService.setOrder(this.order);
    }
  }
}
