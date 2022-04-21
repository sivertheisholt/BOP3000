import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot } from '@angular/router';
import { NotificationService } from '../_services/notification.service';
import { UserService } from '../_services/user.service';

@Injectable({
  providedIn: 'root'
})
export class DiscordConnectionGuard implements CanActivate {
  constructor(private userService: UserService, private router: Router, private notificationService: NotificationService) {}

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Promise<boolean> {
    return new Promise((resolve) => {
        this.userService.getUserData().subscribe(
            (user) => {
                this.userService.getDiscordConnectionStatus(user.id!).subscribe(
                    (discordResponse) => {
                        if(discordResponse.connected){
                            resolve(true);
                        } else {
                            this.router.navigate(['settings'], {queryParams: {viewMode: 'tab3'}});
                            this.notificationService.setNewNotification({type: 'warning', message: 'You need to connect your Discord in order to join lobbies.'});
                            resolve(false);
                        }
                    }
                )
            })
        })
    }
}
