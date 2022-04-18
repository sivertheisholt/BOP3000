import { HttpBackend, HttpClient, HttpContext } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { map } from "rxjs/operators";
import { Country } from "../_models/country.model";
import { Member } from "../_models/member.model";
import { environment } from '../../environments/environment';

@Injectable({providedIn: 'root'})
export class UserSettingsService{
    baseUrl = environment.apiUrl;

    constructor(private http: HttpClient){
    }


    getAllLanguages(){
        return this.http.get<Country[]>(this.baseUrl + 'countries');
    }

    hideSteam(status: boolean){
        return this.http.patch<boolean>(this.baseUrl + 'members/steam/hide', {hide: status});
    }

    hideDiscord(status: boolean){
        return this.http.patch<boolean>(this.baseUrl + 'members/discord/hide', {hide: status});
    }
}