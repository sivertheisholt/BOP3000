import { Injectable } from "@angular/core";
import { ActivatedRouteSnapshot, Resolve, RouterStateSnapshot } from "@angular/router";
import { TranslateService } from "@ngx-translate/core";
import { map } from "rxjs/operators";
import { UserService } from "../_services/user.service";

@Injectable({providedIn: 'root'})
export class ActivityLogResolver implements Resolve<any>{
    constructor(private userService: UserService, private translate: TranslateService){}
    resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
        return this.userService.getUserActivities().pipe(
            map(response => {
                response.forEach(element => {
                    switch (element.identifier) {
                        case 'lobby-created':
                          element.outputText = this.translate.instant('ACTIVITY_LOBBY_CREATED');
                          break;
                        case 'lobby-finished':
                          element.outputText = this.translate.instant('ACTIVITY_LOBBY_FINISHED');
                          break;
                        case 'lobby-joined':
                          element.outputText = this.translate.instant('ACTIVITY_LOBBY_JOINED');
                          break;
                        case 'member-followed':
                          element.outputText = this.translate.instant('ACTIVITY_FOLLOWING');
                          break;
                        default:
                          break;
                      }
                })
                return response;
            })
        );
    }
}