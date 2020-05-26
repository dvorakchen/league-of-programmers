import { Component, OnInit } from '@angular/core';
import { Router, NavigationEnd, ActivatedRoute } from '@angular/router';
import { REDIRECT, CommonService } from '../../services/common';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent implements OnInit {

  showActions = true;

  constructor(
    private router: Router,
    private common: CommonService,
    private route: ActivatedRoute
  ) { }

  ngOnInit(): void {
    this.router.events.subscribe(event => {
      if (event instanceof NavigationEnd) {
        const ROUTER = event.url.split(';')[0];
        this.showActions = ROUTER !== '/login';
      }
    });
  }

  writeBlog() {
    /*
     * 没有登陆就跳到登录页再去写博文
     */

    this.router.navigate(['/login', { redirect: location.pathname + location.search }]);
  }
}
