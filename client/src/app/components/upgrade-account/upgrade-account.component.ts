import { Component, OnInit } from '@angular/core';
import { faCheck, faTimesCircle } from '@fortawesome/free-solid-svg-icons';

@Component({
  selector: 'app-upgrade-account',
  templateUrl: './upgrade-account.component.html',
  styleUrls: ['./upgrade-account.component.css']
})
export class UpgradeAccountComponent implements OnInit {
  faCheck = faCheck; faTimesCircle = faTimesCircle;
  constructor() { }

  ngOnInit(): void {
  }

}
