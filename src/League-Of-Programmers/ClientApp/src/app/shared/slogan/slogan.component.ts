import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-slogan',
  template: `
<div class="slogan">
- 我们是软件工程师
</div>
  `,
  styleUrls: ['./slogan.component.sass']
})
export class SloganComponent implements OnInit {

    constructor() {}

    ngOnInit() {

    }
}
