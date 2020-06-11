import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, UrlTree, Router } from '@angular/router';
import { Observable } from 'rxjs';
import { Global, RoleCategories } from '../global';
import { IdentityService } from '../services/identity.service';

@Injectable({
  providedIn: 'root'
})
export class AdminGuard implements CanActivate {

  constructor(
    private router: Router,
    private identity: IdentityService
  ) {}

  canActivate(
    next: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {

    // tslint:disable-next-line: no-bitwise
    if (!Global.loginInfo || (Global.loginInfo.role & RoleCategories.Administrator) === 0) {
        this.router.navigate(['/login']);
        return false;
    }

    return this.isAdminLogged();
  }

  isAdminLogged(): Observable<boolean> {
    return new Observable<boolean>((ob) => {
      this.identity.checkIsAdministratorLoggedIn().subscribe(r => {
        if (r.status === 200) {
            Global.loginInfo = r.data;
            ob.next(true);
        } else {
          Global.loginInfo = null;
        }
        ob.complete();
      });
    });
  }
}
