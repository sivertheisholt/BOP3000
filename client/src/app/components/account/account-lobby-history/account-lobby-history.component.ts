import { Component, Input, OnInit } from '@angular/core';
@Component({
  selector: 'app-account-lobby-history',
  templateUrl: './account-lobby-history.component.html',
  styleUrls: ['./account-lobby-history.component.css']
})
export class AccountLobbyHistoryComponent implements OnInit {
  @Input('finishedLobbies') finishedLobbies? : any[];

  constructor(){ 

  }

  ngOnInit(): void {

  }


}
