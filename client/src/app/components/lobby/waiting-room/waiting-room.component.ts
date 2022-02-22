import { Component, OnInit } from '@angular/core';
import { faMinus, faPlus, faCheck } from '@fortawesome/free-solid-svg-icons';

@Component({
  selector: 'app-waiting-room',
  templateUrl: './waiting-room.component.html',
  styleUrls: ['./waiting-room.component.css']
})
export class WaitingRoomComponent implements OnInit {
  faMinus = faMinus; faPlus = faPlus;

  constructor() { }

  ngOnInit(): void {
  }

}
