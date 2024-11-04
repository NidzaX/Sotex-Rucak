import { Component, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { LoginComponent } from './modules/components/auth/login/login.component';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { AuthInterceptor } from './core/interceptor/authInterceptor';
import { AuthService } from './core/services/auth.service';

@Component({
  selector: 'app-root',
  standalone: true,
  providers: [{
    provide: HTTP_INTERCEPTORS,
    useClass: AuthInterceptor,
    multi: true
  }],
  imports: [RouterOutlet, LoginComponent],
  template:'<router-outlet></router-outlet>',
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent implements OnInit{
  title = 'Frontend';
  isLoggedIn: boolean = false;
  
  constructor(private authService: AuthService) {}

  ngOnInit(): void {
    this.isLoggedIn = this.authService.isAuthenticated();
    this.refreshPage();
  }

  refreshPage(){
   if(!localStorage.getItem('pageRefreshed')) {
    localStorage.setItem('pageRefreshed', 'true');
    location.reload();
   } else {
    localStorage.removeItem('pageRefreshed');
   }
  }
}
