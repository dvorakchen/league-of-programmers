import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';

import { MaterialModule } from './material.module';

const EXPORTS_MODULE = [
  CommonModule,
  ReactiveFormsModule,
  FormsModule,
  RouterModule,
  MaterialModule
];

@NgModule({
  imports: [...EXPORTS_MODULE],
  exports: [...EXPORTS_MODULE]
})
export class CommonModules { }
