import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators, FormGroup } from '@angular/forms';
import { CommonService } from '../../../services/common';
import { RegisterModel, IdentityService } from '../../../services/identity.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.sass']
})
export class RegisterComponent implements OnInit {

  loading = false;
  registerInfo: FormGroup;

  constructor(
    private fb: FormBuilder,
    private common: CommonService,
    private identity: IdentityService,
    private router: Router
  ) { }

  ngOnInit(): void {
    this.registerInfo = this.fb.group({
      account: ['', [Validators.required, Validators.minLength(2)]],
      password: ['', [Validators.required, Validators.minLength(6)]],
      confirmPassword: ['', [Validators.required]],
    });
  }

  get account() {
    return this.registerInfo.get('account');
  }
  get password() {
    return this.registerInfo.get('password');
  }
  get confirmPassword() {
    return this.registerInfo.get('confirmPassword');
  }

  register() {
    if (this.registerInfo.invalid) {
      return;
    }

    this.loading = true;
    const MODEL: RegisterModel = {
      account: this.account.value.trim(),
      password: this.password.value.trim(),
      confirmPassword: this.confirmPassword.value.trim()
    };
    this.identity.register(MODEL).subscribe(resp => {
      if (resp.status === 201) {
        this.common.snackOpen('注册成功');
        this.router.navigate(['/login']);
      } else {
        this.common.snackOpen(resp.data, 10000);
      }
      this.loading = false;
    });
  }
}
