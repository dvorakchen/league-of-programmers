import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from '../../shared/shared.module';

import { PagesRoutingModule } from './pages-routing.module';
import { Page404Component } from './page404/page404.component';
import { Page429Component } from './page429/page429.component';


@NgModule({
  declarations: [Page404Component, Page429Component],
  imports: [
    SharedModule,
    PagesRoutingModule
  ]
})
export class PagesModule { }
