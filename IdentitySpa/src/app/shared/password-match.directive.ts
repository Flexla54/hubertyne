import { AbstractControl, ValidationErrors, ValidatorFn } from '@angular/forms';

export const PasswordMatchValidator: ValidatorFn = (
  control: AbstractControl
): ValidationErrors | null => {
  const password1 = control.get('pw1');
  const password2 = control.get('pw2');

  return password1 && password2 && password1.value !== password2.value
    ? { passwordDiffers: true }
    : null;
};
