import { Injectable } from "@angular/core";
import { BehaviorSubject, Observable } from "rxjs";

@Injectable({
    providedIn: 'root'
})
export class SpinnerService{
    private count = 0;
    private spinner$ = new BehaviorSubject<boolean>(false);

    constructor(){}

    getSpinnerObserver(): Observable<boolean>{
        return this.spinner$.asObservable();
    }

    requestStarted(){
        console.log(this.count);
        if(++this.count === 1){
            this.spinner$.next(true);
        }
    }

    requestEnded(){
        console.log(this.count);
        if(this.count === 0 || --this.count === 0){
            this.spinner$.next(false);
        }
    }

    resetSpinner(){
        this.count = 0;
        this.spinner$.next(false);
    }
}