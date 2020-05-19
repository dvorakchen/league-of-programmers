import { NgModule } from '@angular/core';
import { SharedModule } from '../shared/shared.module';

import { IdentityRoutingModule } from './identity-routing.module';
import { LoginComponent } from './login/login.component';


@NgModule({
  declarations: [LoginComponent],
  imports: [
    SharedModule,
    IdentityRoutingModule
  ]
})
export class IdentityModule { }
