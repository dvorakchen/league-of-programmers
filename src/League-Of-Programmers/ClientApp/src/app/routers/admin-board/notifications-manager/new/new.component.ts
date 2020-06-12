import { Component, OnInit } from '@angular/core';
import { NotificationsService, NewNotification } from '../../../../services/notifications.service';
import { CommonService } from '../../../../services/common';
import { MatDialogRef } from '@angular/material/dialog';

@Component({
  selector: 'app-new',
  templateUrl: './new.component.html',
  styleUrls: ['./new.component.sass']
})
export class NewComponent implements OnInit {

  isLoading = false;
  newNotification: NewNotification = {
    title: '',
    content: '',
    isTop: false
  };

  constructor(
    private notification: NotificationsService,
    private dialogRef: MatDialogRef<NewComponent>,
    private common: CommonService
  ) { }

  ngOnInit(): void {
  }

  post() {
    this.isLoading = true;

    this.notification.postNotification(this.newNotification).subscribe(r => {
      if (r.status === 200) {
        this.common.snackOpen('发布成功', 3000);
        this.dialogRef.close();
      } else {
        this.common.snackOpen('发布失败', 3000);
        this.isLoading = false;
      }
    });
  }

  close() {
    this.dialogRef.close();
  }
}
