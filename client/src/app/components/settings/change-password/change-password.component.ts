import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { GamesService } from 'src/app/_services/games.service';

@Component({
  selector: 'app-change-password',
  templateUrl: './change-password.component.html',
  styleUrls: ['./change-password.component.css']
})
export class ChangePasswordComponent implements OnInit {
  changePasswordForm! : FormGroup;

  constructor(private gamesService: GamesService) { }

  ngOnInit(): void {
    this.changePasswordForm = new FormGroup({
      oldPassword: new FormControl(null, Validators.required),
      newPassword: new FormControl(null, Validators.required),
      repeatNewPassword: new FormControl(null, Validators.required)
    });
  }

  onSubmit(){
    this.gamesService.searchGame('count').subscribe(
      (res) => {
        console.log(res);
      }
    );
    console.log(this.changePasswordForm);
  }

}
