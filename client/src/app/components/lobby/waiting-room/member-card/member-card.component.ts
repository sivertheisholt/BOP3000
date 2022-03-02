import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { faMinus, faPlus, faCheck } from '@fortawesome/free-solid-svg-icons';

@Component({
  selector: 'app-member-card',
  templateUrl: './member-card.component.html',
  styleUrls: ['./member-card.component.css']
})

export class MemberCardComponent implements OnInit {
  faMinus = faMinus; faPlus = faPlus;
  message: string = "Hello!"

  @Output() acceptMemberEvent = new EventEmitter<string>();
  
  constructor() { 
  }

  ngOnInit(): void {
  }
  acceptMember() {
    this.acceptMemberEvent.emit()
  } 
  
}
