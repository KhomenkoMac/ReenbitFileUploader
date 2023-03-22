import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { ReactiveFormsModule } from '@angular/forms';

import { AppComponent } from './app.component';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { API_BASE_URL, Client } from './api.service';
import { CommonModule } from '@angular/common';
import { environment } from '../environments/environment';


@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    BrowserModule, 
    HttpClientModule, 
    NgbModule,
    ReactiveFormsModule,
    CommonModule
  ],
  providers: [
    {provide: API_BASE_URL, useValue: environment.baseUrl},
    Client
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
