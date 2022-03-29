import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Game } from 'src/app/_models/game.model';

@Component({
  selector: 'app-gamecard',
  templateUrl: './gamecard.component.html',
  styleUrls: ['./gamecard.component.css']
})
export class GamecardComponent implements OnInit {
  activeGames: Game[] = [];
  @ViewChild('gameSearchInput') gameSearchInput?: ElementRef;

  constructor(private route: ActivatedRoute) { }

  ngOnInit(): void {
    this.activeGames = this.route.snapshot.data['games'];
    
  }

  onGameSearch(e: any){
    if(e.target.value.length <= 0){
      this.activeGames = this.route.snapshot.data['games'];
      return;
    }
    this.activeGames = this.activeGames.filter((item) => {
      return item.name.toLowerCase().includes(e.target.value.toLowerCase());
    });
  }
}
