import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { Country } from 'src/app/_models/country.model';

@Component({
  selector: 'app-profile-settings',
  templateUrl: './profile-settings.component.html',
  styleUrls: ['./profile-settings.component.css']
})
export class ProfileSettingsComponent implements OnInit {
  profileSettingsForm! : FormGroup
  countries : Country[] = [];
  genders = ['Male', 'Female', 'Other', 'Hidden'];

  constructor(private route: ActivatedRoute) { }

  ngOnInit(): void {
    this.countries = this.route.snapshot.data['countries'];
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
