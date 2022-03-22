import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { SpinnerService } from 'src/app/_services/spinner.service';

@Component({
  selector: 'app-spinner',
  templateUrl: './spinner.component.html',
  styleUrls: ['./spinner.component.css']
})
export class SpinnerComponent implements OnInit {

  showSpinner = false;

  constructor(private spinnerService: SpinnerService, private cdRef: ChangeDetectorRef) {
    
   }

  ngOnInit(): void {
    this.init();
  }

  init(){
    this.spinnerService.getSpinnerObserver().subscribe(
      (status) => {
        this.showSpinner = status === true;
        this.cdRef.detectChanges();
      }
    )
  }

}
