import { NgModule } from '@angular/core';
import { SharedModule } from '../../../shared/shared.module';

import { BlogsManagerRoutingModule } from './blogs-manager-routing.module';
import { ListComponent } from './list/list.component';


@NgModule({
  declarations: [ListComponent],
  imports: [
    SharedModule,
    BlogsManagerRoutingModule
  ]
})
export class BlogsManagerModule { }
