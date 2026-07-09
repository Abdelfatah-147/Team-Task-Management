import { AbstractControl, ValidationErrors, ValidatorFn } from "@angular/forms";



export function passwordMatchValidator(passwordKey: string = 'password', confirmPasswordKey: string = 'confirmPassword') : ValidatorFn {
    return (group: AbstractControl) : ValidationErrors | null => {
        const password = group.get(passwordKey);
        const confirmPassword = group.get(confirmPasswordKey);

        if(!password || !confirmPassword) {
            return null;
        }
        
        if (confirmPassword.value && confirmPassword.value !== password.value) {
            confirmPassword.setErrors({ ...confirmPassword.errors, passwordMismatch: true });
        } else if (confirmPassword.errors) {
            const { passwordMismatch, ...rest } = confirmPassword.errors;
            confirmPassword.setErrors(Object.keys(rest).length ? rest : null);
        }

        return null;
    }
}