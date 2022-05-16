import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor,
  HttpResponse,
  HttpErrorResponse
} from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { NavigationExtras, Router } from '@angular/router';
import { tap } from 'rxjs/operators';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {

  constructor(private router: Router) {}

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    return next.handle(request).pipe(
      tap(
        (event) => {
          if(event instanceof HttpResponse){
            
          }
        },
        (error: HttpErrorResponse) => {
          switch(error.status){
            case 400:
              //Validation error
              if(error.error.errors) {
                const modalStateErrors = [];
                for(const key in error.error.errors) {
                  if(error.error.errors[key]) {
                    modalStateErrors.push(error.error.errors[key]);
                  }
                }
                this.router.navigate(['home'])
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
              const navigationExtras: NavigationExtras = {state: {error: error.error}};
              this.router.navigateByUrl('/server-error', navigationExtras);
              break;
            default:
              console.log("Something unexpected went wrong");
              break;
          }
          throw error;
        } 
      )
    )
  }
}
