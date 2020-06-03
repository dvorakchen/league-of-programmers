import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-page429',
  template: `
<div class="page-container">
<span class="circuit-breaker-title">League Of Programmers</span>
<div class="circuit-breaker-description">
  您请求地太频繁了，请稍后再试
</div>
</div>
`,
  styleUrls: ['./page429.component.sass']
})
export class Page429Component implements OnInit {

  constructor() { }

  ngOnInit(): void {
  }

}
