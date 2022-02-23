import { Injectable } from "@angular/core";
import { FormGroup } from "@angular/forms";

@Injectable({
    providedIn: 'root'
})
export class ValidationService{

    regexUsername = '/[^A-Za-z0-9]+/';

    constructor(){}
}