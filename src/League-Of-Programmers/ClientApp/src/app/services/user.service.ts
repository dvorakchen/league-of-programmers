import { Injectable } from '@angular/core';
import { ServicesBase, CommonService, Result } from './common';
import { HttpClient } from '@angular/common/http';
import { Observable, of, pipe } from 'rxjs';
import { catchError, debounceTime, retry } from 'rxjs/operators';
import { Global } from '../global';

export interface Profile {
  avatar: string;
  account: string;
  email: string;
}

@Injectable({
  providedIn: 'root'
})
export class UserService {

  readonly routePrefix = '/api/clients/clients/';

  constructor(
    private http: HttpClient,
    private base: ServicesBase,
    private common: CommonService
  ) { }

  /**
   * 当前用户是否为登录用户
   * return:
   *      200:    has user
   *          body: bool // is self
   *      404:    has not client
   */
  isSelf(): Observable<Result> {
    const RESULT: Result = {
      status: 404,
      data: ''
    };
    if (!Global.loginInfo || !Global.loginInfo.account) {
      return of(RESULT);
    }
    return this.http.get<Result>(`${this.routePrefix}${Global.loginInfo.account}/check`)
      .pipe(
        catchError(this.base.handleError)
      );
  }

  /**
   * 获取用户主页信息
   * @param account 账号
   *
   * return:
   *      200:    successfully
   *      404:    client not exist
   */
  getProfile(account: string): Observable<Result> {
    return this.http.get<Result>(`${this.routePrefix}${account}/home`)
      .pipe(
        catchError(this.base.handleError)
      );
  }
}
