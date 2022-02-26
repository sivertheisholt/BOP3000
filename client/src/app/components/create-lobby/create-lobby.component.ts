import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { fromEvent } from 'rxjs';
import { debounceTime, distinctUntilChanged, filter, map } from 'rxjs/operators';
import { GamesService } from 'src/app/_services/games.service';
import { LobbyService } from 'src/app/_services/lobby.service';

@Component({
  selector: 'app-create-lobby',
  templateUrl: './create-lobby.component.html',
  styleUrls: ['./create-lobby.component.css']
})
export class CreateLobbyComponent implements OnInit {
  createLobbyForm!: FormGroup;
  submitted = false;
  games: any;
  selectedType = ['Fun', 'Competitive', 'Ranked', 'Tryhard', 'Chill'];
  selectedMaxplayers = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 ,16, 17, 18, 19, 20];


  model: any = {};

  @ViewChild('gameInput', {static: true}) gameInput? : ElementRef;

  isSearching: boolean;

  constructor(private lobbyService: LobbyService, private gamesService: GamesService, private router: Router) {
    this.isSearching = false;

  }

  ngOnInit(): void {
    this.createLobbyForm = new FormGroup({
      gameId: new FormControl(null),
      title: new FormControl(null, [Validators.required]),
      lobbyDescription: new FormControl(null),
      gameType: new FormControl(null, [Validators.required]),
      maxUsers: new FormControl(null, [Validators.required]),
      lobbyRequirement: new FormGroup({
        gender: new FormControl(null)
      })
    });

    fromEvent(this.gameInput?.nativeElement, 'keyup').pipe(
      map((event: any) => {
        return event.target.value;
      })
      ,filter(response => response.length > 2)
      , debounceTime(1000)
      ,distinctUntilChanged()
    ).subscribe((input: string) => {
      this.isSearching = true;
      this.gamesService.searchGame(input).subscribe((response) => {
        this.isSearching = false;
        this.games = response;
      }, (err) => {
        this.isSearching = false;
        console.log(err);
      })
    })
  }

  onSubmit(){
    this.submitted = true;
    this.lobbyService.postLobby(this.createLobbyForm.value).subscribe(
      (res) => {
        //Redirect bruker til det nye rommet
        console.log(res);
        this.router.navigate(['lobby', res.id]);
      }, err => {
        console.log(err);
      }
    )
  }

  selectGame(game: any){
    this.gameInput!.nativeElement.value = game.name;
    this.games = [];
    this.createLobbyForm.controls.gameId.setValue(game.appId);
  }

}
