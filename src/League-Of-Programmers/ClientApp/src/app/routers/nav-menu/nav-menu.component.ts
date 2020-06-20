import { Component, OnInit, OnDestroy } from '@angular/core';
import { Router, NavigationEnd } from '@angular/router';
import { CommonService } from '../../services/common';
import { Global, RoleCategories } from '../../global';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.sass']
})
export class NavMenuComponent implements OnInit, OnDestroy {

  showActions = true;
  isLoggedIn = false;
  isAdministrator = false;
  userName = '';
  account = '';

  constructor(
    private router: Router,
    private common: CommonService
  ) { }

  sub: Subscription;
  ngOnDestroy(): void {
    this.sub.unsubscribe();
  }

  ngOnInit(): void {
    this.sub = this.router.events.subscribe(event => {
      if (event instanceof NavigationEnd) {
        const ROUTER = event.url.split(';')[0];
        this.showActions = ROUTER !== '/login';
      }
    });
    if (Global.loginInfo) {
      this.isLoggedIn = true;
      this.userName = Global.loginInfo.userName;
      this.account = Global.loginInfo.account;
      // tslint:disable-next-line: no-bitwise
      this.isAdministrator = (Global.loginInfo.role & RoleCategories.Administrator) !== 0;
    } else {
      this.isLoggedIn = false;
    }
  }

  search(value: string) {
    if (!value.trim()) {
      return;
    }
    this.common.snackOpen(value, 1000);
  }

  logout() {
    Global.loginInfo = null;
    location.href = '/';
  }
}
