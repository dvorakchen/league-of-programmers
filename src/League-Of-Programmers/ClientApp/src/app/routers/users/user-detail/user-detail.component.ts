import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { CommonService } from '../../../services/common';
import {} from '../../../services/identity.service';

@Component({
  selector: 'app-user-detail',
  templateUrl: './user-detail.component.html',
  styleUrls: ['./user-detail.component.css']
})
export class UserDetailComponent implements OnInit {

  isSelf = true;

  editBoxAppearance = 'legacy';
  editUserInfo = false;
  editButtonText = '修改';
  editButtonIsLoading = false;

  constructor(
    private route: ActivatedRoute,
    private common: CommonService
  ) { }

  ngOnInit(): void {
  }

  edit() {
    if (this.editUserInfo) {
      this.common.snackOpen('编辑中', 3000);
    }

    this.editUserInfo = !this.editUserInfo;
    this.editButtonText = this.editUserInfo ? '确认修改' : '修改';
    this.editBoxAppearance = this.editUserInfo ? 'outline' : 'legacy';
    this.editButtonIsLoading = false;
  }
}
