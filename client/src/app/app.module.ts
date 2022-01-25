import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { SharedModule } from './_modules/shared.module';
import { TestErrorsComponent } from './components/errors/test-errors/test-errors.component';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { ErrorInterceptor } from './_interceptors/error.interceptor';
import { ServerErrorComponent } from './components/errors/server-error/server-error.component';
import { NavbarComponent } from './components/navbar/navbar.component';
import { RegisterComponent } from './components/register/register.component';
import { HomeComponent } from './components/home/home.component';
import { StartComponent } from './components/start/start.component';
import { SidebarComponent } from './components/sidebar/sidebar.component';
import { NotFoundComponent } from './components/errors/not-found/not-found.component';
import { ActivityListComponent } from './components/activity-list/activity-list.component';
import { HomeButtonsSectionComponent } from './components/home/home-buttons-section/home-buttons-section.component';
import { FooterComponent } from './components/footer/footer.component';
import { LoginComponent } from './components/login/login.component';
import { CreateLobbyComponent } from './components/create-lobby/create-lobby.component';
import { NgSelectModule } from '@ng-select/ng-select';


@NgModule({
  declarations: [
    AppComponent,
    NavbarComponent,
    RegisterComponent,
    HomeComponent,
    StartComponent,
    SidebarComponent,
    TestErrorsComponent,
    ServerErrorComponent,
    NotFoundComponent,
    ActivityListComponent,
    HomeButtonsSectionComponent,
    FooterComponent,
    LoginComponent,
    CreateLobbyComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    SharedModule,
    NgSelectModule
  ],
  providers: [
    {provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true}
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }