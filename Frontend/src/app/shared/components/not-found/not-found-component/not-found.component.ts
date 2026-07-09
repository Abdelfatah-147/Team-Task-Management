import { Component } from '@angular/core';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-not-found',
  standalone: true,
  imports: [RouterLink],
  template: `
    <div style="display:flex;flex-direction:column;align-items:center;justify-content:center;height:100vh;gap:1rem;font-family:Inter,sans-serif;">
      <h1 style="font-size:1.5rem;">Page not found</h1>
      <p style="color:#6b7280;">The page you're looking for doesn't exist.</p>
      <a routerLink="/dashboard" style="color:#5b5fef;font-weight:600;">Back to dashboard</a>
    </div>
  `
})

export class NotFoundComponent {}