import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';

@Component({
    selector: 'app-notice-content',
    templateUrl: './notice-content.component.html',
    styleUrls: ['./notice-content.component.sass']
})
export class NoticeContentComponent implements OnInit {

    constructor(
        public dialogRef: MatDialogRef<NoticeContentComponent>,
        @Inject(MAT_DIALOG_DATA) public data: number) {}

    ngOnInit() {}

    close() {
        this.dialogRef.close();
    }
}
