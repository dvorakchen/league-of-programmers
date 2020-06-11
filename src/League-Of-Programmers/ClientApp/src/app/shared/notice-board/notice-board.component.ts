import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { NoticeContentComponent } from './notice-content.component';
import { NotificationItem, NotificationsService } from '../../services/notifications.service';
import { Paginator } from '../../services/common';

@Component({
  selector: 'app-notice-board',
  templateUrl: './notice-board.component.html',
  styleUrls: ['./notice-board.component.sass']
})
export class NoticeBoardComponent implements OnInit {

  notificationList: NotificationItem[] = [];

  constructor(
    private dialog: MatDialog,
    private notification: NotificationsService
    ) {}

  ngOnInit() {
    this.notification.getNotificationList(1, 6, '').subscribe(resp => {
      if (resp.status === 200) {
        const R = resp.data as Paginator<NotificationItem>;
        this.notificationList = R.list;
      }
    });
  }

  openNotice(id: number) {
    this.dialog.open(NoticeContentComponent, {
      minWidth: '40%',
      minHeight: '70%',
      data: id
    });
  }
}
