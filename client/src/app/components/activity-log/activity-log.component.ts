import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ActivityLog } from 'src/app/_models/activity-log.model';

@Component({
  selector: 'app-activity-log',
  templateUrl: './activity-log.component.html',
  styleUrls: ['./activity-log.component.css']
})
export class ActivityLogComponent implements OnInit {
  activities: ActivityLog[] = [];

  constructor(private route: ActivatedRoute) {
    this.activities = this.route.snapshot.data['activities'];

  }

  ngOnInit(): void {
  }

  fixDate(date: Date){
    let d = new Date(date);
    let fixedDate = ('0' + d.getDate()).slice(-2) + '.' + ('0' + d.getMonth()).slice(-2) + '.' + ('0' + d.getFullYear()).slice(-2) + ' @ ' + ('0' + d.getHours()).slice(-2) + ':' + ('0' + d.getMinutes()).slice(-2);
    return fixedDate;
  }
  
}
