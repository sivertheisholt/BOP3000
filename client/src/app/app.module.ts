import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { SharedModule } from './_modules/shared.module';
import { TestErrorsComponent } from './components/errors/test-errors/test-errors.component';
import { HttpClient, HTTP_INTERCEPTORS } from '@angular/common/http';
import { ErrorInterceptor } from './_interceptors/error.interceptor';
import { ServerErrorComponent } from './components/errors/server-error/server-error.component';
import { NavbarComponent } from './components/navbar/navbar.component';
import { RegisterComponent } from './components/register/register.component';
import { HomeComponent } from './components/home/home.component';
import { StartComponent } from './components/start/start.component';
import { SidebarComponent } from './components/sidebar/sidebar.component';
import { NotFoundComponent } from './components/errors/not-found/not-found.component';
import { HomeButtonsSectionComponent } from './components/home/home-buttons-section/home-buttons-section.component';
import { FooterComponent } from './components/footer/footer.component';
import { LoginComponent } from './components/login/login.component';
import { CreateLobbyComponent } from './components/create-lobby/create-lobby.component';
import { NgSelectModule } from '@ng-select/ng-select';
import { AccountComponent } from './components/account/account.component';
import { ReactiveFormsModule } from '@angular/forms';
import { FindLobbyComponent } from './components/find-lobby/find-lobby.component';
import { RoomcardComponent } from './components/find-lobby/game-lobby/roomcard/roomcard.component';
import { LobbyComponent } from './components/lobby/lobby.component';
import { GameLobbyComponent } from './components/find-lobby/game-lobby/game-lobby.component';
import { GamecardComponent } from './components/find-lobby/gamecard/gamecard.component';
import { AuthInterceptorService } from './_interceptors/auth-interceptor.service';
import { SettingsComponent } from './components/settings/settings.component';
import { LobbyInfoComponent } from './components/lobby/lobby-info/lobby-info.component';
import { WaitingRoomComponent } from './components/lobby/waiting-room/waiting-room.component';
import { JoinedUsersComponent } from './components/lobby/joined-users/joined-users.component';
import { ChatComponent } from './components/lobby/chat/chat.component';
import { ChangePasswordComponent } from './components/settings/change-password/change-password.component';
import { ProfileSettingsComponent } from './components/settings/profile-settings/profile-settings.component';
import { ConnectionsComponent } from './components/settings/connections/connections.component';
import { ForgottenPasswordComponent } from './components/forgotten-password/forgotten-password.component';
import { ManageAccountComponent } from './components/settings/manage-account/manage-account.component';
import { HelpComponent } from './components/help/help.component';
import { AccordionDirective } from './_directives/accordion.directive';
import { ContactFormComponent } from './components/contact-form/contact-form.component';
import { HostPanelComponent } from './components/lobby/host-panel/host-panel.component';
import { ArchivedLobbyComponent } from './components/archived-lobby/archived-lobby.component';
import { ArchivedLobbyPartyComponent } from './components/archived-lobby/archived-lobby-party/archived-lobby-party.component';
import { ArchivedLobbyInfoComponent } from './components/archived-lobby/archived-lobby-info/archived-lobby-info.component';
import { AccountInfoComponent } from './components/account/account-info/account-info.component';
import { AccountCardComponent } from './components/account/account-card/account-card.component';
import { AccountLobbyHistoryComponent } from './components/account/account-lobby-history/account-lobby-history.component';
import { UserPanelComponent } from './components/lobby/user-panel/user-panel.component';
import { UpgradeAccountComponent } from './components/upgrade-account/upgrade-account.component';
import { ActivityLogComponent } from './components/activity-log/activity-log.component';
import { JoinVoiceComponent } from './components/lobby/join-voice/join-voice.component';
import { FaqComponent } from './components/help/faq/faq.component';
import { AboutComponent } from './components/about/about.component';
import { NotifierModule } from 'angular-notifier';
import { TranslateLoader, TranslateModule } from '@ngx-translate/core';
import { TranslateHttpLoader } from '@ngx-translate/http-loader';
import { InfiniteScrollModule } from 'ngx-infinite-scroll';
import { QuickJoinComponent } from './components/home/quick-join/quick-join.component';
import { TermsAgreementComponent } from './components/terms-agreement/terms-agreement.component';

export function httpTranslateLoaderFactory(http: HttpClient){
  return new TranslateHttpLoader(http);
}

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
    HomeButtonsSectionComponent,
    FooterComponent,
    LoginComponent,
    CreateLobbyComponent,
    AccountComponent,
    FindLobbyComponent,
    RoomcardComponent,
    LobbyComponent,
    GamecardComponent,
    GameLobbyComponent,
    SettingsComponent,
    LobbyInfoComponent,
    WaitingRoomComponent,
    JoinedUsersComponent,
    ChatComponent,
    ChangePasswordComponent,
    ProfileSettingsComponent,
    ConnectionsComponent,
    ForgottenPasswordComponent,
    ManageAccountComponent,
    HelpComponent,
    AccordionDirective,
    ContactFormComponent,
    HostPanelComponent,
    ArchivedLobbyComponent,
    ArchivedLobbyPartyComponent,
    ArchivedLobbyInfoComponent,
    AccountInfoComponent,
    AccountCardComponent,
    AccountLobbyHistoryComponent,
    UserPanelComponent,
    UpgradeAccountComponent,
    ActivityLogComponent,
    JoinVoiceComponent,
    FaqComponent,
    AboutComponent,
    QuickJoinComponent,
    TermsAgreementComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    SharedModule,
    NgSelectModule,
    ReactiveFormsModule,
    NotifierModule.withConfig({
      position: {
        horizontal: {
          position: 'right',
          distance: 12
        },
        vertical: {
          position: 'bottom',
          distance: 12,
          gap: 10
        }
      },
      theme: 'material'
    }),
    
    TranslateModule.forRoot({
      defaultLanguage: 'en',
      loader: {
        provide: TranslateLoader,
        useFactory: httpTranslateLoaderFactory,
        deps: [HttpClient]
      }
    }),
    InfiniteScrollModule
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true },
    { provide: HTTP_INTERCEPTORS, useClass: AuthInterceptorService, multi: true }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }