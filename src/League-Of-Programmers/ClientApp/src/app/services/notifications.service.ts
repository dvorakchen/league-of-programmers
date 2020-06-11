import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ServicesBase, Result, CLIENT_SIDE } from './common';
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
}
