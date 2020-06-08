import { NgModule } from '@angular/core';
import { SharedModule } from '../../shared/shared.module';

import { MarkdownModule } from 'ngx-markdown';

import { BlogsRoutingModule } from './blogs-routing.module';
import { BlogDetailComponent } from './blog-detail/blog-detail.component';
import { BlogWriteComponent } from './blog-write/blog-write.component';
import { EditComponent } from './edit/edit.component';


@NgModule({
  declarations: [BlogDetailComponent, BlogWriteComponent, EditComponent],
  imports: [
    SharedModule,
    BlogsRoutingModule,
    MarkdownModule.forChild()
  ]
})
export class BlogsModule { }
