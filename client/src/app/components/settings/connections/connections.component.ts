import { HttpClient, HttpHeaders } from '@angular/common/http';
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
    const formData = new FormData();
    Object.keys(this.connectToSteamForm.value).forEach((key) => {
      console.log(key);
      formData.append(key, this.connectToSteamForm.value[key]);
    });
    formData.append("Name", this.connectToSteamForm.controls['Name'].value);
    formData.append("ReturnUrl", this.connectToSteamForm.controls['ReturnUrl'].value);
    console.log(formData);
    console.log(formData.get('Name'));
/*     console.log(this.connectToSteamForm.value);
    this.http.post('URL HER', formData).pipe(
      map((response) => {
        console.log(response);
      })
    ).subscribe(
      response => {
        console.log(response);
      }
    ) */
  }

}
