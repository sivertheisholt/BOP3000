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

  constructor(private http: HttpClient, private route: ActivatedRoute, @Inject(DOCUMENT) private document: Document) {
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
    this.connectToSteamForm = new FormGroup({
      Provider: new FormControl("Steam"),
      ReturnUrl: new FormControl("/")
    });
  }

  onSubmit(){
    this.http.post('https://localhost:5001/api/accounts/steam', this.connectToSteamForm.value, {responseType: "text", observe: 'response'}, ).subscribe(
      response => {
        this.document.location.href = response.url!;
      }
    )
  }
}