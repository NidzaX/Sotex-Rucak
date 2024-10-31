import { Component, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { LoginComponent } from "./login/login.component";

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, LoginComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent implements OnInit{
  title = 'Frontend';

  ngOnInit(): void {
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
