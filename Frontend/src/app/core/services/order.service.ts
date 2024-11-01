import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class OrderService {
  private order: any = null;

  setOrder(order: any) {
    console.log("Setting order:", order); // Log for debugging
    this.order = order;
  }

  getOrder() { 
    console.log("Getting order:", this.order); // Log for debugging
    return this.order;
  }
}
