import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class MenuService{
  private apiUrl = 'http://localhost:5105/api/menus/get-menu-items';

  constructor(private http: HttpClient) {}

  getDishes(): Observable<any> {
    const headers = new HttpHeaders({
      'Authorization': `Bearer ${localStorage.getItem('authToken')}`
    });
    return this.http.get<any>(this.apiUrl, { headers });
  }
  
  getSideDishes(): Observable<any> {
    const headers = new HttpHeaders({
      'Authorization': `Bearer ${localStorage.getItem('authToken')}`
    });
    return this.http.get<any>(this.apiUrl, { headers });
  }
  
}
