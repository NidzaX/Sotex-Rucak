import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class MenuService{
  private apiUrl = 'http://localhost:5105/api/menus/get-menu-items';

  constructor(private http: HttpClient) {}

  getDishes(): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/dishes`);
  }

  getSideDishes(): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/sideDishes`);
  }
}
