import { Component, OnInit } from '@angular/core';
import { IdentityService } from '../../../services/identity.service';
import { CommonService, REDIRECT } from '../../../services/common';
import { FormBuilder, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
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
    private route: ActivatedRoute,
    private router: Router
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
        let red = this.route.snapshot.paramMap.get(REDIRECT);
        if (!red) {
          red = '/';
        }
        console.log(data.data);
        this.router.navigateByUrl(red);
      } else {
        this.common.snackOpen(data.data);
      }
      this.loading = false;
    });
  }
}
