import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ServicesBase, Result, CLIENT_SIDE, ADMINISTRATOR_SIDE } from './common';
import { HttpParams } from '@angular/common/http';
import { catchError, retry, debounceTime } from 'rxjs/operators';
import { Observable } from 'rxjs';

export interface NotificationItem {
  id: number;
  title: string;
  dateTime: string;
  isTop: boolean;
}

export interface NotificationDetail {
  title: string;
  content: string;
}

export interface NewNotification {
  title: string;
  content: string;
  isTop: boolean;
}

@Injectable({
  providedIn: 'root'
})
export class NotificationsService {

  constructor(
    private base: ServicesBase,
    private http: HttpClient
  ) { }

  /**
   * 获取通知列表
   */
  getNotificationList(index: number, size: number, s: string): Observable<Result> {
    let p = new HttpParams();
    if (s) {
      p = p.append('s', s.trim());
    }
    return this.http.get<Result>(`${CLIENT_SIDE}notifications?${p.toString()}`)
    .pipe(
      retry(1),
      debounceTime(500),
      catchError(this.base.handleError)
    );
  }

  /**
   * 获取通知详情
   */
  getNotificationDetail(id: number): Observable<Result> {
    return this.http.get<Result>(`${CLIENT_SIDE}notifications/${id}`)
    .pipe(
      retry(1),
      debounceTime(500),
      catchError(this.base.handleError)
    );
  }

  /**
   * 删除一个通知
   */
  deleteNotification(id: number): Observable<Result> {
    return this.http.delete<Result>(`${ADMINISTRATOR_SIDE}notifications/${id}`)
    .pipe(
      catchError(this.base.handleError)
    );
  }

  /**
   * 发布新通知
   */
  postNotification(model: NewNotification) {
    return this.http.post<Result>(`${ADMINISTRATOR_SIDE}notifications`, model)
    .pipe(
      debounceTime(1000),
      catchError(this.base.handleError)
    );
  }
}
