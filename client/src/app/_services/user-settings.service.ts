import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Country } from "../_models/country.model";

@Injectable({providedIn: 'root'})
export class UserSettingsService{
    baseUrl = 'https://localhost:5001/api/';

    constructor(private http: HttpClient){}

    getAllLanguages(){
        return this.http.get<Country[]>(this.baseUrl + 'countries');
    }
}