import { Component, OnInit } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { OrderService } from '../../../../../core/services/order.service';
import { MenuService } from '../../../../../core/services/menu.service'; // Import the MenuService
import { GetAllOrdersDto } from '../../../../../core/models/GetAllOrdersDto';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { AuthService } from '../../../../../core/services/auth.service';
import { LogoutComponent } from "../../../auth/logout/logout.component";

@Component({
  selector: 'app-menu-items',
  standalone: true,
  imports: [RouterModule, CommonModule, FormsModule, LogoutComponent],
  templateUrl: './menu-items.component.html',
  styleUrls: ['./menu-items.component.css']
})
export class MenuItemsComponent implements OnInit {
  menuItems: any = [];                  
  orders: GetAllOrdersDto[] = [];      
  hasOrders: boolean = false;

  constructor(
    private router: Router,
    private orderService: OrderService,
    private menuService: MenuService,
    private authService: AuthService,
  ) {}

  ngOnInit() {
    this.fetchMenuItems();
    this.fetchUserOrders();
  }

  fetchMenuItems() {
    this.menuService.getDishes().subscribe({
      next: (response) => {
        this.menuItems = response;
        console.log('Fetched menu items:', this.menuItems);
      },
      error: (error) => console.error('Error fetching menu items:', error),
    });
  }

  fetchUserOrders() {
    this.orderService.getUserOrders().subscribe({
      next: (response: GetAllOrdersDto[]) => {
        this.orders = response;
        this.hasOrders = this.orders.length > 0; 
        console.log('Fetched user orders:', this.orders);
      },
      error: (error) => console.error('Error fetching orders:', error)
    });
  }

  reviewOrder() {
    const order = {
      dishes: this.menuItems.dishes.filter((dish: any) => dish.quantity > 0),
      sideDishes: this.menuItems.sideDishes.filter((sideDish: any) => sideDish.quantity > 0)
    };

    if(order.dishes.length === 0 && order.sideDishes.length === 0) {
      alert('Please select at least one dish or side dish before reviewing your order.');
      return;
    }

    this.orderService.setOrder(order);
    this.router.navigate(['/dashboard/menu/menu-items/review'], { state: { order } });
  }

  viewOrders() {
    this.router.navigate(['/dashboard/menu/user-orders']); 
  }

}
