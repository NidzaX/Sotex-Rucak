import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { firstValueFrom } from 'rxjs';
import { Router } from '@angular/router';
@Component({
  selector: 'app-register',
  standalone: true,
  imports: [],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css'
})
export class RegisterComponent implements OnInit {
  constructor(private http: HttpClient, private router: Router) {}

  ngOnInit() {
    // Set up Google Sign-In callback function
    (window as any).handleGoogleSignIn = this.handleGoogleSignIn.bind(this);
  }

  async handleGoogleSignIn(response: any) {
    const token = response.credential; // Google token from the callback
    console.log('Google token:', token);

    const jwt_decode = (await import('jwt-decode')).default;
    const decodedToken: any = jwt_decode(token);
    console.log('Decoded Token:', decodedToken);  // Log decoded data if needed
    const username = decodedToken.username;
    const password = decodedToken.password;
    const email = decodedToken.email;

    const formData = new FormData();
    formData.append('Username', username);
    formData.append('Password', password);
    formData.append('Email', email);
    formData.append('Token', token);

    try {
      const result = await firstValueFrom(this.http.post<any>(
        'http://localhost:5105/api/users/register-google',
        formData
      ));
      
      console.log('API response:', result);

      // Handle successful registration and navigate to desired page
      this.router.navigate(['/dashboard']);
    } catch (error: any) {
      console.error('Error during registration:', error);

      // Navigate to error page or show an error message
      this.router.navigate(['/error']);
    }
  }
}