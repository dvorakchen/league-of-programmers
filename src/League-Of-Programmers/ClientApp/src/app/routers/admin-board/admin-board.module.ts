import { NgModule } from '@angular/core';
import { SharedModule } from '../../shared/shared.module';

import { AdminBoardRoutingModule } from './admin-board-routing.module';
import { HomeComponent } from './home/home.component';


@NgModule({
  declarations: [HomeComponent],
  imports: [
    SharedModule,
    AdminBoardRoutingModule
  ]
})
export class AdminBoardModule { }
