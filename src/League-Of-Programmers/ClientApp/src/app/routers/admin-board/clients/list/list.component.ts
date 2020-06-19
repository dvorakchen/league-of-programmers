import { Component, OnInit } from '@angular/core';
import { UserService, UserItem } from '../../../../services/user.service';
import { Paginator, CommonService } from '../../../../services/common';
import { PageEvent } from '@angular/material/paginator';

@Component({
  selector: 'app-list',
  templateUrl: './list.component.html',
  styleUrls: ['./list.component.sass']
})
export class ListComponent implements OnInit {

  index = 0;
  totalSize = 0;
  size = 10;

  account = '';
  dataSource: UserItem[] = [];
  displayedColumns = ['userName', 'account', 'email', 'blogCounts', 'createDate', 'actions'];

  constructor(
    private user: UserService,
    private common: CommonService
  ) { }

  ngOnInit(): void {
    this.getClientList();
  }

  private getClientList() {
    this.user.getClientsList(this.index + 1, this.size, this.account).subscribe(r => {
      if (r.status === 200) {
        const R = r.data as Paginator<UserItem>;
        this.dataSource = R.list;
        this.totalSize = R.totalSize;
      } else {
        this.common.snackOpen('获取失败');
      }
    });
  }

  searchList() {
    this.getClientList();
  }

  pageChange(pager: PageEvent) {
    this.index = pager.pageIndex;
    this.getClientList();
  }
}
