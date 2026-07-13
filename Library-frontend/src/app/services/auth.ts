import { Injectable, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { RegisterRequest } from '../models/register-request';
import { Observable } from 'rxjs';
import { LoginRequest } from '../models/login-request';
import { LoginResponse } from '../models/login-response';
import { RegisterResponse } from '../models/register-response';

@Injectable({
  providedIn: 'root',
})
export class Auth {

  private baseUrl = 'http://localhost:5041/api';

  crtUserName = signal(localStorage.getItem('name') || null);

  constructor(private http: HttpClient) {}

  register(registerData: RegisterRequest): Observable<RegisterResponse>
  {
    return this.http.post<RegisterResponse>(`${this.baseUrl}/auth/register`, registerData);
  }

  login(loginData: LoginRequest): Observable<LoginResponse>
  {
    return this.http.post<LoginResponse>(`${this.baseUrl}/auth/login`, loginData);
  }

  saveLogin(loginResponse: LoginResponse)
  {
    localStorage.setItem('token', loginResponse.token);
    localStorage.setItem('userId', loginResponse.userId.toString());
    localStorage.setItem('name', loginResponse.name);  

    this.crtUserName.set(loginResponse.name);
  }

  logout()
  {
    localStorage.removeItem('token');
    localStorage.removeItem('userId');
    localStorage.removeItem('name');

    this.crtUserName.set(null);
  }

  isLoggedIn(): boolean
  {
    return !!localStorage.getItem('token');
  }

}
