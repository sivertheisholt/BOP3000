import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { map } from "rxjs/operators";
import { Member } from "../_models/member.model";

@Injectable({providedIn: 'root'})
export class UserService{
    baseUrl = 'https://localhost:5001/api/';
    
    constructor(private http: HttpClient){}

    getUserData(){
        return this.http.get(this.baseUrl + 'members/current').pipe(
            map(response => {
                console.log(response);
            })
        );
    }

    updateUserData(memberData: Member){
        console.log(memberData);
/*         return this.http.post(this.baseUrl + 'members/', {
            body: 'test'
        }).pipe(
            map(response => {
                console.log(response);
            })
        ) */
    }
    
}