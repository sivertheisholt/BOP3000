import { Component, OnInit } from '@angular/core';
import { faUser, faUserCircle, faSignOutAlt } from '@fortawesome/free-solid-svg-icons';

@Component({
  selector: 'app-sidebar',
  templateUrl: './sidebar.component.html',
  styleUrls: ['./sidebar.component.css']
})
export class SidebarComponent implements OnInit {
  faUser = faUser; faSignOutAlt = faSignOutAlt; faUserCircle = faUserCircle;
  constructor() { }

  ngOnInit(): void {
  }

}
