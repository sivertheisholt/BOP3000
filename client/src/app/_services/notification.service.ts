import { Injectable } from "@angular/core";
import { Observable, Subject } from "rxjs";
import { NotificationModel } from "../_models/notification.model";

@Injectable({
    providedIn: 'root'
})
export class NotificationService{
    private notification$ = new Subject<NotificationModel>();

    get notificationObservable(): Observable<NotificationModel>{
        return this.notification$.asObservable();
    }

    setNewNotification(noti: NotificationModel){
        this.notification$.next(noti);
    }
}