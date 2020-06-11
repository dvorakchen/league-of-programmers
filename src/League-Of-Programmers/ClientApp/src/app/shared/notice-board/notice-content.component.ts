import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { NotificationsService, NotificationDetail } from '../../services/notifications.service';

@Component({
    selector: 'app-notice-content',
    templateUrl: './notice-content.component.html',
    styleUrls: ['./notice-content.component.sass']
})
export class NoticeContentComponent implements OnInit {

    detail: NotificationDetail = {
        title: '',
        content: ''
    };

    constructor(
        private notification: NotificationsService,
        public dialogRef: MatDialogRef<NoticeContentComponent>,
        @Inject(MAT_DIALOG_DATA) public data: number) {}

    ngOnInit() {
        this.notification.getNotificationDetail(this.data).subscribe(resp => {
            if (resp.status === 200) {
                this.detail = resp.data as NotificationDetail;
            }
        });
    }

    close() {
        this.dialogRef.close();
    }
}
