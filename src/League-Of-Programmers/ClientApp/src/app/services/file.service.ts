import { Injectable } from '@angular/core';
import { ServicesBase, CommonService, Result, CreatedResult } from './common';
import { HttpClient } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';

/**
 * 头像文件最大字节数 Byte
 */
// tslint:disable-next-line: no-bitwise
export const AVATAR_MAX_SIZE = 64 << 10;

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
    const R = {
      status: 400,
      data: ''
    };
    if (file === null || file === undefined) {
      R.data = '必须上传头像';
      return of(R);
  }
    if (file.size > AVATAR_MAX_SIZE) {
        R.data = `头像大小必须小于${AVATAR_MAX_SIZE}字节`;
        return of(R);
    }

    const fileForm = new FormData();
    fileForm.set('file', file);

    return this.http.post<Result | CreatedResult>(`/api/files`, fileForm)
    .pipe(
      catchError(this.base.handleError)
    );
  }
}
