import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Location } from '@angular/common';
import { MatDialogRef } from '@angular/material/dialog';
import { CommonService, REDIRECT } from '../../services/common';
import { UserService, ChangePassword } from '../../services/user.service';

@Component({
    selector: 'app-change-password',
    templateUrl: './change-password.component.html',
    styleUrls: ['./change-password.component.sass']
})
export class ChangePasswordComponent implements OnInit {

    changePassword: ChangePassword = {
        oldPassword: '',
        newPassword: '',
        confirmPassword: ''
    };

    constructor(
        private common: CommonService,
        private dialogRef: MatDialogRef<ChangePasswordComponent>,
        private user: UserService,
        private router: Router,
        private loc: Location
    ) {}

    ngOnInit(): void {
    }

    confirmChangePassword() {
        if (this.changePassword.newPassword !== this.changePassword.confirmPassword) {
            this.common.snackOpen('两次新密码必须相同');
            return;
        }
        this.user.changePassword(this.changePassword).subscribe(r => {
            if (r.status === 204) {
                this.common.snackOpen('修改成功，请重新登录');
                
                this.router.navigateByUrl(`/login?${REDIRECT}=${this.loc.path()}`);
                this.dialogRef.close();
                return;
            } else {
                this.common.snackOpen(r.data);
            }
        });
    }
}
