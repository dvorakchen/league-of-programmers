import { Component, OnInit, OnDestroy } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { BlogDetail, BlogService, BlogState } from '../../../../services/blog.service';
import { CommonService } from '../../../../services/common';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-detail',
  templateUrl: './detail.component.html',
  styleUrls: ['./detail.component.sass']
})
export class DetailComponent implements OnInit, OnDestroy {

  id: number;
  detail: BlogDetail = {
    title: '',
    targets: [],
    content: '',
    views: 0,
    likes: 0,
    dateTime: '',
    author: '',
    authorAccount: '',
    state: { key: -1, value: '' }
  };

  constructor(
    private route: ActivatedRoute,
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
      this.getBlogDetail();
    });
  }

  private getBlogDetail() {
    this.blog.getBlogDetail(this.id).subscribe(r => {
      if (r.status === 200) {
        this.detail = r.data as BlogDetail;
      } else {
        this.common.snackOpen(r.data);
      }
    });
  }

  enable() {
    this.blog.enable(this.id).subscribe(r => {
      if (r.status === 204) {
        this.detail.state.key = BlogState.Enabled;
        this.detail.state.value = '启用';
      } else {
        this.common.snackOpen(r.data);
      }
    });
  }

  disable() {
    this.blog.disable(this.id).subscribe(r => {
      if (r.status === 204) {
        this.detail.state.key = BlogState.Disabled;
        this.detail.state.value = '禁用';
      } else {
        this.common.snackOpen(r.data);
      }
    });
  }
}
