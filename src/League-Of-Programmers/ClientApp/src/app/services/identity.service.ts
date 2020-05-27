import { Injectable } from '@angular/core';
import { ServicesBase, CommonService, Result } from './common';
import { HttpClient, HttpResponse } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { catchError, debounceTime, retry } from 'rxjs/operators';

import sha256 from 'crypto-js/sha256';

export interface RegisterModel {
  account: string;
  password: string;
  confirmPassword: string;
}

@Injectable({
  providedIn: 'root'
})
export class IdentityService {

  constructor(
    private base: ServicesBase,
    private common: CommonService,
    private http: HttpClient
  ) { }

  login(account: string, password: string): Observable<Result> {
    password = sha256(password).toString();

    const URL = '/api/clients/login';
    return this.http.patch<Result>(URL, {
      account,
      password
    }).pipe(
      debounceTime(500),
      catchError(this.base.handleError)
    );
  }

  register(model: RegisterModel): Observable<Result> {
    const RESULT: Result = {
      status: 400,
      data: ''
    };

    if (!model.account) {
      this.common.snackOpen('账号不能为空');
      RESULT.data = '账号不能为空';
      return of(RESULT);
    }
    if (model.account.length < 2) {
      this.common.snackOpen(`账号长度不能小于2位`);
      RESULT.data = '账号长度不能小于2位';
      return of(RESULT);
    }
    if (!model.password) {
      this.common.snackOpen('密码不能为空');
      RESULT.data = '密码不能为空';
      return of(RESULT);
    }
    if (model.password.length < 6) {
      this.common.snackOpen(`密码长度不能小于6位`);
      RESULT.data = '密码长度不能小于6位';
      return of(RESULT);
    }
    if (model.password !== model.confirmPassword) {
      this.common.snackOpen('两次密码不一致');
      RESULT.data = '两次密码不一致';
      return of(RESULT);
    }

    model.password = sha256(model.password).toString();
    model.confirmPassword = sha256(model.confirmPassword).toString();

    return this.http.post<Result>('/api/clients/register', model)
      .pipe(
        debounceTime(1000),
        catchError(this.base.handleError)
      );
  }
}
