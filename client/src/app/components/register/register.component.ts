import { Component, OnInit } from '@angular/core';
import { faEnvelope, faLock, faUser } from '@fortawesome/free-solid-svg-icons';
import { AccountService } from '../../_services/account.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  faEnvelope = faEnvelope; faLock = faLock; faUser = faUser;
  model: any = {}
  constructor(public accountService: AccountService) { }

  ngOnInit(): void {
  }

  register() {
    this.accountService.register(this.model).subscribe(res => {
      console.log(res);
    }, err => {
      console.log(err);
    })
  }

  cancel() {
    console.log('cancelled');
  }
}
