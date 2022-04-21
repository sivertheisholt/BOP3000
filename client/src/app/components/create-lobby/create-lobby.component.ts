import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { fromEvent } from 'rxjs';
import { debounceTime, distinctUntilChanged, filter, map } from 'rxjs/operators';
import { GameSearch } from 'src/app/_models/game-search.model';
import { GamesService } from 'src/app/_services/games.service';
import { LobbyService } from 'src/app/_services/lobby.service';
import { CustomValidator } from 'src/app/_validators/custom-validator';

@Component({
  selector: 'app-create-lobby',
  templateUrl: './create-lobby.component.html',
  styleUrls: ['./create-lobby.component.css']
})
export class CreateLobbyComponent implements OnInit {
  createLobbyForm!: FormGroup;
  submitted = false;
  games: GameSearch[] = [];
  selectedType = ['Fun', 'Competitive', 'Ranked', 'Tryhard', 'Chill'];


  model: any = {};

  @ViewChild('gameInput', {static: true}) gameInput? : ElementRef;

  constructor(private lobbyService: LobbyService, private gamesService: GamesService, private router: Router) {

  }

  ngOnInit(): void {
    this.createLobbyForm = new FormGroup({
      gameId: new FormControl(null, [Validators.required]),
      title: new FormControl(null, [Validators.required]),
      lobbyDescription: new FormControl(null),
      gameType: new FormControl(null, [Validators.required]),
      maxUsers: new FormControl(null, [Validators.required, CustomValidator.patternValidator(/^[0-9]*$/, { onlyNumber: true })]),
      lobbyRequirement: new FormGroup({
        gender: new FormControl(null)
      })
    });

    fromEvent(this.gameInput?.nativeElement, 'keyup').pipe(
      map((event: any) => {
        console.log(event.target.value.length);
        if(event.target.value.length > 1){
          this.games = [];
          this.gameInput?.nativeElement.classList.add('loading');
          return event.target.value;
        } else {
          this.gameInput?.nativeElement.classList.remove('loading');
        }
      })
      ,filter(response => response != undefined)
      , debounceTime(1000)
      ,distinctUntilChanged()
    ).subscribe((input: string) => {
      this.gamesService.searchGame(input).subscribe((response) => {
        if(response.length == 0){
          this.gameInput?.nativeElement.classList.remove('loading');
          this.createLobbyForm.get('gameId')?.setErrors({'noResult': 'Game not found.'});
          return;
        }
        this.gameInput?.nativeElement.classList.remove('loading');
        this.games = response;
        return;
      }, (err) => {
        this.gameInput?.nativeElement.classList.remove('loading');
      })
    })
  }

  onSubmit(){
    if(this.createLobbyForm.valid){
      this.lobbyService.postLobby(this.createLobbyForm.value).subscribe(
        (res) => {
          this.router.navigate(['lobby', res.id]);
        }, err => {
          this.createLobbyForm.setErrors({'serverError': 'Something went wrong.'});
        }
      )
    } else {
      this.createLobbyForm.setErrors({'missingFields': 'Have you filled every field thats required?'});
      console.log(this.createLobbyForm);
    }

  }

  selectGame(game: any){
    this.gameInput!.nativeElement.value = game.name;
    this.games = [];
    this.createLobbyForm.controls.gameId.setValue(game.appId);
  }

}
