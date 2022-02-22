import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Game } from 'src/app/_models/game.model';

@Component({
  selector: 'app-gamecard',
  templateUrl: './gamecard.component.html',
  styleUrls: ['./gamecard.component.css']
})
export class GamecardComponent implements OnInit {
  activeGames: Game[] = [];

  constructor(private route: ActivatedRoute) { }

  ngOnInit(): void {
    this.activeGames = this.route.snapshot.data['games'];
  }

}
