import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-blog-detail',
  templateUrl: './blog-detail.component.html',
  styleUrls: ['./blog-detail.component.sass']
})
export class BlogDetailComponent implements OnInit {

  content = `
  ## title 2
  \`\`\`js
  publci class static Main()
  {
    Console.WriteLine('Hello world');
  }
  \`\`\`
  `;
  account = 'myfor';
  constructor() { }

  ngOnInit(): void {
  }

}
