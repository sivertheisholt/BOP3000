import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot } from '@angular/router';
import { LobbyService } from '../_services/lobby.service';
import { NotificationService } from '../_services/notification.service';

@Injectable({
  providedIn: 'root'
})
export class CreateLobbyGuard implements CanActivate {
  constructor(private lobbyService: LobbyService, private router: Router, private notificationService: NotificationService) {}

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Promise<boolean> {
    return new Promise((resolve) => {
        this.lobbyService.getQueueStatus().subscribe(
            (res) => {
                if(!res.inQueue && res.lobbyId == 0){
                    resolve(true);
                } else {
                    this.notificationService.setNewNotification({type: 'error', message: "You're already in a lobby, leave or end the one you're already in to create a new one."});
                    this.router.navigate(['lobby',res.lobbyId]);
                    resolve(false);
                }
            })
        })
    }
}
