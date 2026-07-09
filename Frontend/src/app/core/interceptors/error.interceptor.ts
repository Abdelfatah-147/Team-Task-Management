import { HttpErrorResponse, HttpInterceptor, HttpInterceptorFn } from "@angular/common/http";
import { inject } from "@angular/core";
import { AuthService } from "../services/auth.service";
import { Router } from "@angular/router";
import { catchError, throwError } from "rxjs";


function extractErrorMessage(error: HttpErrorResponse) : string {
    if (typeof error.error === 'string')
        return error.error;

    if (error.error && typeof error.error === 'object') {
        if (error.error.errors){
            const messages = Object.values(error.error.errors).flat();
            return messages.join(' | ');
        }
        if (error.error.message)
            return error.error.message;
        if (error.error.title)
            return error.error.title;
    }

    switch (error.status) {
    case 0:
      return 'Unable to reach the server. Please check your connection.';
    case 401:
      return 'Your session has expired, or the credentials are invalid.';
    case 403:
      return 'You do not have permission to perform this action.';
    case 404:
      return 'The requested resource was not found.';
    case 500:
      return 'A server error occurred. Please try again later.';
    default:
      return 'An unexpected error occurred.';
    }
}


export const errorInterceptor: HttpInterceptorFn = (req, next) => {
    const authService = inject(AuthService);
    const router = inject(Router);

    return next(req).pipe(
        catchError((error : HttpErrorResponse) => {
            const message = extractErrorMessage(error);
            const isAuthRequest = req.url.includes('/auth/login') || req.url.includes('/auth/register');
            if (error.status === 401 && !isAuthRequest){
                authService.logout();
                router.navigate(['/login']);
            }

            return throwError(() => Error(message));
        })
    );
};
