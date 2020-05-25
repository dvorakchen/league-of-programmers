import { Injectable } from '@angular/core';
import { ServicesBase, CommonService, Result } from './common';
import { HttpClient, HttpResponse } from '@angular/common/http';
import { Observable } from 'rxjs';
import { catchError, debounceTime, retry } from 'rxjs/operators';

import sha256 from 'crypto-js/sha256';

export interface registerModel {
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

  register(model: registerModel): Observable<Result> {
    if (!model.account) {
      this.common.snackOpen('账号不能为空');
      return;
    }
    if (model.account.length < 2) {
      this.common.snackOpen(`账号长度不能小于2位`);
      return;
    }
    if (!model.password) {
      this.common.snackOpen('密码不能为空');
      return;
    }
    if (model.password.length < 6) {
      this.common.snackOpen(`密码长度不能小于6位`);
      return;
    }
    if (model.password != model.confirmPassword) {
      this.common.snackOpen('两次密码不一致');
      return;
    }
    return this.http.post<Result>('/api/clients/register', model)
      .pipe(
        retry(1),
        catchError(this.base.handleError)
      );
  }
}
