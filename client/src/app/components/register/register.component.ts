import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { faEnvelope, faLock, faUser } from '@fortawesome/free-solid-svg-icons';
import { AccountService } from '../../_services/account.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  faEnvelope = faEnvelope; faLock = faLock; faUser = faUser;
  regUserForm! : FormGroup;
  constructor(public accountService: AccountService) { }

  ngOnInit(): void {
    this.regUserForm = new FormGroup({
      email: new FormControl(null),
      password: new FormControl(null),
      repeatPassword: new FormControl(null),
      username: new FormControl(null)
    });
  }

  register() {
    this.accountService.register(this.regUserForm.value).subscribe(res => {
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
