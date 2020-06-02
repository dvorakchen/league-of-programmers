import { NgModule } from '@angular/core';
import { SharedModule } from './shared/shared.module';
import { Routes, RouterModule } from '@angular/router';

import { HomeComponent } from './routers/home/home.component';

const routes: Routes = [
  { path: '', component: HomeComponent, pathMatch: 'full' },
  {
    path: 'login',
    loadChildren: () => import('./routers/login/login.module').then(mod => mod.LoginModule),
    data: { preload: true }
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
    loadChildren: () => import('./routers/users/users.module').then(mod => mod.UsersModule),
    data: { preload: true }
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
