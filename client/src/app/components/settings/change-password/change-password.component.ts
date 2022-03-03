import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { AuthService } from 'src/app/_services/auth.service';
import { GamesService } from 'src/app/_services/games.service';
import { CustomValidator } from 'src/app/_validators/custom-validator';
import { passwordMatchingValidator } from 'src/app/_validators/password-matching';

@Component({
  selector: 'app-change-password',
  templateUrl: './change-password.component.html',
  styleUrls: ['./change-password.component.css']
})
export class ChangePasswordComponent implements OnInit {
  changePasswordForm! : FormGroup;
  submitted: boolean = false;
  constructor(private authService: AuthService) { }

  ngOnInit(): void {
    
    this.changePasswordForm = new FormGroup({
      currentPassword: new FormControl(null, Validators.required),
      password: new FormControl(null, Validators.compose([
        Validators.required,
        CustomValidator.patternValidator(/\d/, { hasNumber: true }),
        CustomValidator.patternValidator(/[A-Z]/, { hasCapitalCase: true }),
        CustomValidator.patternValidator(/[a-z]/, { hasSmallCase: true }),
        CustomValidator.patternValidator(/[^A-Za-z0-9]/, { hasSpecialCharacter: true }),
        Validators.minLength(8)
      ])),
      repeatPassword: new FormControl(null, Validators.required)
    },
    {
      validators: passwordMatchingValidator 
    }
    );
  }

  onSubmit(){
    this.submitted = true;
    if(this.changePasswordForm.valid){
      this.authService.updateUserPassword({
        currentPassword: this.changePasswordForm.value.currentPassword,
        newPassword: this.changePasswordForm.value.password
      });
    }
  }

}
