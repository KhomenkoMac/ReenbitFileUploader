import { AbstractControl, ValidationErrors, ValidatorFn } from '@angular/forms';

export function fileTypeValidator(allowedFileType: string) {
  return (control: AbstractControl): ValidationErrors | null => {
    const file: File = control.value
    const isOfSuchFromat = allowedFileType === file?.type.toString()
    return isOfSuchFromat ? null : {fileTypeIsValid: {val: control.value}}
  };
}
