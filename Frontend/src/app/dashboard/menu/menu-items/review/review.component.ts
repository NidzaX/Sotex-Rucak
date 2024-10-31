import { CommonModule } from '@angular/common';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { OrderService } from '../order.service';
import { MenuService } from '../menu.service';


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
  dishesData: any[] = [];
  sideDishesData: any[] = [];

  constructor(
    private router: Router,
    private http: HttpClient,
    private orderService: OrderService,
    private menuService: MenuService,
  ) {
    const navigation = this.router.getCurrentNavigation();
    console.log('Current Navigation:', navigation);
    this.order = navigation?.extras?.state?.['order'];
    console.log('Received order:', this.order);
    this.orderService.setOrder(this.order); 
  }

  ngOnInit() {
    this.order = this.orderService.getOrder();
    console.log("Order in ReviewComponent:", this.order); 

    if (!this.order || 
      (!this.order.dishes || this.order.dishes.length === 0) &&
      (!this.order.sideDishes || this.order.sideDishes.length === 0)) {
      console.warn('Order is empty, redirecting back to menu items.');
      this.isOrderEmpty = true;
      this.router.navigate(['/dashboard/menu/menu-items']);
    }

    this.menuService.getDishes().subscribe(response => {
      console.log("Dishes Data:", response.dishes);
      this.dishesData = Array.isArray(response.dishes) ? response.dishes : [];
    });
  
    this.menuService.getSideDishes().subscribe(response => {
      console.log("Side Dishes Data:", response.sideDishes);
      this.sideDishesData = Array.isArray(response.sideDishes) ? response.sideDishes : [];
    });
  }

  submitOrder() {
    console.log('Submit Order clicked');
    
    console.log('Order Dishes:', this.order.dishes);
    console.log('Order SideDishes:', this.order.sideDishes);

    const headers = new HttpHeaders({
      Authorization: `Bearer ${localStorage.getItem('authToken')}`,
      'Content-Type': 'application/json',
    });

    const orderDto = {
      dishes: this.order.dishes.map((d: any) => {
        const dishId = this.getDishIdByName(d.name); 
        return { 
          dishId: dishId, 
          dishQuantity: d.quantity 
        };
      }),
      sideDishes: this.order.sideDishes.map((sd: any) => {
        const sideDishId = this.getSideDishIdByName(sd.name); 
        return { 
          sideDishId: sideDishId,
          sideDishQuantity: sd.quantity 
        };
      }),
    };

    this.http
      .post('http://localhost:5105/api/orders/addOrder', orderDto, { headers })
      .subscribe({
        next: (response) => {
          console.log('Order submitted successfully:', response);
          this.router.navigate(['/dashboard/menu/menu-items/order-success'], 
            {
              state: { order: this.orderService.getOrder() }
            }
          );
        },
        error: (error) => console.error('Error submitting order:', error),
      });
  }

  private getDishIdByName(name: string): string {
    if (!Array.isArray(this.dishesData)) {
      console.error('dishesData is not an array');
      return '';
    }
    const dish = this.dishesData.find(d => d.name == name);
    return dish ? dish.dishId : '';
  }
  
  private getSideDishIdByName(name: string): string {
    if (!Array.isArray(this.sideDishesData)) {
      console.error('sideDishesData is not an array');
      return '';
    }
    const sideDish = this.sideDishesData.find(sd => sd.name == name);
    return sideDish ? sideDish.sideDishId : '';
  }
}
