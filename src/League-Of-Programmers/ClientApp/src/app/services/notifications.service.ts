import { Injectable } from '@angular/core';
import { CommonService, ServicesBase } from './common';

@Injectable({
  providedIn: 'root'
})
export class NotificationsService {

  constructor(
    private common: CommonService,
    private base: ServicesBase
  ) { }
}
