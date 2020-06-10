import { Injectable, isDevMode, Optional, Inject } from '@angular/core';
import { Location, DOCUMENT, ɵparseCookieValue as parseCookieValue } from '@angular/common';
import {
  HttpEvent, HttpInterceptor, HttpHandler, HttpRequest, HttpErrorResponse, HttpResponse
} from '@angular/common/http';
import { Observable, throwError, of } from 'rxjs';
import { catchError, mergeMap } from 'rxjs/operators';
import { Router } from '@angular/router';
import { environment } from '../../environments/environment';
import { CommonService, Result } from '../services/common';

@Injectable()
export class DefaultInterceptor implements HttpInterceptor {

  constructor(
    private router: Router,
    private loc: Location,
    private common: CommonService,
    @Inject(DOCUMENT) private doc: any
  ) { }

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    const cookieString = this.doc.cookie || '';
    const jwt = parseCookieValue(cookieString, '_j_w_t_');

    let newUrl = environment.SERVER_HOST;
    if (!req.url.startsWith('/')) {
      newUrl += '/';
    }
    newUrl += req.url;
    if (isDevMode()) {
      console.warn(`request: ${newUrl}`);
    }

    const SERVER_REQ = req.clone({
      url: newUrl,
      setHeaders: {
        Authorization: `Bearer ${jwt}`
      }
    });

    return next.handle(SERVER_REQ)
      .pipe(
        mergeMap((resp) => {
          if (resp instanceof HttpResponse) {
            let data = resp.body;
            switch (resp.status) {
              case 201:
                data = resp.headers.get('Location');
                break;
              default:
                break;
            }
            if (data === null || data === undefined) {
              data = {};
            }
            const DATA = resp.clone<Result>({
              body: {
                status: resp.status,
                data
              }
            });
            return of(DATA);
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
          //  case 400: { } break;
          case 401: {
            this.common.snackOpen('请先登录', 3000);
            this.router.navigate(['/login', { redirect: this.loc.path(true) }]);
          }         break;
          case 404: {
            this.router.navigate(['/pages', '404']);
          }         break;
          case 429: {
            //  熔断页面
            this.router.navigate(['/pages', '429']);
          }         break;
          default: break;
        }
        return throwError(err);
      }));
  }
}
