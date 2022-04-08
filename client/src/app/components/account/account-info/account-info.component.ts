import { AfterViewInit, Component, Input, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { DiscordConnection } from 'src/app/_models/discord-connection.model';
import { Member } from 'src/app/_models/member.model';
import { SteamConnection } from 'src/app/_models/steam-connection.model';
import { UserService } from 'src/app/_services/user.service';

@Component({
  selector: 'app-account-info',
  templateUrl: './account-info.component.html',
  styleUrls: ['./account-info.component.css']
})
export class AccountInfoComponent implements OnInit {
  @Input('user') user? : Member;
  steamStatus?: SteamConnection;
  discordStatus?: DiscordConnection;

  constructor(private userService: UserService, private route: ActivatedRoute) { }

  ngOnInit(): void {
    this.userService.getDiscordConnectionStatus(this.route.snapshot.params.id).subscribe(
      (res) => {
        this.discordStatus = res;
      }
    );

    this.userService.getSteamConnectionStatus(this.route.snapshot.params.id).subscribe(
      (res) => {
        this.steamStatus = res;
      }
    );
  }
  

}
