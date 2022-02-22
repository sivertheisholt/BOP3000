import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { faEnvelope, faLock, faUser } from '@fortawesome/free-solid-svg-icons';
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
  constructor(private authService: AuthService, private validationService: ValidationService) { }

  ngOnInit(): void {
    this.regUserForm = new FormGroup({
      email: new FormControl(null, [Validators.required, Validators.email]),
      password: new FormControl(null, Validators.required),
      repeatPassword: new FormControl(null, Validators.required),
      username: new FormControl(null, [Validators.required, Validators.pattern(this.validationService.regexUsername)])
    }
    );
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
