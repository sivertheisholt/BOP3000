import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { faEnvelope, faLock, faUser } from '@fortawesome/free-solid-svg-icons';
import { Country } from 'src/app/_models/country.model';
import { ValidationService } from 'src/app/_services/validation.service';
import { AuthService } from '../../_services/auth.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  faEnvelope = faEnvelope; faLock = faLock; faUser = faUser;
  regUserForm! : FormGroup;
  countries : Country[] = [];
  genders = ['Male', 'Female', 'Other', 'Hidden'];

  constructor(private authService: AuthService, private validationService: ValidationService, private route: ActivatedRoute) { }

  ngOnInit(): void {
    //this.countries = this.route.snapshot.data['countries'];
    this.regUserForm = new FormGroup({
      email: new FormControl(null, [Validators.required, Validators.email]),
      password: new FormControl(null, Validators.required),
      repeatPassword: new FormControl(null, Validators.required),
      username: new FormControl(null, [Validators.required, Validators.pattern(this.validationService.regexUsername)]),
      country: new FormControl(null, Validators.required),
      gender: new FormControl(null, Validators.required)
    });
  }

  register() {
    this.authService.register(this.regUserForm.value).subscribe(res => {
      console.log('res');
      console.log(res);
    }, err => {
      console.log('error');
      console.log(err);
    })
  }

  cancel() {
    console.log('cancelled');
  }
}
