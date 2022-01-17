import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { FormsModule } from '@angular/forms';

@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    HttpClientModule,
    FontAwesomeModule,
    FormsModule,
  ],
  exports: [
    HttpClientModule,
    FontAwesomeModule,
    FormsModule,
  ]
})
export class SharedModule { }