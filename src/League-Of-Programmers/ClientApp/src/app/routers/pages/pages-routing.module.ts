import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { Page404Component } from './page404/page404.component';
import { Page429Component } from './page429/page429.component';

const routes: Routes = [
  { path: '', component: Page404Component, pathMatch: 'full' },
  { path: '404', component: Page404Component },
  { path: '429', component: Page429Component }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class PagesRoutingModule { }
