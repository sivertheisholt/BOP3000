import { Injectable } from "@angular/core";
import { ActivatedRouteSnapshot, Resolve, RouterStateSnapshot } from "@angular/router";
import { map } from "rxjs/operators";
import { ActivityLog } from 'src/app/_models/activity-log.model';
import { UserService } from "../_services/user.service";

@Injectable({providedIn: 'root'})
export class ActivityLogResolver implements Resolve<any>{
    constructor(private userService: UserService){}
    resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
        return this.userService.getUserActivities().pipe(
            map(response => {
                response.forEach(element => {
                    switch (element.identifier) {
                        case 'lobby-created':
                          console.log('lobby created');
                          element.outputText = 'just created a lobby.';
                          break;
                        case 'lobby-finished':
                          console.log('lobby finished');
                          element.outputText = 'just finished a lobby.';
                          break;
                        case 'lobby-joined':
                          console.log('lobby joined');
                          element.outputText = 'joined a lobby!';
                          break;
                        case 'member-followed':
                          console.log('member followed');
                          element.outputText = 'started following you!';
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