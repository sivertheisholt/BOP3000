import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { AuthService } from "../_services/auth.service";

@Injectable({providedIn: 'root'})
export class AuthInterceptorService implements HttpInterceptor{
    constructor(private authService: AuthService){}
    intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        const token = this.authService.getUserId();
        if (token) {
            req = req.clone({ headers: req.headers.set('Authorization', 'Bearer ' + token) });
        }
        //req = req.clone({ headers: req.headers.set('Accept', '*/*') });
        req = req.clone({
            setHeaders: {
              "Permissions-Policy": "camera=*,geolocation=*,microphone=*,autoplay=*,fullscreen=*,picture-in-picture=*,sync-xhr=*,encrypted-media=*,oversized-images=*",
              "Strict-Transport-Security": "max-age=31536000; includeSubdomains",
              "X-Frame-Options": "SAMEORIGIN",
              "X-Content-Type-Options": "nosniff",
              "X-Xss-Protection": "1; mode=block",
              "Content-Security-Policy": "script-src https: 'unsafe-inline' 'unsafe-eval';style-src https: 'unsafe-inline' 'unsafe-eval';img-src https: data:;font-src https: data:;"
            }
          });
        console.log('Intercepted HTTP call: ' + req);
        return next.handle(req);
    }
}