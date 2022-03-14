import { HttpBackend, HttpClient, HttpContext } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { map } from "rxjs/operators";
import { Country } from "../_models/country.model";
import { Member } from "../_models/member.model";
import { environment } from '../../environments/environment';

@Injectable({providedIn: 'root'})
export class UserSettingsService{
    baseUrl = environment.apiUrl;
    private http: HttpClient

    constructor(private handler: HttpBackend){
        this.http = new HttpClient(handler);
    }


    getAllLanguages(){
        return this.http.get<Country[]>(this.baseUrl + 'countries');
    }
}