import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { MenuItemsComponent } from "./menu-items/menu-items.component";
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-menu',
  standalone: true,
  imports: [FormsModule, MenuItemsComponent, CommonModule],
  templateUrl: './menu.component.html',
  styleUrl: './menu.component.css'
})
export class MenuComponent {
  selectedFile: File | null = null;
  uploadedMenu: any = null;
  isMenuAvailable: boolean = false;

  constructor(private http: HttpClient, private router: Router) {}

  onFileSelected(event: any) {
    const file: File = event.target.files[0];
    if(file){
      this.selectedFile = file;
    }
  }

  checkMenuStatus() {
    const headers = new HttpHeaders({
      Authorization: `Bearer ${localStorage.getItem('authToken')}`
    });

    this.http
    .get('http://localhost:5105/api/menus/get-menu-status', {headers})
      .subscribe({
        next: (response: any) => {
          this.isMenuAvailable = response.isActive || response.isActiveTomorrow;
          if(this.isMenuAvailable) {
            this.router.navigate(['/dashboard/menu/menu-items']);
          }
        },
        error: (error) => console.error('Error checking menu status:', error)
      });
  }

  uploadMenu() {
    if(!this.selectedFile) {
      console.error('file', this.selectedFile);
      return;
    }

    const formData = new FormData();
    formData.append('file', this.selectedFile);

    const headers = new HttpHeaders({
      Authorization: `Bearer ${localStorage.getItem('authToken')}`
    });

    this.http
    .post('http://localhost:5105/api/menus/parse-and-save-menu', formData, {headers})
      .subscribe({
        next: (response) => {
          console.log("Menu uploaded successfully:", response);
          this.router.navigate(['/menu-items']);
        },
        error: (error) => console.error("Error uploading menu:", error)
      });
  }
}
