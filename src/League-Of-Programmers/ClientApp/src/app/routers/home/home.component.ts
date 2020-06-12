import { Component, OnInit } from '@angular/core';
import { BlogItem, BlogService } from '../../services/blog.service';
import { Paginator, CommonService } from '../../services/common';
import { PageEvent, MatPaginatorIntl } from '@angular/material/paginator';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.sass']
})
export class HomeComponent implements OnInit {

  index = 0;
  size = 10;
  totalSize = 0;
  search = '';

  blogList: BlogItem[] = [];

  constructor(
    private pagerInit: MatPaginatorIntl,
    private blog: BlogService,
    private common: CommonService,
    private route: ActivatedRoute
  ) { }

  ngOnInit(): void {
    this.route.paramMap.subscribe(r => {
      this.search = r.get('s');
      this.getBlogList();
    });

    this.pagerInit.nextPageLabel = '下一页';
    this.pagerInit.previousPageLabel = '上一页';
    this.pagerInit.itemsPerPageLabel = '当前条数';
    this.pagerInit.getRangeLabel = (index: number, size: number, totalSize: number) => {
      return `${index} - ${size} 共 ${totalSize}`;
    };
  }

  private getBlogList() {
    this.blog.getBlogList(this.index, this.size, this.search).subscribe(resp => {
      if (resp.status === 200) {
        const RESP = resp.data as Paginator<BlogItem>;
        this.totalSize = RESP.totalSize;
        this.blogList = RESP.list;
      } else {
        this.common.snackOpen('获取失败');
      }
    });
  }

  pageChange(pager: PageEvent) {
    this.index = pager.pageIndex;
    this.getBlogList();
  }
}
