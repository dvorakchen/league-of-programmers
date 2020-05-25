import { Injectable } from '@angular/core';
import {
  HttpEvent, HttpInterceptor, HttpHandler, HttpRequest, HttpErrorResponse, HttpResponse
} from '@angular/common/http';
import { Observable, throwError, of } from 'rxjs';
import { catchError, mergeMap } from 'rxjs/operators';
import { FAULT, CommonService } from '../services/common';
import { Router } from '@angular/router';

@Injectable()
export class DefaultInterceptor implements HttpInterceptor {

  constructor(
    private common: CommonService,
    private router: Router
  ) { }

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    return next.handle(req)
      .pipe(
        catchError((err: HttpErrorResponse) => {
          switch (err.status) {
            case 401: {
              this.router.navigateByUrl('/identity');
            } break;
            default: {
              this.common.snackOpen(err.message);
              return throwError(err);
            }
          }
        })
      );
  }
}
