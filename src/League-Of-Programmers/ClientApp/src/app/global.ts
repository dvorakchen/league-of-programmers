import { isDevMode } from '@angular/core';

export interface UserInfo {
  userName: string;
  role: RoleCategories;
}

export enum RoleCategories {
  // tslint:disable-next-line: no-bitwise
  Client = 1 << 0,
  // tslint:disable-next-line: no-bitwise
  Administrator = 2 << 1,
}

export class Global {
  private static USERNAME_STORE_KEY = '__username__';
  private static ROLE_STORE_KEY = '__role__';

  public static get loginInfo(): UserInfo {
    if (!this.userName || !this.role) {
      return null;
    }

    const u: UserInfo = {
      userName: this.userName,
      role: this.role
    };

    if (isDevMode()) {
      console.log(`get login info: username: ${u.userName} - role: ${u.role}`);
    }
    return u;
  }

  public static set loginInfo(userInfo: UserInfo) {
    if (userInfo) {
      this.userName = userInfo.userName;
      this.role = userInfo.role;
    } else {
      localStorage.removeItem(this.ROLE_STORE_KEY);
      localStorage.removeItem(this.USERNAME_STORE_KEY);
    }
    if (isDevMode() && userInfo) {
      console.log(`set login info: username: ${userInfo.userName} - role: ${userInfo.role}`);
    }
  }

  private static get userName(): string {
    const name = localStorage.getItem(this.USERNAME_STORE_KEY);
    if (name) {
      return name;
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
