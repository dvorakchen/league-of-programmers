import { Injectable } from '@angular/core';
import { ServicesBase, CommonService, Result } from './common';
import { HttpClient, HttpResponse } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { catchError, debounceTime, retry } from 'rxjs/operators';
import { KeyValue } from '@angular/common';

export interface BlogItem {
  id: number;
  title: string;
  abstract: string;
  dateTime: string;
  views: number;
  state: KeyValue<number, string>;
}

@Injectable({
  providedIn: 'root'
})
export class BlogService {

  constructor(
    private base: ServicesBase,
    private common: CommonService,
    private http: HttpClient
  ) { }

  /**
   * 获取一个用户的博文
   * @param account 要获取博文的用户的账号
   */
  getBlogsByUser(account: string): Observable<Result> {
    account = account.trim();
    if (!account) {
      const R: Result = {
        status: 400,
        data: ''
      };
      return of(R);
    }
    return this.http.get<Result>(`/api/clients/blogs/${account}`)
    .pipe(
      retry(1),
      catchError(this.base.handleError)
    );
  }
}
