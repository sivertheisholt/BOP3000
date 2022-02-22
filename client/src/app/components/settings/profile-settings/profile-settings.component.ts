import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-profile-settings',
  templateUrl: './profile-settings.component.html',
  styleUrls: ['./profile-settings.component.css']
})
export class ProfileSettingsComponent implements OnInit {
  profileSettingsForm! : FormGroup
  countries = [
    {code: 'no', name: 'Norway'},
    {code: 'zh', name: 'China'},
    {code: 'fr', name: 'France'},
    {code: 'de', name: 'Germany'},
    {code: 'se', name: 'Sweden'},
  ];

  genders = ['Male', 'Female', 'Other', 'Hidden'];

  constructor() { }

  ngOnInit(): void {
    this.profileSettingsForm = new FormGroup({
      username: new FormControl(null, Validators.required),
      email: new FormControl(null, Validators.required),
      description: new FormControl(null, Validators.required),
      gender: new FormControl(null, Validators.required),
      country: new FormControl(null, Validators.required)
    });
  }

  onSubmit(){
    console.log(this.profileSettingsForm);
  }

}
