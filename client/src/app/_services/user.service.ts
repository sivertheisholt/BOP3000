import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { map } from "rxjs/operators";
import { Member } from "../_models/member.model";
import { UserProfile } from "../_models/user-profile.model";

@Injectable({providedIn: 'root'})
export class UserService{
    baseUrl = 'https://localhost:5001/api/';
    
    constructor(private http: HttpClient){}

    getUserData(){
        return this.http.get<UserProfile>(this.baseUrl + 'members/current').pipe(
            map(response => {
                console.log(response);
                return response;
            })
        )
    }

    deleteAccount(){
        return this.http.delete(this.baseUrl + 'accounts/delete').pipe(
            map(response =>{
                console.log(response);
            })
        );
    }

    updateMember(model: any){
        return this.http.patch(this.baseUrl + 'members', model).pipe(
            map(response => {
                console.log(response);
            })
        ).subscribe((response) => {
            console.log(response);
        });
    }
    
}