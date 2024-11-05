import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { GetAllOrdersDto } from '../models/GetAllOrdersDto';
import { Observable } from 'rxjs';
import { NewOrderDto } from '../models/NewOrderDto';

@Injectable({
  providedIn: 'root'
})
export class OrderService {
  private orderSubmitted = false;
  private order: any = null;
  private readonly apiUrl = 'http://localhost:5105/api/orders';
  private readonly getOrdersUrl = `${this.apiUrl}/getUserOrders`;
  private readonly addOrderUrl = `${this.apiUrl}/addOrder`;
  constructor(private http: HttpClient) {}

  setOrder(order: any) {
    console.log("Setting order:", order); // Log for debugging
    this.order = order;
  }

  getOrder() { 
    console.log("Getting order:", this.order); // Log for debugging
    return this.order;
  }

  getUserOrders(): Observable<GetAllOrdersDto[]> {
    return this.http.get<GetAllOrdersDto[]>(this.getOrdersUrl)
  }

  cancelOrder(orderId: string): Observable<any> {
    return this.http.post(`${this.apiUrl}/cancelOrder?orderId=${orderId}`, {});
  }

  submitOrder(orderDto: NewOrderDto): Observable<any> {
    return this.http.post(`${this.addOrderUrl}`, orderDto)
  }

  setOrderSubmitted(status: boolean) {
    this.orderSubmitted = status;
  }

  isOrderSubmitted(): boolean {
    return this.orderSubmitted;
  }
  
  clearOrder() {
    this.order = null;
  }
}
