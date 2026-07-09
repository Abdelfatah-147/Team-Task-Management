import { inject } from "@angular/core";
import { CanActivateFn, Router } from "@angular/router";
import { AuthService } from "../services/auth.service";

export const roleGuard: CanActivateFn = (route, state) => {
    const authService = inject(AuthService);
    const router = inject(Router);
    const allowedRoles : string[] = route.data['roles'] ?? [];
    const userRoles = authService.roles();
    const hasAccess = allowedRoles.some((role) => userRoles.includes(role));

    if (hasAccess){ 
        return true;
    }

    router.navigate(['/unauthorized']);
    return false;
}