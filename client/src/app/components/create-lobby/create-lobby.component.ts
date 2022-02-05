import { Component, OnInit } from '@angular/core';
import { FormArray, FormControl, FormGroup, Validators } from '@angular/forms';
import { AccountService } from 'src/app/_services/account.service';
import { LobbyService } from 'src/app/_services/lobby.service';

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

  constructor(private accService: AccountService, private lobbyService: LobbyService) { }

  ngOnInit(): void {
    this.createLobbyForm = new FormGroup({
      gameId: new FormControl(null, [Validators.required]),
      title: new FormControl(null, [Validators.required]),
      lobbyDescription: new FormControl(null),
      gameType: new FormControl(null, [Validators.required]),
      maxUsers: new FormControl(null, [Validators.required]),
      lobbyRequirement: new FormGroup({
        gender: new FormControl(null)
      })
    });

  }

  onSubmit(){
    this.submitted = true;
    this.lobbyService.postLobby(this.createLobbyForm.value).subscribe(
      (res) => {
        //Redirect bruker til det nye rommet
        console.log(res);
      }, err => {
        console.log(err);
      }
    )
  }

}
