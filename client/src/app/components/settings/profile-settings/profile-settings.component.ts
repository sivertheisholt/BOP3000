import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { fromEvent } from 'rxjs';
import { debounceTime, distinctUntilChanged, filter, map } from 'rxjs/operators';
import { Country } from 'src/app/_models/country.model';
import { CustomImg } from 'src/app/_models/custom-img.model';
import { Member } from 'src/app/_models/member.model';
import { NotificationService } from 'src/app/_services/notification.service';
import { UserSettingsService } from 'src/app/_services/user-settings.service';
import { UserService } from 'src/app/_services/user.service';
import { CustomValidator } from 'src/app/_validators/custom-validator';

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
  @ViewChild('emailInput', {static: true}) emailInput? : ElementRef;
  @ViewChild('usernameInput', {static: true}) usernameInput? : ElementRef;

  constructor(private route: ActivatedRoute, private userService: UserService, private userSettingsService: UserSettingsService, private notificationService: NotificationService) {
    this.countries = this.route.snapshot.data['countries'];
    this.user = this.route.snapshot.data['user'];
    
    this.userSettingsService.getCustomizationImages().subscribe(
      (res) => {
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
      username: new FormControl(this.user?.username, [CustomValidator.patternValidator(/^[A-Za-z0-9 ]+$/, { hasSpecialCharacter: true }), Validators.required]),
      email: new FormControl(this.user?.email, [Validators.email, Validators.required]),
      description: new FormControl(this.user?.memberProfile?.description),
      gender: new FormControl(this.user?.memberProfile?.gender, Validators.required),
      country: new FormControl(this.user.memberProfile?.countryIso?.id, Validators.required),
      dateDay: new FormControl(userBirthday.getDate()),
      dateMonth: new FormControl(this.months[userBirthday.getMonth()]),
      dateYear: new FormControl(userBirthday.getFullYear())
    });

    fromEvent(this.usernameInput?.nativeElement, 'keyup').pipe(
      map((event: any) => {
        if(event.target.value.length > 3){
          this.usernameInput?.nativeElement.classList.add('loading');
          return event.target.value;
        } else{
          this.usernameInput?.nativeElement.classList.remove('loading');
        }
      })
      ,filter(res => res != undefined)
      ,debounceTime(1000)
      ,distinctUntilChanged()
    ).subscribe((input: string) => {
      this.userService.searchUser(input).subscribe((response) => {
        if(response.length == 0){
          this.usernameInput?.nativeElement.classList.remove('loading');
          return;
        }
        if(response[0].userName.toLowerCase() != input.toLowerCase()){
          this.usernameInput?.nativeElement.classList.remove('loading');
          return;
        }
        this.profileSettingsForm.get('username')?.setErrors({'usernameTaken': 'Username is taken.'});
        this.usernameInput?.nativeElement.classList.remove('loading');
        return;
      }, (err) => {
        this.usernameInput?.nativeElement.classList.remove('loading');
      })
    })

    fromEvent(this.emailInput?.nativeElement, 'keyup').pipe(
      map((event: any) => {
        if(event.target.value.length > 3){
          this.emailInput?.nativeElement.classList.add('loading');
          return event.target.value;
        } else{
          this.emailInput?.nativeElement.classList.remove('loading');
        }
      })
      ,filter(res => res != undefined)
      ,debounceTime(1000)
      ,distinctUntilChanged()
    ).subscribe((input: string) => {
      this.userService.searchEmailExists(input).subscribe((response) => {
        if(response){
          this.emailInput?.nativeElement.classList.remove('loading');
          this.profileSettingsForm.get('email')?.setErrors({'emailTaken': 'Email is taken.'});
          return;
        }
        this.emailInput?.nativeElement.classList.remove('loading');
        return;
      }, (err) => {
        this.emailInput?.nativeElement.classList.remove('loading');
      })
    })


  }

  onSubmit(){
    if(this.profileSettingsForm.valid){
      let newDate = new Date(this.profileSettingsForm.value.dateYear + '-' + this.profileSettingsForm.value.dateMonth + '-' + this.profileSettingsForm.value.dateDay);
      const model = {
        username: this.profileSettingsForm.value.username,
        email: this.profileSettingsForm.value.email,
        countryId: this.profileSettingsForm.value.country,
        gender: this.profileSettingsForm.value.gender,
        description: this.profileSettingsForm.value.description,
        birthday: newDate
      }
      if(model.description == null){
        model.description = "";
      }
      this.userService.updateMember(model).subscribe(
        (res) => {
          this.notificationService.setNewNotification({message: 'Successfully updated settings', type: 'success'})
        }, error => {
          this.notificationService.setNewNotification({message: 'Something went wrong', type: 'error'})
        }
      );
    }
  }

  onChangeAccountBg(url: string){
    this.selectedImgUrl = url;
    this.userSettingsService.postChangeAccountBackground(url).subscribe(
      (res) => {
        this.notificationService.setNewNotification({message: 'Successfully changed account background', type: 'success'})
      }
    )
    
  }
}
