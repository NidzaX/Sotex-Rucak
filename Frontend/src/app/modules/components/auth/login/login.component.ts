import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { GoogleRegisterDto } from '../../../../core/models/google-register.dto';
import { AuthService } from '../../../../core/services/auth.service';
import { MenuService } from '../../../../core/services/menu.service';

@Component({
  selector: 'app-login',
  standalone: true,
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
})
export class LoginComponent {
  constructor(
    private authService: AuthService,
    private menuService: MenuService,
    private router: Router
  ) {}

  ngOnInit() {
    (window as any).handleGoogleSignIn = this.handleGoogleSignIn.bind(this);
  }

  async handleGoogleSignIn(response: any) {
    const token = response.credential;
    const jwt_decode = (await import('jwt-decode')).default;
    const decodedToken: any = jwt_decode(token);
    const email = decodedToken.email;
    const username = decodedToken.username;

    const formDataRegister: GoogleRegisterDto = {
      username,
      email,
      token,
      password: "", 
    };

    const formDataLogin = { email, token };

    try {
      await this.authService.registerWithGoogle(formDataRegister);

      const loginResult = await this.authService.signinWithGoogle(formDataLogin);

      if (loginResult.token) {
        localStorage.setItem('authToken', loginResult.token);

        this.menuService.checkMenuStatus().subscribe({
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
}
