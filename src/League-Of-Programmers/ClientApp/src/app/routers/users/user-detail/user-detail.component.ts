import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { CommonService } from '../../../services/common';
import { UserService } from '../../../services/user.service';
import { BlogService, BlogItem } from '../../../services/blog.service';

@Component({
  selector: 'app-user-detail',
  templateUrl: './user-detail.component.html',
  styleUrls: ['./user-detail.component.sass']
})
export class UserDetailComponent implements OnInit {

  isSelf = false;
  account = '';

  blogList: BlogItem[] = [];

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private common: CommonService,
    private user: UserService,
    private blog: BlogService
  ) { }

  ngOnInit(): void {
    this.route.paramMap.subscribe(p => {
      this.account = p.get('name');
      if (this.account) {
        this.user.isSelf().subscribe(resp => {
          this.isSelf = (resp.status === 200 && resp.data === true);
        });
      } else {
        this.router.navigate(['/']);
      }
    });
  }

  private getBlogList() {
    this.blog.getBlogsByUser(this.account).subscribe(resp => {
      if (resp.status === 200) {
        this.blogList = resp.data as BlogItem[];
      } else {
        this.common.snackOpen(resp.data, 3000);
      }
    });
  }

}
