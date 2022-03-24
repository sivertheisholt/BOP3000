import { Component, Input, OnInit } from '@angular/core';
import { Member } from 'src/app/_models/member.model';

@Component({
  selector: 'app-account-info',
  templateUrl: './account-info.component.html',
  styleUrls: ['./account-info.component.css']
})
export class AccountInfoComponent implements OnInit {
  @Input('user') user? : Member;

  constructor() { }

  ngOnInit(): void {
  }

}
