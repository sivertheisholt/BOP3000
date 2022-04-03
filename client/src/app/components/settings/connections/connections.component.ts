import { DOCUMENT } from '@angular/common';
import { HttpBackend, HttpClient, HttpHeaders } from '@angular/common/http';
import { Component, Inject, OnInit } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { map } from 'rxjs/operators';

@Component({
  selector: 'app-connections',
  templateUrl: './connections.component.html',
  styleUrls: ['./connections.component.css']
})
export class ConnectionsComponent implements OnInit {
  connectToSteamForm!: FormGroup;
  private httpClient: HttpClient;

  constructor(private http: HttpClient, private route: ActivatedRoute, @Inject(DOCUMENT) private document: Document, private handler: HttpBackend) {
    this.httpClient = new HttpClient(handler);
    if(this.route.snapshot.queryParams.success != null)
    {
      this.steamSuccess();
    }
  }

  steamSuccess(): void {
    this.http.patch('https://localhost:5001/api/accounts/steam-success', {}).subscribe(
      response => {
        console.log(response);
      }
    )
  }

  ngOnInit(): void {
  }

  onSubmit(){
    let options = {
      headers: new HttpHeaders().set('Content-Type', 'application/x-www-form-urlencoded'),
      responseType: "text" as "json", observe: "response" as "body"
  };
    this.httpClient.post('https://localhost:5001/api/accounts/steam', "", options).subscribe(
      (response: any) => {
        this.document.location.href = response.url!;
      }
    )
  }
}