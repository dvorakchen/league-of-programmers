import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { BlogDetailComponent } from './blog-detail/blog-detail.component';
import { BlogWriteComponent } from './blog-write/blog-write.component';

const routes: Routes = [
  { path: ':id', component: BlogDetailComponent },
  { path: ':id/:title', component: BlogDetailComponent },
  { path: 'write', component: BlogWriteComponent }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class BlogsRoutingModule { }
