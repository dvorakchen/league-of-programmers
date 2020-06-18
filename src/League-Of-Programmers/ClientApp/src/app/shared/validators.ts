import { Directive } from '@angular/core';
import { ValidatorFn, AbstractControl, NG_VALIDATORS, Validator, ValidationErrors } from '@angular/forms';

/**
 * 对比两个字符串是否相同的验证器
 *
 * <form appMustSame></form>
 * newPassword 和 confirmPassword 是两个要对比的值
 */
export const passwordMustSameValidator: ValidatorFn|null = (control: AbstractControl): ValidationErrors | null => {
        const v1 = control.get('newPassword');
        const v2 = control.get('confirmPassword');
        return mustSame(v1?.value, v2?.value);
    };

function mustSame(value1: string, value2: string): ValidationErrors | null {
    return value1 && value2 && value1 === value2 ? null : {mustSame: true};
}
/**
 * 对比两个密码是否相同的指令
 *
 * <form appMustSame></form>
 * newPassword 和 confirmPassword 是两个要对比的值
 */
@Directive({
  selector: '[appPasswordMustSame]',
  providers: [{provide: NG_VALIDATORS, useExisting: PasswordMustSameValidatorDirective, multi: true}]
})
export class PasswordMustSameValidatorDirective implements Validator {
  validate(control: AbstractControl): ValidationErrors | null {
    return passwordMustSameValidator(control);
  }
}
