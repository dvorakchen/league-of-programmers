import { NgModule } from '@angular/core';
import { SharedModule } from './shared/shared.module';
import { Routes, RouterModule } from '@angular/router';

import { HomeComponent } from './home/home.component';

const routes: Routes = [
  { path: '', component: HomeComponent, pathMatch: 'full' },
  {
    path: 'identity',
    loadChildren: () => import('./identity/identity.module').then(mod => mod.IdentityModule)
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
