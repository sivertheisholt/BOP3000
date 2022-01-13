import { Component, OnInit } from '@angular/core';
import { faEnvelope, faLock, faUser } from '@fortawesome/free-solid-svg-icons';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  faEnvelope = faEnvelope; faLock = faLock; faUser = faUser;
  model: any = {}
  constructor() { }

  ngOnInit(): void {
  }

  register() {
    console.log(this.model)
  }

  cancel() {
    console.log('cancelled');
  }

}
