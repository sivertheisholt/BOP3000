import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot } from '@angular/router';
import { LobbyService } from '../_services/lobby.service';

@Injectable({
  providedIn: 'root'
})
export class CreateLobbyGuard implements CanActivate {
  constructor(private lobbyService: LobbyService, private router: Router) {}

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Promise<boolean> {
    return new Promise((resolve) => {
        this.lobbyService.getQueueStatus().subscribe(
            (res) => {
                console.log(res);
                if(!res.inQueue && res.lobbyId == 0){
                    resolve(true);
                } else {
                    this.router.navigate(['lobby',res.lobbyId]);
                    resolve(false);
                }
            })
        })
    }
}
