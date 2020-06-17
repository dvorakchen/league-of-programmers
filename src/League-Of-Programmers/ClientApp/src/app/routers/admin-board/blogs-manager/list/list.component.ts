import { Component, OnInit } from '@angular/core';
import { BlogItem, BlogService } from '../../../../services/blog.service';
import { Paginator } from '../../../../services/common';
import { PageEvent } from '@angular/material/paginator';

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
  state = '';
  dataSource: BlogItem[] = [];
  displayedColumns: string[] = ['title', 'authorAccount', 'state', 'dateTime', 'actions'];
  constructor(
    private blog: BlogService
  ) { }

  ngOnInit(): void {
    this.getBlogsList();
  }

  private getBlogsList() {
    this.blog.getBlogsListByAdministartor(this.index + 1, this.size, this.state, this.search)
    .subscribe(resp => {
      if (resp.status === 200) {
        const R = resp.data as Paginator<BlogItem>;
        this.totalSize = R.totalSize;
        this.dataSource = R.list;
      }
    });
  }

  searchList() {
    this.search = this.search.trim();
    this.getBlogsList();
  }

  pageChange(pager: PageEvent) {
    this.index = pager.pageIndex;
    this.getBlogsList();
  }
}
