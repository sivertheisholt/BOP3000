import { Injectable } from "@angular/core";
import { ActivatedRouteSnapshot, Resolve, RouterStateSnapshot } from "@angular/router";
import { map } from "rxjs/operators";
import { UserService } from "../_services/user.service";

@Injectable({providedIn: 'root'})
export class UserResolver implements Resolve<any>{
    constructor(private userService: UserService){}
    resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
        return this.userService.getUserData().pipe(
            map(response => {
                return response;
            })
        )
    }
}