import { NgModule } from '@angular/core';
import { CommonModules } from './shared/common.module';
import { Routes, RouterModule } from '@angular/router';

import { HomeComponent } from './home/home.component';
import { CounterComponent } from './counter/counter.component';
import { FetchDataComponent } from './fetch-data/fetch-data.component';

const routes: Routes = [
  //{ path: '', component: HomeComponent, pathMatch: 'full' },
  //{ path: 'counter', component: CounterComponent },
  //{ path: 'fetch-data', component: FetchDataComponent },
];

@NgModule({
  imports: [
    CommonModules,
    RouterModule.forRoot(routes)
  ],
  exports: [RouterModule]
})
export class RoutesModule { }
