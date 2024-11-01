import { Component } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { firstValueFrom } from 'rxjs';

declare const google: any; // Declare Google API

@Component({
  selector: 'app-login',
  standalone: true,
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {
  constructor(private http: HttpClient, private router: Router) {}

  ngOnInit() {
    (window as any).handleGoogleSignIn = this.handleGoogleSignIn.bind(this);
  }

  async handleGoogleSignIn(response: any) {
    const token = response.credential; // Google token
    console.log('Google token:', token);
  
    const jwt_decode = (await import('jwt-decode')).default;
    const decodedToken: any = jwt_decode(token);
    console.log('Decoded Token:', decodedToken);  // Check the structure
    const email = decodedToken.email;
  
    const formData = new FormData();
    formData.append('Email', email);
    formData.append('Token', token);
  
    try {
      const result = await firstValueFrom(this.http.post<any>(
        'http://localhost:5105/api/users/signin-google',
        formData
      ));
      
      console.log('API response:', result); // Log the entire response
  
      // Change here: Use result.token instead of result.Token
      if (result.token) {
        localStorage.setItem('authToken', result.token);
        console.log('Token stored in localStorage:', result.token); // Log what you're storing
        
        // Check menu status after storing the token
        this.checkMenuStatus().subscribe({
          next: (response: any) => {
            if (response.isActive || response.isActiveTomorrow) {
              // Redirect to menu items if there's an active menu
              this.router.navigate(['/dashboard/menu/menu-items']);
            } else {
              // Otherwise, redirect to the upload menu page
              this.router.navigate(['/dashboard/menu']);
            }
          },
          error: (error) => {
            console.error('Error checking menu status:', error);
            // Handle error case, possibly redirect to an error page or show a message
            this.router.navigate(['/error']);
          }
        });
      } else {
        console.error('Token is undefined in the response');
        // Handle the case when Token is not in the response
      }
    } catch (error: any) { // Specify that error is of type 'any'
      console.error('Error during login:', error);
  
      // Check if error has a specific structure and log details accordingly
      if (error.error) {
        console.error('API Error Response:', error.error);
      } else {
        console.error('Unknown error:', error); // Log the unknown error
      }
  
      this.router.navigate(['/error']);
    }
  }

  checkMenuStatus() {
    const headers = { Authorization: `Bearer ${localStorage.getItem('authToken')}` };
    return this.http.get('http://localhost:5105/api/menus/get-menu-status', { headers });
  }
}
