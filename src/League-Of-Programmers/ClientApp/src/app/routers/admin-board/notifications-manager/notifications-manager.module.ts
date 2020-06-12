import { NgModule } from '@angular/core';
import { SharedModule } from '../../../shared/shared.module';

import { NotificationsManagerRoutingModule } from './notifications-manager-routing.module';
import { ListComponent } from './list/list.component';
import { DetailComponent } from './detail/detail.component';
import { NewComponent } from './new/new.component';


@NgModule({
  declarations: [ListComponent, DetailComponent, NewComponent],
  imports: [
    SharedModule,
    NotificationsManagerRoutingModule
  ]
})
export class NotificationsManagerModule { }
