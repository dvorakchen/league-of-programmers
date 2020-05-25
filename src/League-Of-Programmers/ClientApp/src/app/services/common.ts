import { Injectable } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { HttpErrorResponse } from '@angular/common/http';
import { of, Observable } from 'rxjs';
import { Router } from '@angular/router';

export interface Result<T = any> {
  status: number;
  data: T;
}

/**
 * 请求失败, 可以和 Result 的 data 对比
 */
export const FAULT: undefined = undefined;

/**
 * 分页模型
 */
export interface Paginator<T = any> {
  index: number;
  size: number;
  totalPages: number;
  totalRows: number;
  list: T[];
}

export const REDIRECT = 'redirect';

//  基本服务
@Injectable({
  providedIn: 'root'
})
export class ServicesBase {

  constructor(
    private router: Router
  ) { }

  handleError(error: HttpErrorResponse): Observable<Result> {
    const R: Result = {
      status: 200,
      data: ''
    };
    switch (error.status) {
      case 401: {
        //  跳到登录页
        this.router.navigate(['/identity', { REDIRECT: location.href }]);
        return of(R);
      }
      default: break;
    }
    console.error(`backend returned code ${error.status}`);
    console.error(`error: ${error.error}`);
    return of(R);
  }
}

//  通用服务
@Injectable({
  providedIn: 'root'
})
export class CommonService {

  constructor(
    private snack: MatSnackBar
  ) { }

  snackOpen(message: string, duration?: number) {
    if (!duration) {
      duration = null;
    }

    this.snack.open(message, '关闭', {
      duration,
    });
  }
}
