import { Directive, ElementRef, HostListener, OnInit } from "@angular/core";

@Directive({
    selector: '[accordion]'
})
export class AccordionDirective implements OnInit{

    constructor(private el: ElementRef){}

    ngOnInit(): void {
        
    }

    @HostListener('click') toggleAccordion(){
        let element = this.el.nativeElement.nextElementSibling;
        if (element.style.display == 'none' || element.style.display == '') {
          element.style.display = 'block';
        } else {
          element.style.display = 'none';
        }
    }
}