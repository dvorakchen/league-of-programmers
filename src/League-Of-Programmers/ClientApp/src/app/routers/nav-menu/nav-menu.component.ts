import { Component, OnInit } from '@angular/core';
import { Location } from '@angular/common';
import { Router, NavigationEnd, ActivatedRoute } from '@angular/router';
import { CommonService } from '../../services/common';
import { Global } from '../../global';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.sass']
})
export class NavMenuComponent implements OnInit {

  showActions = true;
  isLoggedIn = false;
  userName = '';

  constructor(
    private router: Router,
    private common: CommonService,
    private loc: Location,
    private route: ActivatedRoute
  ) { }

  ngOnInit(): void {
    this.router.events.subscribe(event => {
      if (event instanceof NavigationEnd) {
        const ROUTER = event.url.split(';')[0];
        this.showActions = ROUTER !== '/login';
      }
    });
    if (Global.loginInfo) {
      this.isLoggedIn = true;
      this.userName = Global.loginInfo.userName;
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
