import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { map } from "rxjs/operators";
import { Country } from "../_models/country.model";
import { environment } from '../../environments/environment';
import { CustomImg } from "../_models/custom-img.model";

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

    getCustomizationImages(){
        return this.http.get<CustomImg[]>(this.baseUrl + 'images/customizer_images').pipe(
            map((res) => {
                return res;
            })
        );
    }

    postChangeAccountBackground(url: string){
        return this.http.post<string>(this.baseUrl + 'members/set-background', {url: url});
    }
}