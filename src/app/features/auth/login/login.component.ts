import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../../../core/auth.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
})
export class LoginComponent {
  formData = {
    email: '',
    password: '',
  };

  error = '';

  constructor(private authService: AuthService, private router: Router) {}

  onSubmit(): void {
    this.authService.login(this.formData).subscribe({
      next: (res) => {
        this.authService.saveUser(res);
        this.router.navigate(['/']);
      },
      error: () => {
        this.error = 'Invalid email or password';
      },
    });
  }
}
