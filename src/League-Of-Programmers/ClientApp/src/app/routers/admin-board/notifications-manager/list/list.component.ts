import { Component, OnInit } from '@angular/core';
import { PageEvent } from '@angular/material/paginator';
import { NotificationsService, NotificationItem } from '../../../../services/notifications.service';
import { Paginator } from '../../../../services/common';

@Component({
  selector: 'app-list',
  templateUrl: './list.component.html',
  styleUrls: ['./list.component.sass']
})
export class ListComponent implements OnInit {

  index = 0;
  size = 10;
  totalSize = 0;
  search = '';
  dataSource: NotificationItem[] = [];
  displayedColumns: string[] = ['title', 'dateTime', 'isTop', 'actions'];

  constructor(
    private notification: NotificationsService
  ) { }

  ngOnInit(): void {
    this.getNotificationsList();
  }

  private getNotificationsList() {
    this.notification.getNotificationList(this.index + 1, this.size, this.search.trim()).subscribe(r => {
      if (r.status === 200) {
        const R = r.data as Paginator;
        this.totalSize = R.totalSize;
        this.dataSource = R.list;
      }
    });
  }

  searchList() {
    this.search = this.search.trim();
    this.getNotificationsList();
  }

  pageChange(pager: PageEvent) {
    this.index = pager.pageIndex;
    this.getNotificationsList();
  }
}
