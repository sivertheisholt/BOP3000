import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { faCheckCircle, faEnvelope, faLock, faUser, faStopCircle } from '@fortawesome/free-solid-svg-icons';
import { Country } from 'src/app/_models/country.model';
import { CustomValidator } from 'src/app/_validators/custom-validator';
import { passwordMatchingValidator } from 'src/app/_validators/password-matching';
import { AuthService } from '../../_services/auth.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  faEnvelope = faEnvelope; faLock = faLock; faUser = faUser; faCheckCircle = faCheckCircle; faStopCircle = faStopCircle;
  regUserForm! : FormGroup;
  countries : Country[] = [];
  genders = ['Male', 'Female', 'Other', 'Hidden'];

  constructor(private authService: AuthService, private route: ActivatedRoute) { }

  ngOnInit(): void {
    this.countries = this.route.snapshot.data['countries'];
    this.regUserForm = new FormGroup({
      email: new FormControl(null, [Validators.required, Validators.email]),
      password: new FormControl(null, Validators.compose([
        Validators.required,
        CustomValidator.patternValidator(/\d/, { hasNumber: true }),
        CustomValidator.patternValidator(/[A-Z]/, { hasCapitalCase: true }),
        CustomValidator.patternValidator(/[a-z]/, { hasSmallCase: true }),
        CustomValidator.patternValidator(/[^A-Za-z0-9]/, { hasSpecialCharacter: true }),
        Validators.minLength(8)
      ])),
      repeatPassword: new FormControl(null, Validators.required),
      username: new FormControl(null, [Validators.required, CustomValidator.patternValidator(/^[A-Za-z0-9 ]+$/, { hasSpecialCharacter: true })]),
      gender: new FormControl(null, Validators.required),
      countryId: new FormControl(null, Validators.required)
    },{
      validators: passwordMatchingValidator
    }
    );
  }

  register() {
    if(this.regUserForm.valid){
      this.authService.register(this.regUserForm.value).subscribe(res => {
        console.log('res');
        console.log(res);
    }, err => {
        console.log('error');
        console.log(err);
      });
    }
  }

  cancel() {
    console.log('cancelled');
  }
}
