import { DOCUMENT } from '@angular/common';
import { HttpBackend, HttpClient, HttpHeaders } from '@angular/common/http';
import { Component, Inject, OnInit } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { map } from 'rxjs/operators';

@Component({
  selector: 'app-connections',
  templateUrl: './connections.component.html',
  styleUrls: ['./connections.component.css']
})
export class ConnectionsComponent implements OnInit {
  connectToSteamForm!: FormGroup;

  private httpClient: HttpClient;

  constructor(private handler: HttpBackend, @Inject(DOCUMENT) private document: Document) {
    this.httpClient = new HttpClient(handler);
  }

  ngOnInit(): void {
    this.connectToSteamForm = new FormGroup({
      Provider: new FormControl("Steam"),
      ReturnUrl: new FormControl("/")
    });
  }

  onSubmit(){
    this.httpClient.post('https://localhost:5001/api/accounts/steam', this.connectToSteamForm.value, {responseType: "text", observe: 'response'}, ).subscribe(
      response => {
        this.document.location.href = response.url!;
      }
    )
  }

}
