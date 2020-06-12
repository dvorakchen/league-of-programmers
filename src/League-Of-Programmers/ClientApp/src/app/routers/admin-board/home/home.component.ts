import { Component, OnInit } from '@angular/core';
import { AdminMenuService } from '../../../services/admin-menu.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.sass']
})
export class HomeComponent implements OnInit {

  crumbs = '';
  menuList = [];


  constructor(
    private adminService: AdminMenuService
  ) { }

  ngOnInit(): void {
    this.adminService.getMenus().subscribe(r => {
      this.menuList = (r as any).data.menu;
    });
  }

  setCrubms(crumbs: string) {
    this.crumbs = crumbs;
  }
}
