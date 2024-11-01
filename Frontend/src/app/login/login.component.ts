import { Component } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { firstValueFrom } from 'rxjs';

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
    const jwt_decode = (await import('jwt-decode')).default;
    const decodedToken: any = jwt_decode(token);
    const email = decodedToken.email;
    const username = decodedToken.username;
    const password = decodedToken.password;

    const formDataRegister = new FormData();
    formDataRegister.append('Username', username);
    formDataRegister.append('Password', password);
    formDataRegister.append('Email', email);
    formDataRegister.append('Token', token);

    const formDataLogin = new FormData();
    formDataLogin.append('Email', email);
    formDataLogin.append('Token', token);

    try {
      await firstValueFrom(this.http.post<any>(
        'http://localhost:5105/api/users/register-google',
        formDataRegister
      ));

      const loginResult = await firstValueFrom(this.http.post<any>(
        'http://localhost:5105/api/users/signin-google',
        formDataLogin
      ));

      if (loginResult.token) {
        localStorage.setItem('authToken', loginResult.token);
        this.checkMenuStatus().subscribe({
          next: (response: any) => {
            if (response.isActive || response.isActiveTomorrow) {
              this.router.navigate(['/dashboard/menu/menu-items']);
            } else {
              this.router.navigate(['/dashboard/menu']);
            }
          },
          error: () => this.router.navigate(['/error']),
        });
      } else {
        console.error('Token is undefined in the response');
      }
    } catch (error: any) {
      console.error('Error during registration/login:', error);
      this.router.navigate(['/error']);
    }
  }

  checkMenuStatus() {
    const headers = { Authorization: `Bearer ${localStorage.getItem('authToken')}` };
    return this.http.get('http://localhost:5105/api/menus/get-menu-status', { headers });
  }
}
