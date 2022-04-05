import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { map } from "rxjs/operators";
import { Member } from "../_models/member.model";
import { environment } from '../../environments/environment';
import { UserSearch } from "../_models/user-search.model";
import { ActivityLog } from "../_models/activity-log.model";
import { ProfileImage } from "../_models/profile-image.model";

@Injectable({providedIn: 'root'})
export class UserService{
    baseUrl = environment.apiUrl;
    
    constructor(private http: HttpClient){}

    getUserData(){
        return this.http.get<Member>(this.baseUrl + 'members/current');
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

    getSpecificUser(id: number){
        return this.http.get<Member>(this.baseUrl + 'members/' + id).pipe(
            map(response => {
                let user: Member;
                user = response;
                return user;
            })
        );
    }

    followUser(id: number){
        return this.http.patch(this.baseUrl + 'members/follow?memberId=' + id, '').pipe(
            map(response => {
                return response;
            })
        );
    }

    unfollowUser(id: number){
        return this.http.patch(this.baseUrl + 'members/unfollow?memberId=' + id, '').pipe(
            map(response => {
                return response;
            })
        );
    }

    checkFollowing(id: number){
        return this.http.get<boolean>(this.baseUrl + 'members/check-follow?memberId=' + id).pipe(
            map(response => {
                return response;
            })
        );
    }

    searchUser(input: string){
        return this.http.get<UserSearch>(this.baseUrl + 'members/search?name=' + input);
    }

    getUserActivities(){
        return this.http.get<ActivityLog[]>(this.baseUrl + 'members/current/activity');
    }

    postProfileImage(formData: FormData){
        let headers = new HttpHeaders();
        headers.append('Content-Type', 'multipart/form-data');
        headers.append('Accept', 'application/json');
        let options = {
            headers: headers
        }
        return this.http.post<ProfileImage>(this.baseUrl + 'members/set-photo', formData, options);
    }

}