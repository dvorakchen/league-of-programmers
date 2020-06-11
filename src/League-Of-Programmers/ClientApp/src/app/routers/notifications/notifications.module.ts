import { NgModule } from '@angular/core';
import { MaterialModule } from '../../shared/material.module';

import { NotificationsRoutingModule } from './notifications-routing.module';
import { ListComponent } from './list/list.component';


@NgModule({
  declarations: [ListComponent],
  imports: [
    MaterialModule,
    NotificationsRoutingModule
  ]
})
export class NotificationsModule { }
