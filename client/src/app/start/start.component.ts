import { Component, OnInit } from '@angular/core';
import { faEnvelope, faLock } from '@fortawesome/free-solid-svg-icons';

@Component({
  selector: 'app-start',
  templateUrl: './start.component.html',
  styleUrls: ['./start.component.css']
})
export class StartComponent implements OnInit {
  faEnvelope = faEnvelope; faLock = faLock;
  constructor() { }

  ngOnInit(): void {
  }

}
