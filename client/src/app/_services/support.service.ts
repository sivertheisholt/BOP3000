import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { environment } from '../../environments/environment';
import { SupportTicket } from "../_models/support-ticket.model";


@Injectable({providedIn: 'root'})
export class SupportService{
    baseUrl = environment.apiUrl;

    constructor(private http: HttpClient){}
    
    postTicket(ticket: SupportTicket){
        return this.http.post<SupportTicket>(this.baseUrl + 'support/create-ticket', ticket);
    }
}