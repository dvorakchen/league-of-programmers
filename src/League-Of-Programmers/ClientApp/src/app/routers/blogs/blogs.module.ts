import { NgModule } from '@angular/core';
import { SharedModule } from '../../shared/shared.module';

import { MarkdownModule } from 'ngx-markdown';

import { BlogsRoutingModule } from './blogs-routing.module';
import { BlogDetailComponent } from './blog-detail/blog-detail.component';


@NgModule({
  declarations: [BlogDetailComponent],
  imports: [
    SharedModule,
    BlogsRoutingModule,
    MarkdownModule.forChild()
  ]
})
export class BlogsModule { }
