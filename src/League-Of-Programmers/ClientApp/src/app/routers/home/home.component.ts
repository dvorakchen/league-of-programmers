import { Component } from '@angular/core';
import { IdentityService } from '../../services/identity.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent {

  constructor(
    private identity: IdentityService
  ) { }

  check() {
    this.identity.checkIsLoggedIn().subscribe(resp => {
      if (resp.status === 200) {
        alert(resp.data);
      }
    });
  }
}
