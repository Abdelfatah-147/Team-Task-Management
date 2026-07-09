import { Component, inject, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { AuthService } from '../../../core/services/auth.service';
import { Router, RouterLink } from '@angular/router';
import { passwordMatchValidator } from '../../../core/validators/password-match.validator';
import { RegisterDto } from '../../../core/models/auth/register.dto';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [ReactiveFormsModule, RouterLink],
  templateUrl: './register.component.html',
  styleUrl: './register.component.scss'
})
export class RegisterComponent {
  readonly isSubmitting = signal(false);
  readonly errorMessage = signal<string | null>(null);
  
  private readonly fb = inject(FormBuilder);
  private readonly authService = inject(AuthService);
  private readonly router = inject(Router);

  readonly form = this.fb.group({
    fullName: ['', [Validators.required, Validators.minLength(2)]],
    userName: ['', [Validators.required, Validators.minLength(3)]],
    email: ['', [Validators.required, Validators.email]],
    password: ['', [Validators.required, Validators.minLength(8), Validators.pattern(/[A-Z]/), Validators.pattern(/[0-9]/)]],
    confirmPassword: ['', [Validators.required]]
  }, 
  {validators: passwordMatchValidator('password', 'confirmPassword')});

  submit() : void {
    if (this.form.invalid){
      this.form.markAllAsTouched();
      return;
    }

    this.errorMessage.set(null);
    this.isSubmitting.set(true);

    const dto = this.form.getRawValue() as RegisterDto;

    this.authService.register(dto).subscribe({
      next: () => this.router.navigateByUrl('/dashboard'),
      error: (err: Error) => {
        this.errorMessage.set(err.message);
        this.isSubmitting.set(false);
      }
    });
  }


  get fullName() { return this.form.controls.fullName; }
  get userName() { return this.form.controls.userName; }
  get email() { return this.form.controls.email; }
  get password() { return this.form.controls.password; }
  get confirmPassword() { return this.form.controls.confirmPassword; }


  

}
