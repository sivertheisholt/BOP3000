import { Component, OnInit } from '@angular/core';
import { RoomCard } from '../../../../_models/room-card.model';

@Component({
  selector: 'app-roomcard',
  templateUrl: './roomcard.component.html',
  styleUrls: ['./roomcard.component.css']
})
export class RoomcardComponent implements OnInit {

  constructor() { }
  roomCard: RoomCard[] = [
    new RoomCard(
      'MMO disc',
      'Lorem ipsum dolor sit, amet consectetur adipisicing elit. Eum tenetur quo rerum placeat deleniti soluta asperiores facilis quam non eius magni vitae laboriosam deserunt, ea, iste, voluptates beatae sunt. Quia repudiandae, provident quaerat ipsum natus consequatur blanditiis molestias enim ab.',
      'https://m.media-amazon.com/images/M/MV5BMjEwMTMwNjg0M15BMl5BanBnXkFtZTgwODc1ODM3MDE@._V1_.jpg',
      'https://scontent.fsvg1-1.fna.fbcdn.net/v/t1.6435-9/156623795_10221644964931544_373919933969752127_n.jpg?_nc_cat=101&ccb=1-5&_nc_sid=09cbfe&_nc_ohc=8kSt3JoiJTIAX9hv0zB&_nc_ht=scontent.fsvg1-1.fna&oh=00_AT9rzEAum0YSeHo2ow3jU0IuDFdPhTqSKRdGtAkDXBztsA&oe=621C8488',
      5,
      25,
      'Casual',
      'Govert',
      'World of Warcraft'
    ),
    new RoomCard(
      'Athena',
      'Lorem ipsum something something',
      'https://compass-ssl.xboxlive.com/assets/02/fd/02fde636-f6f3-405f-b70e-50dd23de5f6f.jpg?n=mobile.jpg',
      'https://i.pinimg.com/originals/a0/80/7e/a0807ee91a153edac9c80104f70dcd17.jpg',
      9,
      15,
      'Adventure',
      'Sigve',
      'Sea of Thieves'
    ),
    new RoomCard(
      'LESSGO',
      'shoot to kill',
      'https://static.posters.cz/image/750/plakater/counter-strike-team-i48831.jpg',
      'https://cdn.cloudflare.steamstatic.com/steamcommunity/public/images/avatars/75/7514b0304efd66200bc6aeb671642d925b95ca15_full.jpg',
      2,
      5,
      'Ranked',
      'dybe',
      'Counter Strike Go'
    ),
    new RoomCard(
      'Que pasa',
      'i need a hero',
      'https://i.redd.it/zlnfyb3tm8u51.jpg',
      'https://cdn.discordapp.com/avatars/266598683246723072/fe0ef9e8041bb34171a98611fad512c2.webp?size=128',
      6,
      8,
      'Chill',
      'Sivert',
      'Sea of Thieves'
    ),
    new RoomCard(
      'Athena',
      'Lorem ipsum something something',
      'https://compass-ssl.xboxlive.com/assets/02/fd/02fde636-f6f3-405f-b70e-50dd23de5f6f.jpg?n=mobile.jpg',
      'https://cdn.discordapp.com/avatars/266598683246723072/fe0ef9e8041bb34171a98611fad512c2.webp?size=128',
      9,
      15,
      'Adventure',
      'Sigve',
      'Sea of Thieves'
    ),
    new RoomCard(
      'Athena',
      'Lorem ipsum something something',
      'https://compass-ssl.xboxlive.com/assets/02/fd/02fde636-f6f3-405f-b70e-50dd23de5f6f.jpg?n=mobile.jpg',
      'https://cdn.discordapp.com/avatars/266598683246723072/fe0ef9e8041bb34171a98611fad512c2.webp?size=128',
      9,
      15,
      'Adventure',
      'Sigve',
      'Sea of Thieves'
    )
  ];

  ngOnInit(): void {
  }

}
