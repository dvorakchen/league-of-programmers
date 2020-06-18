import { Component, OnInit, AfterViewInit, OnDestroy } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { NewBlog, BlogService } from '../../../services/blog.service';
import { CommonService, CreatedResult } from '../../../services/common';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-blog-write',
  templateUrl: './blog-write.component.html',
  styleUrls: ['./blog-write.component.sass']
})
export class BlogWriteComponent implements OnInit, AfterViewInit, OnDestroy {

  preview = ``;
  contentBox: Subscription;
  isPosting = false;

  blog = this.fb.group({
    title: ['', [Validators.required]],
    targets: ['', [Validators.required]],
    content: ['', [Validators.required]]
  });

  constructor(
    private fb: FormBuilder,
    private blogService: BlogService,
    private common: CommonService,
    private router: Router
  ) { }

  ngOnDestroy(): void {
    this.contentBox.unsubscribe();
  }

  ngOnInit(): void {
  }

  ngAfterViewInit(): void {
    const CONTENT_DOM = document.getElementById('txt_content');
    this.contentBox = this.common.setTabEvent(CONTENT_DOM);
  }

  postBlog() {
    if (this.blog.invalid) {
      return;
    }

    this.isPosting = true;

    const NEW_BLOG: NewBlog = {
      title: this.blog.get('title').value.trim(),
      //  targets: this.blog.get('targets').value.trim().split(','),
      targets: [],
      content: this.blog.get('content').value
    };

    const targetsV1: string[] = this.blog.get('targets').value.trim().split(',');
    targetsV1.forEach(v => {
      NEW_BLOG.targets.push(...v.split('，'));
    });

    this.blogService.writeBlog(NEW_BLOG).subscribe(resp => {
      switch (resp.status) {
        case 201:
          {
            this.common.snackOpen('发布成功', 3000);
            const R = resp as CreatedResult;
            this.router.navigateByUrl(R.location);
          }
          break;
        case 400:
          {
            this.common.snackOpen(resp.data);
          }
          break;
        default:
          this.common.snackOpen('发布失败');
          break;
      }
      this.isPosting = false;
    });
  }

  changeTab(index: number) {
    if (index === 1) {
      this.preview = this.blog.get('content').value;
    }
  }
}
