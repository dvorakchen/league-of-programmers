import { Injectable, isDevMode } from '@angular/core';
import { Location } from '@angular/common';
import {
  HttpEvent, HttpInterceptor, HttpHandler, HttpRequest, HttpErrorResponse, HttpResponse
} from '@angular/common/http';
import { Observable, throwError, of } from 'rxjs';
import { catchError, mergeMap } from 'rxjs/operators';
import { Router } from '@angular/router';
import { environment } from '../../environments/environment';

@Injectable()
export class DefaultInterceptor implements HttpInterceptor {

  constructor(
    private router: Router,
    private loc: Location
  ) { }

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    let newUrl = environment.SERVER_HOST;
    if (!req.url.startsWith('/')) {
      newUrl += '/';
    }
    newUrl += req.url;
    if (isDevMode()) {
      console.warn(`request: ${newUrl}`);
    }

    const SERVER_REQ = req.clone({url: newUrl});

    return next.handle(SERVER_REQ)
      .pipe(
        mergeMap((resp) => {
          if (resp instanceof HttpResponse) {
            if (resp.body !== null) {
              resp.body.status = resp.status;
              resp.body.data = resp.body;
            }
            // } else {
            //   resp.body.data = resp.body;
            // }
          }
          return of(resp);
      }),
      catchError((err: HttpErrorResponse) => {
        if (isDevMode()) {
          console.error(`DEV: backend returned code ${err.status}`);
          console.error(`DEV: message: ${err.message}`);
          console.error(`DEV: error: ${err.error}`);
        }
        switch (err.status) {
          case 401: {
            this.router.navigate(['/login', { redirect: this.loc.path(true) }]);
          } break;
          case 429: {
            //  熔断页面
          } break;
          default: {
            return throwError(err);
          }
        }
      }));
  }
}
