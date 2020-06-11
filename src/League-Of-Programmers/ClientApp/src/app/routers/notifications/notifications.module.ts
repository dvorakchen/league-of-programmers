import { NgModule } from '@angular/core';
import { SharedModule } from '../../shared/shared.module';

import { NotificationsRoutingModule } from './notifications-routing.module';
import { ListComponent } from './list/list.component';


@NgModule({
  declarations: [ListComponent],
  imports: [
    SharedModule,
    NotificationsRoutingModule
  ]
})
export class NotificationsModule { }
