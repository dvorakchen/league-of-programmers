import { Component, OnInit, Inject, ViewChild, ElementRef } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { CommonService, CreatedResult } from '../../services/common';
import { FileService, AVATAR_MAX_SIZE } from '../../services/file.service';
import { UserService } from '../../services/user.service';

@Component({
    selector: 'app-avatar-dialog',
    template: `
<div class="box">
    <img class="w100 h100" *ngIf="data.avatarPath" [src]="data.avatarPath | safeUrl" alt="头像" >
    <div class="actions">
        <div class="self-action">
            <button mat-stroked-button color="primary" *ngIf="data.isSelf" (click)="changeAvatar()">修改头像</button>
            <button mat-raised-button color="primary" [disabled]="!canModify" (click)="modifiAvator()">确认修改</button>
            <input type="file" #fil_avatar hidden (change)="fileChange()">
        </div>
        <div class="close">
            <button mat-button color="primary" (click)="close()">关闭</button>
        </div>
    </div>
</div>
    `,
    styleUrls: ['./avatar-dialog.component.sass']
})
export class AvatarDialogComponent implements OnInit {

    canModify = false;
    /**
     * 待上传的头像
     */
    waitToUploadAvatar: any;
    newAvatarPath = '';

    constructor(
        private common: CommonService,
        private dialogRef: MatDialogRef<AvatarDialogComponent>,
        private file: FileService,
        private user: UserService,
        @Inject(MAT_DIALOG_DATA) public data: any
    ) {}

    @ViewChild('fil_avatar', {static: false}) fileNewAvatar: ElementRef;

    ngOnInit(): void {
        if (this.data.isSelf === null || this.data.avatarPath === null) {
            this.common.snackOpen('参数有误');
        }
    }

    changeAvatar() {
        this.fileNewAvatar.nativeElement.click();
    }

    fileChange() {
        if (this.fileNewAvatar.nativeElement.files.length > 0) {
            this.waitToUploadAvatar = this.fileNewAvatar.nativeElement.files[0];
            this.data.avatarPath = URL.createObjectURL(this.waitToUploadAvatar);
            this.canModify = true;
        }
    }

    modifiAvator() {
        if (this.waitToUploadAvatar === null || this.waitToUploadAvatar === undefined) {
            this.common.snackOpen('必须上传头像');
            return;
        }
        if (this.waitToUploadAvatar.size > AVATAR_MAX_SIZE) {
            this.common.snackOpen(`头像大小必须小于${AVATAR_MAX_SIZE}字节`);
            return;
        }
        this.canModify = false;
        this.file.uploadAvatar(this.waitToUploadAvatar).subscribe(r => {
            if (r.status === 201) {
                //  得到 ID
                const FILE_RESULT = r as CreatedResult;
                this.user.modifyAvator(+FILE_RESULT.data).subscribe(avatarResp => {
                    if (avatarResp.status === 200) {
                        this.common.snackOpen('修改成功');
                        this.canModify = false;
                        this.newAvatarPath = avatarResp.data;
                        return;
                    } else {
                        this.common.snackOpen(avatarResp.data);
                    }
                    this.canModify = true;
                });
            } else {
                this.common.snackOpen(r.data);
            }
        });
    }

    close() {
        this.dialogRef.close(this.newAvatarPath);
    }
}
