import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormsModule, NG_VALIDATORS } from '@angular/forms';
import { RouterModule } from '@angular/router';

import { MaterialModule } from './material.module';

import { NoticeBoardComponent } from './notice-board/notice-board.component';
import { UserInfoBoxComponent } from './user-info-box/user-info-box.component';
import { ChangePasswordComponent } from './user-info-box/change-password.component';
import { AvatarDialogComponent } from './avatar-dialog/avatar-dialog.component';
import { SafeUrlPipe } from './pipe/safe-url.pipe';

import { SloganComponent } from './slogan/slogan.component';
import { NoticeContentComponent } from './notice-board/notice-content.component';
import { DeleteConfirmDialogComponent } from './delete-confirm-dialog/delete-confirm-dialog.component';

//  import { PasswordMustSameValidatorDirective } from './validators';

const EXPORTS_MODULE = [
  CommonModule,
  ReactiveFormsModule,
  FormsModule,
  RouterModule,
  MaterialModule
];

const EXPORTS_COMPONENT = [
  NoticeBoardComponent,
  UserInfoBoxComponent,
  SloganComponent,
  NoticeContentComponent,
  DeleteConfirmDialogComponent,
  AvatarDialogComponent,
  ChangePasswordComponent
];

const EXPORTS_PIPE = [
  SafeUrlPipe
];

const EXPORTS_DIRECTIVE = [
  // PasswordMustSameValidatorDirective
];

@NgModule({
  declarations: [...EXPORTS_COMPONENT, ...EXPORTS_PIPE, ...EXPORTS_DIRECTIVE],
  imports: [...EXPORTS_MODULE],
  exports: [...EXPORTS_MODULE, ...EXPORTS_COMPONENT, ...EXPORTS_DIRECTIVE],
  providers: [...EXPORTS_PIPE]
})
export class SharedModule { }
