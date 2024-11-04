import { HttpClient, HttpHeaders } from '@angular/common/http';
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

  private getHeaders(): HttpHeaders {
    return new HttpHeaders({
      Authorization: `Bearer ${localStorage.getItem('authToken')}`
    });
  }

  setOrder(order: any) {
    console.log("Setting order:", order); // Log for debugging
    this.order = order;
  }

  getOrder() { 
    console.log("Getting order:", this.order); // Log for debugging
    return this.order;
  }

  getUserOrders(): Observable<GetAllOrdersDto[]> {
    return this.http.get<GetAllOrdersDto[]>(this.getOrdersUrl, {headers: this.getHeaders()})
  }

  cancelOrder(orderId: string): Observable<any> {
    const headers = this.getHeaders();
    return this.http.post(`${this.apiUrl}/cancelOrder?orderId=${orderId}`, {}, { headers });
  }

  submitOrder(orderDto: NewOrderDto): Observable<any> {
    const headers = this.getHeaders();
    return this.http.post(`${this.addOrderUrl}`, orderDto, {headers: this.getHeaders()})
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
