import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { Country } from 'src/app/_models/country.model';
import { UserProfile } from 'src/app/_models/user-profile.model';
import { UserService } from 'src/app/_services/user.service';

@Component({
  selector: 'app-profile-settings',
  templateUrl: './profile-settings.component.html',
  styleUrls: ['./profile-settings.component.css']
})
export class ProfileSettingsComponent implements OnInit {
  profileSettingsForm! : FormGroup
  countries : Country[] = [];
  genders = ['Male', 'Female', 'Other', 'Hidden'];
  user?: UserProfile;

  constructor(private route: ActivatedRoute, private userService: UserService) {

    this.userService.getUserData().subscribe(
      (response) => {
        this.user = response;
        console.log(this.user);
      }
    );
    
   }

  ngOnInit(): void {
    
    this.countries = this.route.snapshot.data['countries'];
    this.profileSettingsForm = new FormGroup({
      username: new FormControl(null),
      email: new FormControl(null),
      description: new FormControl(null),
      gender: new FormControl(null),
      country: new FormControl(null),
      dateDay: new FormControl(null, [Validators.max(31), Validators.min(1)]),
      dateMonth: new FormControl(null, [Validators.max(12), Validators.min(1)]),
      dateYear: new FormControl(null, [Validators.max(2009), Validators.min(1900)])
    });
  }

  onSubmit(){
    let someDate = new Date(this.profileSettingsForm.value.dateYear + '-' + this.profileSettingsForm.value.dateMonth + '-' + this.profileSettingsForm.value.dateDay);
    const model = {
      username: this.profileSettingsForm.value.username,
      email: this.profileSettingsForm.value.email,
      countryId: this.profileSettingsForm.value.country,
      gender: this.profileSettingsForm.value.gender,
      birthday: someDate
    }
    this.userService.updateMember(model);
  }
}
