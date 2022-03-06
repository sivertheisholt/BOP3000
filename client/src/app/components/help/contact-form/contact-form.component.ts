import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-contact-form',
  templateUrl: './contact-form.component.html',
  styleUrls: ['./contact-form.component.css']
})
export class ContactFormComponent implements OnInit {
  contactForm!: FormGroup;

  selectSubject : any = [
    {id: 0, subject: 'Account'},
    {id: 1, subject: 'Security'},
    {id: 2, subject: 'General'},
    {id: 3, subject: 'Payment'},
    {id: 4, subject: 'Feedback'},
    {id: 5, subject: 'Report'},
    {id: 6, subject: 'Other'}
  ]
  constructor() { }

  ngOnInit(): void {
    this.contactForm = new FormGroup({
      contactSubject: new FormControl(null, Validators.required),
      contactDescription: new FormControl(null, Validators.required)
    })
  }

  onSubmit(){
    console.log(this.contactForm);
  }

}
