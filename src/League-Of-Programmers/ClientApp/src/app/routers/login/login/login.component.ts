import { Component, OnInit } from '@angular/core';
import { IdentityService } from '../../../services/identity.service';
import { CommonService, REDIRECT } from '../../../services/common';
import { FormBuilder, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { Global } from '../../../global';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.sass']
})
export class LoginComponent implements OnInit {

  hide = true;
  loading = false;

  loginInfo = this.fb.group({
    account: ['', [Validators.required]],
    password: ['', [Validators.required]],
  });

  get account() { return this.loginInfo.get('account'); }
  get password() { return this.loginInfo.get('password'); }

  constructor(
    private identity: IdentityService,
    private common: CommonService,
    private fb: FormBuilder,
    private route: ActivatedRoute
  ) { }

  ngOnInit(): void {
  }

  login() {
    if (this.loginInfo.invalid) {
      return;
    }

    this.loading = true;
    this.identity.login(this.account.value.trim(), this.password.value.trim()).subscribe(data => {
      if (data.status === 200) {
        Global.loginInfo = data.data;
        let red = this.route.snapshot.paramMap.get(REDIRECT);
        if (!red) {
          red = '/';
        }
        location.href = red;
      } else {
        Global.loginInfo = null;
        this.common.snackOpen(data.data, 10000);
      }
      this.loading = false;
    });
  }
}
