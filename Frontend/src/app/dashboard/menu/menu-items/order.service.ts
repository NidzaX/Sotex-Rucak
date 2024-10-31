import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class OrderService {
  private order: any = null;

  setOrder(order: any) {
    this.order = order;
  }

  getOrder() { 
    return this.order;
  }
}
