import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { NoticeContentComponent } from '../../../shared/notice-board/notice-content.component';

@Component({
  selector: 'app-list',
  templateUrl: './list.component.html',
  styleUrls: ['./list.component.sass']
})
export class ListComponent implements OnInit {

  constructor(private dialog: MatDialog) { }

  ngOnInit(): void {
  }

  openNotice(id: number) {
    this.dialog.open(NoticeContentComponent, {
      minWidth: '40%',
      minHeight: '70%',
      data: id
    });
  }
}
