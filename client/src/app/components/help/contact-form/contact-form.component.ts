import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { Member } from 'src/app/_models/member.model';

@Component({
  selector: 'app-contact-form',
  templateUrl: './contact-form.component.html',
  styleUrls: ['./contact-form.component.css']
})
export class ContactFormComponent implements OnInit {
  contactForm!: FormGroup;
  selectSubject : string[] = ['Account', 'Security', 'General', 'Payment', 'Feedback', 'Report', 'Other'];

  user: Member;
  constructor(private route: ActivatedRoute) {
    this.user = this.route.snapshot.data['user'];
  }

  ngOnInit(): void {
    this.contactForm = new FormGroup({
      subject: new FormControl(null, Validators.required),
      description: new FormControl(null, Validators.required),
      email: new FormControl(this.user.email),
      name: new FormControl(this.user.username)
    })
  }

  onSubmit(){
    console.log(this.contactForm.value);
  }

}
