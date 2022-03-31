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

  fixDate(lobbyStartDate: Date){
    let date = new Date(lobbyStartDate);
    let fixedDate = ('0' + date.getDate()).slice(-2) + '.' + ('0' + date.getMonth()).slice(-2) + '.' + ('0' + date.getFullYear()).slice(-2);
    return fixedDate;
  }
}
