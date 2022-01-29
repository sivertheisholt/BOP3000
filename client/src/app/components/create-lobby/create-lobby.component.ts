import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-create-lobby',
  templateUrl: './create-lobby.component.html',
  styleUrls: ['./create-lobby.component.css']
})
export class CreateLobbyComponent implements OnInit {
  createLobbyForm!: FormGroup;
  submitted = false;
  games = [
    {id: 1, name: 'Counter-Strike: Global Offensive'},
    {id: 2, name: 'League of Legends'},
    {id: 3, name: 'Sea of Thieves'},
    {id: 4, name: 'Dota 2'},
    {id: 5, name: 'DayZ'},
    {id: 6, name: 'Minecraft'},
    {id: 7, name: 'Grand Theft Auto V'},
    {id: 8, name: 'Project Zomboid'},
    {id: 9, name: 'PUBG'},
    {id: 10, name: 'Escape From Tarkov'},
    {id: 11, name: 'Valorant'},
    {id: 12, name: 'Fortnite'},
    {id: 13, name: 'Red Dead Redemption 2'},
    {id: 14, name: 'World of Warcraft'},
    {id: 15, name: 'Rainbow Six'},
    {id: 16, name: 'Ready or Not'},
    {id: 17, name: 'Terraria'},
    {id: 18, name: 'Apex Legends'},
    {id: 19, name: 'Other'}];


    selectedType = ['Fun', 'Competitive', 'Ranked', 'Tryhard', 'Chill'];

    selectedMaxplayers = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 ,16, 17, 18, 19, 20];

    model: any = {};

  constructor() { }

  ngOnInit(): void {
    this.createLobbyForm = new FormGroup({
      game: new FormControl(null, [Validators.required]),
      lobbyName: new FormControl(null, [Validators.required]),
      desc: new FormControl(null),
      type: new FormControl(null, [Validators.required]),
      maxPlayers: new FormControl(null, [Validators.required])
    });

  }

  onSubmit(){
    this.submitted = true;
    console.log(this.createLobbyForm);
  }

}
