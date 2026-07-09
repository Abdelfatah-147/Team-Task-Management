import { Injectable, signal, computed } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, tap, catchError, of } from 'rxjs';
import { LoginDto } from '../models/auth/login.dto';
import { RegisterDto } from '../models/auth/register.dto';
import { AuthResponseDto } from '../models/auth/auth-response.dto';
import { UserProfileDto } from '../models/user/user-profile.dto';
import { environment } from '../../../environments/environment';

const TOKEN_KEY = 'auth_token';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private readonly authUrl = `${environment.apiUrl}/auth`;
  private readonly userUrl = `${environment.apiUrl}/user`;

  private readonly _currentUser = signal<UserProfileDto | null>(null);
  private readonly _isInitializing = signal<boolean>(true);

  readonly currentUser = this._currentUser.asReadonly();
  readonly isInitializing = this._isInitializing.asReadonly();
  readonly isAuthenticated = computed(() => !!this._currentUser());
  readonly roles = computed(() => this._currentUser()?.roles ?? []);

  constructor(private http: HttpClient) {}

  login(dto: LoginDto): Observable<AuthResponseDto> {
    return this.http.post<AuthResponseDto>(`${this.authUrl}/login`, dto).pipe(
      tap((response) => this.storeToken(response.token)),
      tap(() => this.loadCurrentUser().subscribe())
    );
  }

  register(dto: RegisterDto): Observable<AuthResponseDto> {
    return this.http.post<AuthResponseDto>(`${this.authUrl}/register`, dto).pipe(
      tap((response) => this.storeToken(response.token)),
      tap(() => this.loadCurrentUser().subscribe())
    );
  }

  logout(): void {
    localStorage.removeItem(TOKEN_KEY);
    this._currentUser.set(null);
  }

  getToken(): string | null {
    return localStorage.getItem(TOKEN_KEY);
  }

  loadCurrentUser(): Observable<UserProfileDto | null> {
    if (!this.getToken()) {
      this._currentUser.set(null);
      this._isInitializing.set(false);
      return of(null);
    }

    return this.http.get<UserProfileDto>(`${this.userUrl}/me`).pipe(
      tap((profile) => this._currentUser.set(profile)),
      catchError(() => {
        // Token is invalid or expired
        this.logout();
        return of(null);
      }),
      tap(() => this._isInitializing.set(false))
    );
  }

  private storeToken(token: string): void {
    localStorage.setItem(TOKEN_KEY, token);
  }
}