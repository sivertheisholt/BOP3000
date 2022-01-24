import { Injectable } from "@angular/core";
import { Subject } from "rxjs";

@Injectable({providedIn: 'root'})
export class NavigationService {
    visible! : boolean
    sidebarVisibleChange: Subject<boolean> = new Subject<boolean>();

    constructor(){
        this.sidebarVisibleChange.subscribe(
            (value: boolean) => {  
                this.visible = value;
            }
        );
    }

    toggleNav(){
        this.sidebarVisibleChange.next(!this.visible);
    }
}