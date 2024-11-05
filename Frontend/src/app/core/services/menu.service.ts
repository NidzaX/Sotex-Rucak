import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { AddMenuDto } from '../models/AddMenuDto';

@Injectable({
  providedIn: 'root'
})
export class MenuService {

  constructor(private http: HttpClient) {}

  private getHeaders(): HttpHeaders {
    return new HttpHeaders({
      Authorization: `Bearer ${localStorage.getItem('authToken')}`,
    });
  }

  getDishes(): Observable<any> {
    return this.http.get<any>('/api/menus/get-menu-items', { headers: this.getHeaders() });
  }

  getSideDishes(): Observable<any> {
    return this.http.get<any>('/api/menus/get-menu-items', { headers: this.getHeaders() });
  }

  checkMenuStatus(): Observable<any> {
    console.log('Checking menu status...');
    return this.http.get<any>('api/menus/get-menu-status', {
      headers: this.getHeaders(),
      responseType: 'json' as const 
    });
  }  

  uploadMenu(formData: FormData): Observable<AddMenuDto> {
    return this.http.post<AddMenuDto>('api/menus/parse-and-save-menu', formData, { headers: this.getHeaders() });
  }
}
