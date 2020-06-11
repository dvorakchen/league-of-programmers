import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { NoticeContentComponent } from '../../../shared/notice-board/notice-content.component';
import { Paginator } from '../../../services/common';
import { NotificationsService, NotificationItem } from '../../../services/notifications.service';
import { PageEvent, MatPaginatorIntl } from '@angular/material/paginator';

@Component({
  selector: 'app-list',
  templateUrl: './list.component.html',
  styleUrls: ['./list.component.sass']
})
export class ListComponent implements OnInit {

  index = 0;
  size = 20;
  totalSize = 0;
  search = '';

  notificationList: NotificationItem[] = [];

  constructor(
    private pagerInit: MatPaginatorIntl,
    private dialog: MatDialog,
    private notification: NotificationsService
  ) { }

  ngOnInit(): void {
    this.getNotificationList();

    this.pagerInit.nextPageLabel = '下一页';
    this.pagerInit.previousPageLabel = '上一页';
    this.pagerInit.itemsPerPageLabel = '';
    this.pagerInit.getRangeLabel = (index: number, size: number, totalSize: number) => {
      return `${index} - ${size} 共 ${totalSize}`;
    };
  }

  private getNotificationList() {
    this.notification.getNotificationList(this.index + 1, this.size, this.search).subscribe(resp => {
      if (resp.status === 200) {
        const R = resp.data as Paginator;
        this.totalSize = R.totalSize;
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

  pageChange(pager: PageEvent) {
    this.index = pager.pageIndex;
    this.getNotificationList();
  }
}
