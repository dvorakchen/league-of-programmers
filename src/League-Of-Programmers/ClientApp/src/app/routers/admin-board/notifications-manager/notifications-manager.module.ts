import { NgModule } from '@angular/core';
import { SharedModule } from '../../../shared/shared.module';

import { NotificationsManagerRoutingModule } from './notifications-manager-routing.module';
import { ListComponent } from './list/list.component';


@NgModule({
  declarations: [ListComponent],
  imports: [
    SharedModule,
    NotificationsManagerRoutingModule
  ]
})
export class NotificationsManagerModule { }
