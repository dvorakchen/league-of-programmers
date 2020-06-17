import { Injectable } from '@angular/core';
import { ServicesBase, CommonService, Result, CreatedResult } from './common';
import { HttpClient } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';

/**
 * 头像文件最大字节数
 */
export const AVATAR_MAX_SIZE = 64;

@Injectable({
  providedIn: 'root'
})
export class FileService {

  constructor(
    private base: ServicesBase,
    private common: CommonService,
    private http: HttpClient
  ) { }

  uploadAvatar(file: any): Observable<Result | CreatedResult> {
    if (file === undefined || file === null) {
      const R = {
        status: 400,
        data: '必须上传头像'
      };
      return of(R);
    }
    const fileForm = new FormData()
    .append('file', file);
    return this.http.post<Result | CreatedResult>(`/api/files`, fileForm)
    .pipe(
      catchError(this.base.handleError)
    );
  }
}
