import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { NoticeContentComponent } from './notice-content.component';

@Component({
  selector: 'app-notice-board',
  templateUrl: './notice-board.component.html',
  styleUrls: ['./notice-board.component.sass']
})
export class NoticeBoardComponent implements OnInit {

    constructor(private dialog: MatDialog) {}

    ngOnInit() {
    }

    openNotice(id: number) {
      this.dialog.open(NoticeContentComponent, {
        minWidth: '50%',
        minHeight: '70%',
        data: id
      });
    }
}
