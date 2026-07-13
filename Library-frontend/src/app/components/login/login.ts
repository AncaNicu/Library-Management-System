import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { FormControl, Validators } from '@angular/forms';
import { LoginRequest } from '../../models/login-request';
import { Auth } from '../../services/auth';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './login.html',
  styleUrl: './login.css',
})
export class Login {
  
  loginRequest: LoginRequest =
  {
    email: '',
    password: ''
  };

  constructor(
    private authService: Auth,
    private router: Router
  )
  {}

  login()
  {
    this.authService.login(this.loginRequest).subscribe(
      {
        next: response =>
        {
          this.authService.saveLogin(response);
          console.log(response);
          alert('Login successful');

          this.router.navigate(['/books']);
        },
        error: error => 
        {
          console.log(error);
          alert(error.error);
        }
      }
    );
  }
}
