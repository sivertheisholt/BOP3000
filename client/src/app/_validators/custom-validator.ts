import { AbstractControl, ValidationErrors, ValidatorFn } from "@angular/forms";
export class CustomValidator {
    static patternValidator(regex: RegExp, error: ValidationErrors): ValidatorFn{
        return (control: AbstractControl): { [key: string]: any } => {
          if (!control.value) {
            return null as any;
          }
      
          const valid = regex.test(control.value);
      
          return valid ? null : error as any;
        };
    }
}