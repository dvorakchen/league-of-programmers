import { NgModule } from '@angular/core';
import { SharedModule } from '../../shared/shared.module';

import { IdentityRoutingModule } from './identity-routing.module';
import { LoginComponent } from './login/login.component';
import { RegisterComponent } from './register/register.component';


@NgModule({
  declarations: [LoginComponent, RegisterComponent],
  imports: [
    SharedModule,
    IdentityRoutingModule
  ]
})
export class IdentityModule { }
