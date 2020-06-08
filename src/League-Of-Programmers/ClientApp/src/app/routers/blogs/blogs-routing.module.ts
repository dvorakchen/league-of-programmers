import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { BlogDetailComponent } from './blog-detail/blog-detail.component';
import { BlogWriteComponent } from './blog-write/blog-write.component';
import { EditComponent } from './edit/edit.component';
import { LoginGuard } from '../../guards/login.guard';

const routes: Routes = [
  { path: 'write', component: BlogWriteComponent, canActivate: [LoginGuard] },
  { path: ':id/edit', component: EditComponent, canActivate: [LoginGuard] },
  { path: ':id', component: BlogDetailComponent },
  { path: ':id/:title', component: BlogDetailComponent }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class BlogsRoutingModule { }
