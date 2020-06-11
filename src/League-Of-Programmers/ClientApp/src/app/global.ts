import { isDevMode } from '@angular/core';

export interface UserInfo {
  account: string;
  userName: string;
  role: RoleCategories;
}

export enum RoleCategories {
  // tslint:disable-next-line: no-bitwise
  Client = 1 << 0,
  // tslint:disable-next-line: no-bitwise
  Administrator = 1 << 1,
}

export class Global {
  private static USERNAME_STORE_KEY = '__username__';
  private static ROLE_STORE_KEY = '__role__';
  private static ACCOUNT_STORE_KEY = '__account__';

  public static get loginInfo(): UserInfo {
    if (!this.userName || !this.role) {
      return null;
    }

    const USER_INFO: UserInfo = {
      account: this.account,
      userName: this.userName,
      role: this.role
    };

    if (isDevMode()) {
      console.log(`get login info: username: ${USER_INFO.userName} - role: ${USER_INFO.role} - account: ${USER_INFO.account}`);
    }
    // console.log(` -- ${(this.role & RoleCategories.Client) === 0}`);
    // console.log(` -- ${(this.role & RoleCategories.Administrator) === 0}`);
    return USER_INFO;
  }

  public static set loginInfo(userInfo: UserInfo) {
    if (userInfo) {
      this.account = userInfo.account;
      this.userName = userInfo.userName;
      this.role = userInfo.role;
    } else {
      localStorage.removeItem(this.ROLE_STORE_KEY);
      localStorage.removeItem(this.USERNAME_STORE_KEY);
      localStorage.removeItem(this.ACCOUNT_STORE_KEY);
    }
    if (isDevMode() && userInfo) {
      console.log(`set login info: username: ${userInfo.userName} - role: ${userInfo.role} - account: ${userInfo.account}`);
    }
  }

  private static get account(): string {
    const NAME = localStorage.getItem(this.ACCOUNT_STORE_KEY);
    if (NAME) {
      return NAME;
    } else {
      return null;
    }
  }
  private static set account(account: string) {
    localStorage.setItem(this.ACCOUNT_STORE_KEY, account);
  }

  private static get userName(): string {
    const NAME = localStorage.getItem(this.USERNAME_STORE_KEY);
    if (NAME) {
      return NAME;
    } else {
      return null;
    }
  }
  private static set userName(name: string) {
    localStorage.setItem(this.USERNAME_STORE_KEY, name);
  }
  private static get role(): RoleCategories {
    return +localStorage.getItem(this.ROLE_STORE_KEY);
  }
  private static set role(role: RoleCategories) {
    localStorage.setItem(this.ROLE_STORE_KEY, role.toString());
  }
}
