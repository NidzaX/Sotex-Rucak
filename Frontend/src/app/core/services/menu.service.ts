// src/app/core/services/menu.service.ts
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class MenuService {
  private readonly apiUrl = 'http://localhost:5105/api/menus';
  private readonly menuStatusUrl = `${this.apiUrl}/get-menu-status`;

  constructor(private http: HttpClient) {}

  private getHeaders(): HttpHeaders {
    return new HttpHeaders({
      Authorization: `Bearer ${localStorage.getItem('authToken')}`,
    });
  }

  getDishes(): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/get-menu-items`, { headers: this.getHeaders() });
  }

  getSideDishes(): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/get-menu-items`, { headers: this.getHeaders() });
  }

  checkMenuStatus(): Observable<any> {
    return this.http.get<any>(this.menuStatusUrl, { headers: this.getHeaders() });
  }
}
