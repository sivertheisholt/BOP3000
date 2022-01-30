import { Component, OnInit } from '@angular/core';
import { RoomCard } from './room-card.model';

@Component({
  selector: 'app-roomcard',
  templateUrl: './roomcard.component.html',
  styleUrls: ['./roomcard.component.css']
})
export class RoomcardComponent implements OnInit {

  roomCard: RoomCard[] = [
    new RoomCard(
      'Very cool Lobby',
      'This is a very cool lobby for cool people',
      'https://m.media-amazon.com/images/M/MV5BMjEwMTMwNjg0M15BMl5BanBnXkFtZTgwODc1ODM3MDE@._V1_.jpg',
      'https://i.pinimg.com/originals/a0/80/7e/a0807ee91a153edac9c80104f70dcd17.jpg',
      5,
      25,
      'Casual',
      'Govert',
      'World of Warcraft' 
    )
  ];

  constructor() { }

  ngOnInit(): void {
  }

}
