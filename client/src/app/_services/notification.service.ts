import { Injectable } from "@angular/core";
import { Observable, Subject } from "rxjs";

@Injectable({
    providedIn: 'root'
})
export class NotificationService{
    private notification$ = new Subject<string>();

    get notificationObservable(): Observable<string>{
        return this.notification$.asObservable();
    }

    setNewNotification(noti: string){
        this.notification$.next(noti);
    }
}