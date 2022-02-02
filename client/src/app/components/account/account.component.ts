import { Component, OnInit } from '@angular/core';

import { faThumbsDown, faThumbsUp } from '@fortawesome/free-regular-svg-icons';

@Component({
  selector: 'app-account',
  templateUrl: './account.component.html',
  styleUrls: ['./account.component.css']
})
export class AccountComponent implements OnInit {
  faThumbsDown = faThumbsDown; faThumbsUp = faThumbsUp;

  constructor() { }

  ngOnInit(): void {
  }

}
