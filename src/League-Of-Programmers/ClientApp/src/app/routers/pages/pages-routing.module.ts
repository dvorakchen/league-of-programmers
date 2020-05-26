import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { Page404Component } from './page404/page404.component';

const routes: Routes = [
  { path: '', component: Page404Component, pathMatch: 'full' },
  { path: '404', component: Page404Component }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class PagesRoutingModule { }
