import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { HomeComponent } from './home/home.component';

const routes: Routes = [
  {
    path: '', component: HomeComponent,
    children: [
      {
        path: 'notifications',
        loadChildren: () => import('./notifications-manager/notifications-manager.module').then(mod => mod.NotificationsManagerModule)
      },
      {
        path: 'blogs',
        loadChildren: () => import('./blogs-manager/blogs-manager.module').then(mod => mod.BlogsManagerModule)
      }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AdminBoardRoutingModule { }
