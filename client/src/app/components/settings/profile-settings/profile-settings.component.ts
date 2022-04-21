import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { Country } from 'src/app/_models/country.model';
import { CustomImg } from 'src/app/_models/custom-img.model';
import { Member } from 'src/app/_models/member.model';
import { UserSettingsService } from 'src/app/_services/user-settings.service';
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
  user: Member;
  months = ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"];
  days = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31];
  years: number[] = [];
  selectedImgUrl: string = '';
  customizationImages: CustomImg[] = [];

  constructor(private route: ActivatedRoute, private userService: UserService, private userSettingsService: UserSettingsService) {
    this.countries = this.route.snapshot.data['countries'];
    this.user = this.route.snapshot.data['user'];
    
    this.userSettingsService.getCustomizationImages().subscribe(
      (res) => {
        console.log(res);
        this.customizationImages = res;
        this.selectedImgUrl = this.user.memberProfile?.memberCustomization.backgroundUrl!;
      }
    )
  }

  ngOnInit(): void {
    let d = new Date();
    for(let i = 1930; i <= d.getFullYear(); i++){
      this.years.push(i);
    }
    let userBirthday = new Date(this.user.memberProfile?.birthday!);
    this.profileSettingsForm = new FormGroup({
      username: new FormControl(this.user?.username),
      email: new FormControl(this.user?.email),
      description: new FormControl(this.user?.memberProfile?.description),
      gender: new FormControl(this.user?.memberProfile?.gender),
      country: new FormControl(this.user.memberProfile?.countryIso?.id),
      dateDay: new FormControl(userBirthday.getDate(), [Validators.max(31), Validators.min(1)]),
      dateMonth: new FormControl(this.months[userBirthday.getMonth()], [Validators.max(12), Validators.min(1)]),
      dateYear: new FormControl(userBirthday.getFullYear(), [Validators.max(2009), Validators.min(1900)])
    });



  }

  onSubmit(){
    let someDate = new Date(this.profileSettingsForm.value.dateYear + '-' + this.profileSettingsForm.value.dateMonth + '-' + this.profileSettingsForm.value.dateDay);
    const model = {
      username: this.profileSettingsForm.value.username,
      email: this.profileSettingsForm.value.email,
      countryId: this.profileSettingsForm.value.country,
      gender: this.profileSettingsForm.value.gender,
      description: this.profileSettingsForm.value.description,
      birthday: someDate
    }
    this.userService.updateMember(model);
  }

  onChangeAccountBg(url: string){
    this.selectedImgUrl = url;
    this.userSettingsService.postChangeAccountBackground(url).subscribe(
      (res) => {
        console.log(res);
      }
    )
    
  }
}
