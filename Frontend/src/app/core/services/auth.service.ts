import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { firstValueFrom } from "rxjs";
import { GoogleRegisterDto } from "../models/google-register.dto";

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private readonly apiUrl = 'http://localhost:5105/api/users';

  constructor(private http: HttpClient) {}

  private toFormData(data: GoogleRegisterDto): FormData {
    const formData = new FormData();
    formData.append('Username', data.username);
    if (data.password) {
      formData.append('Password', data.password);
    }
    formData.append('Email', data.email);
    formData.append('Token', data.token);
    return formData;
  }

  async registerWithGoogle(data: GoogleRegisterDto) {
    const formData = this.toFormData(data);
    return firstValueFrom(this.http.post<any>(`${this.apiUrl}/register-google`, formData));
  }

  async signinWithGoogle(data: Pick<GoogleRegisterDto, 'email' | 'token'>) {
    const formData = new FormData();
    formData.append('Email', data.email);
    formData.append('Token', data.token);
    return firstValueFrom(this.http.post<any>(`${this.apiUrl}/signin-google`, formData));
  }
}
