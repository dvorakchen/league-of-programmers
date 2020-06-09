import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { CommonService, Paginator } from '../../../services/common';
import { UserService } from '../../../services/user.service';
import { BlogService, BlogItem, BlogState } from '../../../services/blog.service';
import { MatDialog } from '@angular/material/dialog';
import { DeleteConfirmDialogComponent } from '../../../shared/delete-confirm-dialog/delete-confirm-dialog.component';

@Component({
  selector: 'app-user-detail',
  templateUrl: './user-detail.component.html',
  styleUrls: ['./user-detail.component.sass']
})
export class UserDetailComponent implements OnInit {

  index = 0;
  size = 10;

  isSelf = false;
  account = '';

  blogList: BlogItem[] = [];

  constructor(
    private route: ActivatedRoute,
    private common: CommonService,
    private user: UserService,
    private blog: BlogService,
    private dialog: MatDialog
  ) { }

  ngOnInit(): void {
    this.route.paramMap.subscribe(p => {
      this.account = p.get('name');
      if (this.account) {
        this.user.isSelf().subscribe(resp => {
          this.isSelf = (resp.status === 200 && resp.data === true);
          this.getBlogList();
        });
      }
    });
  }

  private getBlogList() {
    const blogState = this.isSelf ? null : BlogState.Enabled;

    this.blog.getBlogsByUser(this.index + 1, this.size, blogState, this.account, '').subscribe(resp => {
      if (resp.status === 200) {
        const PAGER = resp.data as Paginator<BlogItem>;
        this.blogList = PAGER.list;
      } else {
        this.common.snackOpen(resp.data, 3000);
      }
    });
  }

  deleteBlog(id: number, title: string) {
    const DIA = this.dialog.open(DeleteConfirmDialogComponent, {
      data: title
    });
    DIA.afterClosed().subscribe(isconfirmDelete => {
      if (isconfirmDelete) {
        this.blog.deleteBlog(id).subscribe(resp => {
          if (resp.status === 200) {
            this.common.snackOpen('删除成功', 2000);
            this.blogList = this.blogList.filter(blog => blog.id !== id );
          } else {
            this.common.snackOpen(resp.data, 2000);
          }
        });
      }
    });
  }
}
