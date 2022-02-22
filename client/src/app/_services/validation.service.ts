import { Injectable } from "@angular/core";

@Injectable({
    providedIn: 'root'
})
export class ValidationService{

    regexUsername = '/[^A-Za-z0-9]+/';

    constructor(){}
    
    compareInput(input: string, confirmInput: string){

    }
}