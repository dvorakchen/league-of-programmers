import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, UrlTree, Router } from '@angular/router';
import { Observable, from } from 'rxjs';
import { Global } from '../global';
import { IdentityService } from '../services/identity.service';
import { CommonService } from '../services/common';

@Injectable({
  providedIn: 'root'
})
export class LoginGuard implements CanActivate {

  constructor(
    private router: Router,
    private identity: IdentityService,
    private common: CommonService
  ) {}

  canActivate(
    next: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {

    if (!Global.loginInfo) {
        this.router.navigate(['/login']);
        return false;
    }

    return this.isLogged();
  }

  isLogged(): Observable<boolean> {
    return new Observable<boolean>((ob) => {
      this.identity.checkIsLoggedIn().subscribe(r => {
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
