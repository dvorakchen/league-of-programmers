import { Component, OnInit, OnDestroy } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { PageEvent } from '@angular/material/paginator';
import { UserService, UserDetail } from '../../../../services/user.service';
import { BlogService, BlogItem } from '../../../../services/blog.service';
import { CommonService } from '../../../../services/common';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-detail',
  templateUrl: './detail.component.html',
  styleUrls: ['./detail.component.sass']
})
export class DetailComponent implements OnInit, OnDestroy {

  id = 0;
  index = 0;
  size = 10;
  totalSize = 0;

  userDetail: UserDetail = {
    userName: '',
    account: '',
    avatar: '',
    email: '',
    createDate: ''
  };
  blogs: BlogItem[] = [];

  constructor(
    private route: ActivatedRoute,
    private user: UserService,
    private blog: BlogService,
    private common: CommonService
  ) { }

  sub: Subscription;
  ngOnDestroy(): void {
    this.sub.unsubscribe();
  }

  ngOnInit(): void {
    this.sub = this.route.paramMap.subscribe(p => {
      this.id = +p.get('id');
      this.getUserDetail();
      this.getBlogs();
  });
  }

  private getUserDetail() {
    this.user.getClientDetail(this.id).subscribe(r => {
      if (r.status === 200) {
        this.userDetail = r.data;
      } else {
        this.common.snackOpen(r.data);
      }
    });
  }

  private getBlogs() {

  }

  pageChange(pager: PageEvent) {
    this.index = pager.pageIndex;
    this.getBlogs();
  }
}
