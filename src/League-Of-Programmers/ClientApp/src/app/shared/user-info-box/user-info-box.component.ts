import { Component, OnInit, Input } from '@angular/core';
import { Router } from '@angular/router';
import { CommonService } from '../../services/common';
import { UserService, Profile, UserInfo } from '../../services/user.service';

@Component({
  selector: 'app-user-info-box',
  templateUrl: './user-info-box.component.html',
  styleUrls: ['./user-info-box.component.sass']
})
export class UserInfoBoxComponent implements OnInit {

  isSelf = false;

  private acc: string;

  @Input() set account(acc: string) {
    this.acc = acc;
    if (this.acc) {
      this.user.isSelf().subscribe(resp => {
        this.isSelf = (resp.status === 200 && resp.data === true);
      });
      this.getUserProfile();
    }
  }
  get account(): string {
    return this.acc;
  }

  profile: Profile = {
    avatar: '',
    userName: '',
    email: ''
  };
  editBoxAppearance = 'legacy';
  editUserInfo = false;
  editButtonText = '修改';
  editButtonIsLoading = false;

  constructor(
    private router: Router,
    private common: CommonService,
    private user: UserService,
  ) { }

  ngOnInit(): void {
  }

  private getUserProfile() {
    this.user.getProfile(this.account).subscribe(resp => {
      if (resp.status === 200) {
        this.profile = resp.data as Profile;
      } else if (resp.status === 404) {
        this.router.navigate(['/pages', '404']);
      }
    });
  }

  edit(userName: string, email: string) {
    userName = userName.trim();
    email = email.trim();

    if (this.profile.userName === userName && this.profile.email === email) {
      this.finish();
      return;
    }

    this.editButtonIsLoading = true;

    const model: UserInfo = {
      name: userName,
      email
    };

    this.user.modifyUserInfo(model).subscribe(resp => {
      if (resp.status === 200) {
        this.common.snackOpen('修改成功', 2000);
        this.finish();
      } else {
        this.common.snackOpen(resp.data, 3000);
      }
    });
    this.editButtonIsLoading = false;
  }
  private finish() {
    this.editUserInfo = !this.editUserInfo;
    this.editButtonText = this.editUserInfo ? '确认修改' : '修改';
    this.editBoxAppearance = this.editUserInfo ? 'outline' : 'legacy';
    this.editButtonIsLoading = false;
  }
}
