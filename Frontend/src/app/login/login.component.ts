import { Component } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import jwt_decode from 'jwt-decode'; // Use default import
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

    const decodedToken: any = jwt_decode(token); 
    const email = decodedToken.email; 

    const data = {
      Email: email, 
      Token: token, 
    };

    try {
      
      const result = await firstValueFrom(this.http.post<any>(
        'http://localhost:5105/api/users/signin-google',
        data,
        { headers: { 'Content-Type': 'application/json' } }
      ))
      
      console.log('Login successful:', result);

      localStorage.setItem('authToken', result.Token);

      this.router.navigate(['/dashboard']);
    } catch (error) {
      console.error('Error during login:', error);
    
      this.router.navigate(['/error']);
    }
  }
}
