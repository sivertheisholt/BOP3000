import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { faCheckCircle, faEnvelope, faLock, faUser, faStopCircle, faStop } from '@fortawesome/free-solid-svg-icons';
import { fromEvent } from 'rxjs';
import { debounceTime, distinctUntilChanged, filter, map } from 'rxjs/operators';
import { Country } from 'src/app/_models/country.model';
import { UserService } from 'src/app/_services/user.service';
import { CustomValidator } from 'src/app/_validators/custom-validator';
import { passwordMatchingValidator } from 'src/app/_validators/password-matching';
import { AuthService } from '../../_services/auth.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  faEnvelope = faEnvelope; faLock = faLock; faUser = faUser; faCheckCircle = faCheckCircle; faStopCircle = faStopCircle; faStop = faStop;
  regUserForm! : FormGroup;
  countries : Country[] = [];
  genders = ['Male', 'Female', 'Other', 'Hidden'];
  @ViewChild('usernameInput', {static: true}) usernameInput? : ElementRef;
  @ViewChild('emailInput', {static: true}) emailInput? : ElementRef;

  constructor(private authService: AuthService, private route: ActivatedRoute, private router: Router, private userService: UserService) { }

  ngOnInit(): void {
    this.countries = this.route.snapshot.data['countries'];
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
        this.regUserForm.get('username')?.setErrors({'usernameTaken': 'Username is taken.'});
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
          this.regUserForm.get('email')?.setErrors({'emailTaken': 'Email is taken.'});
          return;
        }
        this.emailInput?.nativeElement.classList.remove('loading');
        return;
      }, (err) => {
        this.emailInput?.nativeElement.classList.remove('loading');
      })
    })

    this.regUserForm = new FormGroup({
      email: new FormControl(null, [Validators.required, Validators.email]),
      password: new FormControl(null, Validators.compose([
        Validators.required,
        CustomValidator.patternValidator(/\d/, { hasNumber: true }),
        CustomValidator.patternValidator(/[A-Z]/, { hasCapitalCase: true }),
        CustomValidator.patternValidator(/[a-z]/, { hasSmallCase: true }),
        CustomValidator.patternValidator(/[^A-Za-z0-9]/, { hasSpecialCharacter: true }),
        Validators.minLength(8)
      ])),
      repeatPassword: new FormControl(null, Validators.required),
      username: new FormControl(null, [Validators.required, CustomValidator.patternValidator(/^[A-Za-z0-9 ]+$/, { hasSpecialCharacter: true })]),
      gender: new FormControl(null, Validators.required),
      countryId: new FormControl(null, Validators.required)
    },{
      validators: passwordMatchingValidator
    }
    );
  }

  register() {
    if(this.regUserForm.valid){
      this.authService.register(this.regUserForm.value).subscribe(res => {
        this.router.navigate(['/home']);
    }, err => {
        console.log(err);
      });
    } else {
      this.regUserForm.setErrors({'missingFields': 'Have you filled every field?'});
    }
  }
}
