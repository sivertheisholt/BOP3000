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
        console.log(element.style);
        if (element.style.maxHeight == '0px' || element.style.maxHeight == '') {
          element.style.maxHeight = '1000px';
        } else {
          element.style.maxHeight = '0px';
        }
    }
}