import { Injectable } from '@angular/core';
import { ServicesBase, CommonService, Result, CLIENT_SIDE } from './common';
import { HttpClient } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { Global } from '../global';

export interface Profile {
  avatar: string;
  userName: string;
  email: string;
}

export interface UserInfo {
  name: string;
  email: string;
}

@Injectable({
  providedIn: 'root'
})
export class UserService {

  readonly routePrefix = `${CLIENT_SIDE}clients/`;

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

  /**
   * 修改用户信息，
   * 用户名和邮箱
   *
   * return:
   *      200:    successfully
   *      400:    defeated
   */
  modifyUserInfo(model: UserInfo): Observable<Result> {
    const R: Result = {
      status: 400,
      data: ''
    };

    if (!model.name) {
      R.data = '昵称不能为空';
      return of(R);
    }
    if (!model.email) {
      R.data = '邮箱不能为空';
      return of(R);
    }
    //  验证格式
    const REG = new RegExp('^\\s*([A-Za-z0-9_-]+(\\.\\w+)*@(\\w+\\.)+\\w{2,9})\\s*$');
    if (!REG.test(model.email)) {
      R.data = '邮箱格式不正确';
      return of(R);
    }

    return this.http.patch<Result>(`${this.routePrefix}`, model)
    .pipe(
      catchError(this.base.handleError)
    );
  }
}
