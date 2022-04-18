import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot } from '@angular/router'; 
import { UserService } from '../_services/user.service';
  
  @Injectable({
    providedIn: 'root'
  })
export class BlockedGuard implements CanActivate {
constructor(private userService: UserService, private router: Router) {}

canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Promise<boolean> {
    return new Promise((resolve) => {
            this.userService.checkIfBlocked(route.params.id).subscribe(
                (res) =>{
                    console.log(res);
                    if(res){
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
  