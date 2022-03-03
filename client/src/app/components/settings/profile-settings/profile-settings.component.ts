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
  countries : Country[];
  genders = ['Male', 'Female', 'Other', 'Hidden'];
  user: UserProfile;

  constructor(private route: ActivatedRoute, private userService: UserService) {
    this.countries = this.route.snapshot.data['countries'];
    this.user = this.route.snapshot.data['user'];
   }

  ngOnInit(): void {
    let userBirthday = new Date(this.user.memberProfile?.birthday!);
    console.log(userBirthday);
    this.profileSettingsForm = new FormGroup({
      username: new FormControl(this.user?.username),
      email: new FormControl(this.user?.email),
      description: new FormControl(this.user?.memberProfile?.memberData?.userDescription),
      gender: new FormControl(this.user?.memberProfile?.gender),
      country: new FormControl(this.user.memberProfile?.countryIso?.id),
      dateDay: new FormControl(userBirthday.getDay(), [Validators.max(31), Validators.min(1)]),
      dateMonth: new FormControl(userBirthday.getMonth(), [Validators.max(12), Validators.min(1)]),
      dateYear: new FormControl(userBirthday.getFullYear(), [Validators.max(2009), Validators.min(1900)])
    });



  }

  onSubmit(){
    let someDate = new Date(this.profileSettingsForm.value.dateYear + '-' + this.profileSettingsForm.value.dateMonth + '-' + this.profileSettingsForm.value.dateDay);
    console.log(someDate);
    const model = {
      username: this.profileSettingsForm.value.username,
      email: this.profileSettingsForm.value.email,
      countryId: this.profileSettingsForm.value.country,
      gender: this.profileSettingsForm.value.gender,
      description: this.profileSettingsForm.value.description,
      birthday: someDate
    }
    console.log(model);
    this.userService.updateMember(model);
  }
}
