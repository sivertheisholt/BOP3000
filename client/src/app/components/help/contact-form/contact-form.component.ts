import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { Member } from 'src/app/_models/member.model';
import { NotificationService } from 'src/app/_services/notification.service';
import { SupportService } from 'src/app/_services/support.service';

@Component({
  selector: 'app-contact-form',
  templateUrl: './contact-form.component.html',
  styleUrls: ['./contact-form.component.css']
})
export class ContactFormComponent implements OnInit {
  contactForm!: FormGroup;
  selectSubject : string[] = ['SUBJECTS.ACCOUNT', 'SUBJECTS.SECURITY', 'SUBJECTS.GENERAL', 'SUBJECTS.PAYMENT', 'SUBJECTS.FEEDBACK', 'SUBJECTS.REPORT', 'SUBJECTS.OTHER'];
  submitted: boolean = false;

  user: Member;
  constructor(private route: ActivatedRoute, private supportService: SupportService, private notificationService: NotificationService) {
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
    this.submitted = true;
    if(this.contactForm.valid){
      this.supportService.postTicket(this.contactForm.value).subscribe(
        (res) => {
          this.contactForm.reset();
          this.notificationService.setNewNotification({type: 'success', message: 'Thank you for contacting us! We will try to respond within 48 hours.'})
        }
      )
    }
  }

}
