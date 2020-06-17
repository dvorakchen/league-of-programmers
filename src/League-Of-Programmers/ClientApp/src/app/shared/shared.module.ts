import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';

import { MaterialModule } from './material.module';

import { NoticeBoardComponent } from './notice-board/notice-board.component';
import { UserInfoBoxComponent } from './user-info-box/user-info-box.component';
import { AvatarDialogComponent } from './user-info-box/avatar-dialog.component';
import { SafeUrlPipe } from './pipe/safe-url.pipe';

import { SloganComponent } from './slogan/slogan.component';
import { NoticeContentComponent } from './notice-board/notice-content.component';
import { DeleteConfirmDialogComponent } from './delete-confirm-dialog/delete-confirm-dialog.component';

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
  AvatarDialogComponent
];

const EXPORTS_PROVIDERS = [
  SafeUrlPipe
];

@NgModule({
  declarations: [...EXPORTS_COMPONENT, ...EXPORTS_PROVIDERS],
  imports: [...EXPORTS_MODULE],
  exports: [...EXPORTS_MODULE, ...EXPORTS_COMPONENT],
  providers: [...EXPORTS_PROVIDERS]
})
export class SharedModule { }
