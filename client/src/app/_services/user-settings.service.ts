import { HttpBackend, HttpClient, HttpContext } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { map } from "rxjs/operators";
import { Country } from "../_models/country.model";
import { UserProfile } from "../_models/user-profile.model";

@Injectable({providedIn: 'root'})
export class UserSettingsService{
    baseUrl = 'https://localhost:5001/api/';
    private http: HttpClient

    constructor(private handler: HttpBackend){
        this.http = new HttpClient(handler);
    }


    getAllLanguages(){
        return this.http.get<Country[]>(this.baseUrl + 'countries');
    }
}