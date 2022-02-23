import { Injectable } from "@angular/core";
import { ActivatedRouteSnapshot, Resolve, RouterStateSnapshot } from "@angular/router";
import { map } from "rxjs/operators";
import { UserSettingsService } from "../_services/user-settings.service";

@Injectable({providedIn: 'root'})
export class CountryResolver implements Resolve<any>{
    constructor(private userSettingsService: UserSettingsService){}
    resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
        return this.userSettingsService.getAllLanguages().pipe(
            map(response => {
                console.log(response);
                return response;
            })
        )
    }
}