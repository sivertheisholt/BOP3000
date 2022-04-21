import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot } from '@angular/router'; 
import { NotificationService } from '../_services/notification.service';
import { UserService } from '../_services/user.service';
  
  @Injectable({
    providedIn: 'root'
  })
export class BlockedGuard implements CanActivate {
constructor(private userService: UserService, private router: Router, private notificationService: NotificationService) {}

canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Promise<boolean> {
    return new Promise((resolve) => {
            this.userService.checkIfBlocked(route.params.id).subscribe(
                (res) =>{
                    if(res){
                        this.notificationService.setNewNotification({message: "You are not allowed to view this user's profile", type: 'error'})
                        this.router.navigate(['home']);
                        resolve(false);
                    } else {
                        resolve(true);
                    }
                }
            )
        })
    }
}
  