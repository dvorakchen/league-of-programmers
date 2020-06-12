import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { NotificationDetail, NotificationsService } from '../../../../services/notifications.service';
import { CommonService } from '../../../../services/common';

@Component({
  selector: 'app-detail',
  templateUrl: './detail.component.html',
  styleUrls: ['./detail.component.sass']
})
export class DetailComponent implements OnInit {

  id = 0;
  isloading = false;
  detail: NotificationDetail = {
    title: '',
    content: ''
  };

  constructor(
      private notification: NotificationsService,
      private common: CommonService,
      private dialogRef: MatDialogRef<DetailComponent>,
      @Inject(MAT_DIALOG_DATA) public data: number
    ) { }

  ngOnInit(): void {
    this.id = this.data;
    this.notification.getNotificationDetail(this.id).subscribe(r => {
      if (r.status === 200) {
        this.detail = r.data as NotificationDetail;
      }
    });
  }

  close() {
    this.dialogRef.close();
  }

  delete() {
    this.isloading = true;
    this.notification.deleteNotification(this.id).subscribe(r => {
      if (r.status) {
        this.isloading = false;
        this.dialogRef.close(true);
      } else {
        this.common.snackOpen('删除失败', 3000);
      }
    });
  }
}
