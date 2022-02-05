import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { AccountService } from "../_services/account.service";

@Injectable({providedIn: 'root'})
export class AuthInterceptorService implements HttpInterceptor{
    constructor(private accService: AccountService){}
    intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        const token = this.accService.getUserId();
        if(token){
            req = req.clone({
                headers: req.headers
                .append('Authorization', `Bearer ${token}`)
                .append('Content-Type','application/json')
            });
        }
        console.log('Intercepted HTTP call: ' + req);
        return next.handle(req);
    }
}