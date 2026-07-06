import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RegisterRequest } from '../../models/register-request';
import { Auth } from '../../services/auth';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './register.html',
  styleUrl: './register.css',
})
export class Register {

  //ce trimitem la BE
  registerRequest: RegisterRequest =
  {
    name: '',
    email: '',
    password: '',
    confirmPassword: ''
  };

  constructor(
    private authService: Auth
  )
  {}

  register()
  {
    this.authService.register(this.registerRequest).subscribe(
      {
        next: response =>
        {
          console.log(response);
          alert('Register successful');
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
