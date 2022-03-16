import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { map } from 'rxjs/operators';

@Component({
  selector: 'app-connections',
  templateUrl: './connections.component.html',
  styleUrls: ['./connections.component.css']
})
export class ConnectionsComponent implements OnInit {
  connectToSteamForm!: FormGroup;

  constructor(private http: HttpClient) { }

  ngOnInit(): void {
    this.connectToSteamForm = new FormGroup({
      Name: new FormControl("Steam"),
      ReturnUrl: new FormControl("/")
    });
  }

  onSubmit(){
    console.log(this.connectToSteamForm.value);
    this.http.post('URL HER', this.connectToSteamForm.value).pipe(
      map((response) => {
        console.log(response);
      })
    ).subscribe(
      response => {
        console.log(response);
      }
    )
  }

}
