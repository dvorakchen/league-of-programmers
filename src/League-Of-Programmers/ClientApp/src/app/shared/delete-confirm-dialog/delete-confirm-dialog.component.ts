import { Component, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
/*
 *  这是一个对话框，接受的 data 参数只接受一个字符串，用于提醒。
 *  对话框关闭后返回一个 布尔 值，说明用户是点了取消（false）还是确定删除（true）
 */
@Component({
    selector: 'app-delete-confirm-dialog',
    template: `
    <h1 mat-dialog-title>确认删除？</h1>
    <div mat-dialog-content>
      <p style="max-width:450px;">删除 {{data}} 后不可恢复！</p>
    </div>
    <div mat-dialog-actions>
      <button mat-button (click)="close(false)">取消</button>
      <button mat-button mat-flat-button color="warn" (click)="close(true)">确定删除</button>
    </div>
  `
})
// tslint:disable-next-line: component-class-suffix
export class DeleteConfirmDialogComponent {

    constructor(
      public dialogRef: MatDialogRef<DeleteConfirmDialogComponent>,
      @Inject(MAT_DIALOG_DATA) public data: string) {}

    close(isDelete: boolean): void {
      this.dialogRef.close(isDelete);
    }
}
