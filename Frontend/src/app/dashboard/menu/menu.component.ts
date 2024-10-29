import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-menu',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './menu.component.html',
  styleUrl: './menu.component.css'
})
export class MenuComponent {
  selectedFile: File | null = null;
  purpose: string = '';
  orderNote: string = '';

  constructor(private http: HttpClient) {}

  onFileSelected(event: any) {
    const file: File = event.target.files[0];
    if(file){
      this.selectedFile = file;
    }
  }

  uploadMenu() {
    if(!this.selectedFile || !this.purpose) {
      console.error('file', this.selectedFile);
      return;
    }

    const formData = new FormData();
    formData.append('file', this.selectedFile);
    formData.append('purpose', this.purpose);

    const headers = new HttpHeaders({
      Authorization: `Bearer ${localStorage.getItem('authToken')}`
    });

    this.http.post('http://localhost:5105/api/menus/parse-and-save-menu', formData, {headers})
      .subscribe({
        next: (response) => console.log('Menu uploaded successfully:', response),
        error: (error) => console.error('Error uploading menu:', error)
      })
  }

}
