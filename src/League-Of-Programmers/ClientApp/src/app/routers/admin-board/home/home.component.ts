import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AdminMenuService } from '../../../services/admin-menu.service';
import { UserService, Profile } from '../../../services/user.service';
import { Global } from '../../../global';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.sass']
})
export class HomeComponent implements OnInit {

  crumbs = '';
  menuList = [];

  profile: Profile = {
    avatar: '',
    userName: '',
    email: ''
  };

  constructor(
    private adminService: AdminMenuService,
    private user: UserService,
    private router: Router
  ) { }

  ngOnInit(): void {
    this.adminService.getMenus().subscribe(r => {
      this.menuList = (r as any).data.menu;
    });
    this.getUserProfile();
  }

  private getUserProfile() {
    this.user.getProfile(Global.loginInfo.account).subscribe(resp => {
      if (resp.status === 200) {
        this.profile = resp.data as Profile;
      } else if (resp.status === 404) {
        this.router.navigate(['/pages', '404']);
      }
    });
  }

  setCrubms(crumbs: string) {
    this.crumbs = crumbs;
  }
}
