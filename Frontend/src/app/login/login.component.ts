import { Component } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';

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
    // Bind Google sign-in handler to window object
    (window as any).handleGoogleSignIn = this.handleGoogleSignIn.bind(this);
  }

  async handleGoogleSignIn(response: any) {
    const token = response.credential; // Google token
  
    // Log the token to verify it is being received correctly
    console.log('Google token:', token); 
  
    const data = {
      email: '', // Optional, extracted from the backend if needed
      token: token,
    };
  
    try {
      // Send the Google token to your backend
      const result = await this.http.post<any>(
        'http://localhost:5105/api/users/signin-google',
        data,
        { headers: { 'Content-Type': 'application/json' } }
      ).toPromise();
  
      console.log('Login successful:', result);
  
      // Store the JWT token from your backend for future requests
      localStorage.setItem('authToken', result.token);
  
      // Redirect to dashboard after successful login
      this.router.navigate(['/dashboard']);
    } catch (error) {
      console.error('Error during login:', error);
  
      // Redirect to error page if login fails
      this.router.navigate(['/error']);
    }
  }
  
}
