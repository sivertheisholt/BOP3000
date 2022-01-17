import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { NavigationExtras, Router } from '@angular/router';
import { catchError } from 'rxjs/operators';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {

  constructor(private router: Router) {}

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    return next.handle(request).pipe(
      catchError(err => {
        if(err) {
          switch (err.status) {
            case 400:
              //Validation error
              if(err.error.errors) {
                const modalStateErrors = [];
                for(const key in err.error.errors) {
                  if(err.error.errors[key]) {
                    modalStateErrors.push(err.error.errors[key]);
                  }
                }
                throw modalStateErrors.flat();
              } else {
              //Normal error
              //Show some kind of error to the frontend here
              }
              break;
            case 401:
              //Show some kind of error to the frontend here
              break;
            case 404:
              this.router.navigateByUrl('/not-found');
              break;
            case 500:
              const navigationExtras: NavigationExtras = {state: {error: err.error}};
              this.router.navigateByUrl('/server-error', navigationExtras);
              break;
            default:
              console.log("Something unexpected went wrong");
              break;
          }
        }
        return throwError(err);
      })
    )
  }
}
