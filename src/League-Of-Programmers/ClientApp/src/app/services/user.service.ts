import { Injectable } from '@angular/core';
import { ServicesBase, CommonService, Result } from './common';
import { HttpClient } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { catchError, debounceTime } from 'rxjs/operators';
import { Global } from '../global';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  constructor(
    private http: HttpClient,
    private base: ServicesBase,
    private common: CommonService
  ) { }

  /**
   * 当前用户是否为登录用户
   */
  isSelf(): Observable<Result> {
    const RESULT: Result = {
      status: 404,
      data: ''
    };
    if (!Global.loginInfo) {
      return of(RESULT);
    }
  }

}
