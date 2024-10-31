import { CommonModule } from '@angular/common';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';


@Component({
  selector: 'app-review',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './review.component.html',
  styleUrl: './review.component.css'
})
export class ReviewComponent implements OnInit {
  order: any;
  isOrderEmpty: boolean = false;

  constructor(private router: Router, private http: HttpClient) {
    const navigation = this.router.getCurrentNavigation();
    console.log('Current Navigation:', navigation);
    this.order = navigation?.extras?.state?.['order'];
    console.log('Received order:', this.order);
  }

  ngOnInit() {
    if (!this.order || 
      (!this.order.dishes || this.order.dishes.length === 0) &&
      (!this.order.sideDishes || this.order.sideDishes.length === 0)) {
      console.warn('Order is empty, redirecting back to menu items.');
      this.isOrderEmpty = true;
      this.router.navigate(['/dashboard/menu/menu-items']);
    }
  }

  submitOrder() {
    console.log('Submit Order clicked');

    const headers = new HttpHeaders({
      Authorization: `Bearer ${localStorage.getItem('authToken')}`,
      'Content-Type': 'application/json',
    });

    const orderDto = {
      dishes: this.order.dishes.map((d: any) => ({
        dishId: d.id,
        dishQuantity: d.quantity,
      })),
      sideDishes: this.order.sideDishes.map((sd: any) => ({
        sideDishId: sd.id,
        sideDishQuantity: sd.quantity,
      })),
    };
    
    this.http
      .post('http://localhost:5105/api/orders/addOrder', orderDto, { headers })
      .subscribe({
        next: (response) => {
          console.log('Order submitted successfully:', response);
          this.router.navigate(['/dashboard/menu/menu-items/order-success']);
        },
        error: (error) => console.error('Error submitting order:', error),
      });
  }
}
