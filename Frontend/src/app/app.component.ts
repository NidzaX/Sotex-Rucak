import { Component, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { AuthInterceptor } from './core/interceptor/authInterceptor';
import { AuthService } from './core/services/auth.service';

@Component({
  selector: 'app-root',
  standalone: true,
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],  // Fixed the styleUrls syntax
  providers: [{
    provide: HTTP_INTERCEPTORS,
    useClass: AuthInterceptor,
    multi: true
  }],
  imports: [RouterOutlet]  // Imports needed for RouterOutlet
})
export class AppComponent implements OnInit {
  title = 'Frontend';
  isLoggedIn: boolean = false;
  
  constructor(private authService: AuthService) {}

  ngOnInit(): void {
    this.isLoggedIn = this.authService.isAuthenticated();
    this.refreshPage();
  }

  refreshPage() {
    if (!localStorage.getItem('pageRefreshed')) {
      localStorage.setItem('pageRefreshed', 'true');
      location.reload();
    } else {
      localStorage.removeItem('pageRefreshed');
    }
  }
}
