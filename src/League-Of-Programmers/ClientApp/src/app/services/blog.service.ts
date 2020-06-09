import { Injectable } from '@angular/core';
import { ServicesBase, Result } from './common';
import { HttpClient } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { catchError, retry } from 'rxjs/operators';
import { KeyValue } from '@angular/common';

export interface BlogItem {
  id: number;
  title: string;
  abstract: string;
  dateTime: string;
  views: number;
  state: KeyValue<number, string>;
}

export interface BlogDetail {
  title: string;
  targets: string[];
  content: string;
  views: number;
  dateTime: string;
  author: string;
  authorAccount: string;
}

export interface NewBlog {
  title: string;
  targets: string[];
  content: string;
}

export interface ModifyBlog {
  title: string;
  targets: string[];
  content: string;
}

@Injectable({
  providedIn: 'root'
})
export class BlogService {

  constructor(
    private base: ServicesBase,
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

  /**
   * 获取博文详情
   */
  getBlogDetail(id: number): Observable<Result> {
    return this.http.get<Result>(`/api/clients/blogs/${id}`)
    .pipe(
      retry(1),
      catchError(this.base.handleError)
    );
  }

  /**
   * 写博文
   */
  writeBlog(newPost: NewBlog): Observable<Result> {
    return this.http.post<Result>(`/api/clients/blogs`, newPost)
    .pipe(
      catchError(this.base.handleError)
    );
  }

  /**
   * 修改博文
   */
  modifyBlog(id: number, model: ModifyBlog): Observable<Result> {
    return this.http.put<Result>(`/api/clients/blogs/${id}`, model)
    .pipe(
      catchError(this.base.handleError)
    );
  }

  /**
   * 删除一个博文
   * @param id blog id
   */
  deleteBlog(id: number): Observable<Result> {
    return this.http.delete<Result>(`/api/clients/blogs/${id}`)
    .pipe(
      catchError(this.base.handleError)
    );
  }
}
