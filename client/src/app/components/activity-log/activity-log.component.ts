import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ActivityLog } from 'src/app/_models/activity-log.model';
import { UserService } from 'src/app/_services/user.service';

@Component({
  selector: 'app-activity-log',
  templateUrl: './activity-log.component.html',
  styleUrls: ['./activity-log.component.css']
})
export class ActivityLogComponent implements OnInit {
  activities: ActivityLog[] = [];

  constructor(private route: ActivatedRoute, private userService: UserService) {
    this.activities = this.route.snapshot.data['activities'];
    console.log(this.activities);
  }

  ngOnInit(): void {
  }

  fixDate(date: Date){
    let d = new Date(date);
    let fixedDate = d.toLocaleDateString('en-GB');
    let timeStamp = d.toTimeString().slice(0, 8);
    let output = fixedDate + ' @ ' + timeStamp
    return output;
  }
  
}
