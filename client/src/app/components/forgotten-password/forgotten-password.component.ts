import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { faEnvelope } from '@fortawesome/free-solid-svg-icons';

@Component({
  selector: 'app-forgotten-password',
  templateUrl: './forgotten-password.component.html',
  styleUrls: ['./forgotten-password.component.css']
})
export class ForgottenPasswordComponent implements OnInit {
  forgottenPasswordForm! : FormGroup;
  faEnvelope = faEnvelope;

  constructor() { }

  ngOnInit(): void {
    this.forgottenPasswordForm = new FormGroup({
      email: new FormControl(null, [Validators.required, Validators.email])
    })
  }

  onSubmit(){
    console.log(this.forgottenPasswordForm);
  }

}
