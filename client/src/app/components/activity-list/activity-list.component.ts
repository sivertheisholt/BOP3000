import { Component, OnInit } from '@angular/core';
import { RoomItem } from 'src/app/_models/room-item.model';

@Component({
  selector: 'app-activity-list',
  templateUrl: './activity-list.component.html',
  styleUrls: ['./activity-list.component.css']
})
export class ActivityListComponent implements OnInit {
  gameRooms: RoomItem[] = [
    new RoomItem(1, 'Dr4g0nsl4y3r', 'Counter-Strike Global Offensive', 5),
    new RoomItem(2, 'pu55ysl4y3r69', 'League of Legends', 5),
    new RoomItem(3, 'somebody', 'Counter-Strike Global Offensive', 3),
  ]

  constructor() { }

  ngOnInit(): void {
  }

}
