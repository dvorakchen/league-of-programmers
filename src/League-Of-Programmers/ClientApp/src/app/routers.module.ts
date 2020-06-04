import { NgModule } from '@angular/core';
import { SharedModule } from './shared/shared.module';
import { Routes, RouterModule } from '@angular/router';

import { HomeComponent } from './routers/home/home.component';

const routes: Routes = [
  //  { path: '', component: HomeComponent, pathMatch: 'full' },
  {
    path: '',
    loadChildren: () => import('./routers/home/home.module').then(mod => mod.HomeModule),
    pathMatch: 'full',
    data: {preload: true }
  },
  {
    path: 'blogs',
    loadChildren: () => import('./routers/blogs/blogs.module').then(mod => mod.BlogsModule),
    data: { preload: true }
  },
  {
    path: 'login',
    loadChildren: () => import('./routers/login/login.module').then(mod => mod.LoginModule)
  },
  {
    path: 'register',
    loadChildren: () => import('./routers/register/register.module').then(mod => mod.RegisterModule)
  },
  {
    path: 'pages',
    loadChildren: () => import('./routers/pages/pages.module').then(mod => mod.PagesModule)
  },
  {
    path: ':name',
    loadChildren: () => import('./routers/users/users.module').then(mod => mod.UsersModule)
  }
];

@NgModule({
  imports: [
    SharedModule,
    RouterModule.forRoot(routes)
  ],
  exports: [RouterModule]
})
export class RoutesModule { }
