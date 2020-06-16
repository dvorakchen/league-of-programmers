import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { BlogDetail, BlogService } from '../../../../services/blog.service';
import { CommonService } from '../../../../services/common';

@Component({
  selector: 'app-detail',
  templateUrl: './detail.component.html',
  styleUrls: ['./detail.component.sass']
})
export class DetailComponent implements OnInit {

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

  ngOnInit(): void {
    this.route.paramMap.subscribe(p => {
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
}
