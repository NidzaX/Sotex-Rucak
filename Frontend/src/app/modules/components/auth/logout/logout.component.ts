import { Component } from '@angular/core';
import { AuthService } from '../../../../core/services/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-logout',
  standalone: true,
  imports: [],
  templateUrl: './logout.component.html',
  styleUrl: './logout.component.css'
})
export class LogoutComponent {
  constructor(private authService: AuthService, private router: Router) {}

  logout() {
    this.authService.signOut().then(() => {
      console.log('Logged out successfully');
      this.router.navigate(['/login']).then(() => {
        location.reload();
      });
    }).catch(error => {
      console.error('Logout error:', error);
    })
  }
}
