import { Injectable } from '@angular/core';
import { ServicesBase, Result, CLIENT_SIDE, ADMINISTRATOR_SIDE } from './common';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { catchError, retry } from 'rxjs/operators';
import { Global } from '../global';

import sha256 from 'crypto-js/sha256';
import { enc } from 'crypto-js';

export interface Profile {
  avatar: string;
  userName: string;
  email: string;
}

export interface UserInfo {
  name: string;
  email: string;
}

export interface ChangePassword {
  oldPassword: string;
  newPassword: string;
  confirmPassword: string;
}

export interface UserItem {
  id: number;
  userName: string;
  account: string;
  email: string;
  blogCounts: number;
  createDate: string;
}

@Injectable({
  providedIn: 'root'
})
export class UserService {

  readonly routePrefix = `${CLIENT_SIDE}clients/`;

  constructor(
    private http: HttpClient,
    private base: ServicesBase
  ) { }

  /**
   * 当前用户是否为登录用户
   * return:
   *      200:    has user
   *          body: bool // is self
   *      404:    has not client
   */
  isSelf(account: string = ''): Observable<Result> {
    const RESULT: Result = {
      status: 404,
      data: ''
    };
    if (!Global.loginInfo || !Global.loginInfo.account) {
      return of(RESULT);
    }
    if (account.trim() === '') {
      account = Global.loginInfo.account;
    }
    return this.http.get<Result>(`${this.routePrefix}${account}/check`)
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
   * 获取客户列表
   */
  getClientsList(index: number, size: number, s: string): Observable<Result> {
    let p: HttpParams = new HttpParams();
    p = p.set('index', index.toString());
    p = p.set('size', size.toString());
    if (s && s.trim()) {
      p = p.set('s', s.trim());
    }

    return this.http.get<Result>(`${ADMINISTRATOR_SIDE}clients?${p.toString()}`)
    .pipe(
      retry(1),
      catchError(this.base.handleError)
    );
  }

  /**
   * 获取一个客户详情
   */
  getClientDetail(id: number) {
    return this.http.get<Result>(`${ADMINISTRATOR_SIDE}clients/${id}`)
    .pipe(
      retry(1),
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

  /**
   * 当前登录人修改头像
   * @param avatarId 新头像ID
   */
  modifyAvator(avatarId: number): Observable<Result> {
    return this.http.patch<Result>(`${this.routePrefix}avatar`, avatarId)
    .pipe(
      catchError(this.base.handleError)
    );
  }

  changePassword(changePassword: ChangePassword): Observable<Result> {
    const post: ChangePassword = {
      ...changePassword
    };

    post.oldPassword = sha256(post.oldPassword).toString();
    post.newPassword = sha256(post.newPassword).toString();
    post.confirmPassword = sha256(post.confirmPassword).toString();

    const words = enc.Utf8.parse(JSON.stringify(post));
    const base64 = enc.Base64.stringify(words);

    const headers: HttpHeaders = new HttpHeaders().set('Content-Type', 'application/json');

    //  发送 base64 加密的值
    return this.http.patch<Result>(`${this.routePrefix}password`, `\"${base64}\"`, { headers })
    .pipe(
      catchError(this.base.handleError)
    );
  }
}
