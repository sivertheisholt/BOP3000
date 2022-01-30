import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AccountComponent } from './components/account/account.component';
import { CreateLobbyComponent } from './components/create-lobby/create-lobby.component';
import { NotFoundComponent } from './components/errors/not-found/not-found.component';
import { ServerErrorComponent } from './components/errors/server-error/server-error.component';
import { TestErrorsComponent } from './components/errors/test-errors/test-errors.component';
import { HomeComponent } from './components/home/home.component';
import { RegisterComponent } from './components/register/register.component';
import { RoomSliderComponent } from './components/room/room-slider/room-slider.component';
import { RoomcardComponent } from './components/room/room-slider/roomcard/roomcard.component';
import { StartComponent } from './components/start/start.component';

const routes: Routes = [
  { path: '', component: StartComponent },
  { path: 'register', component: RegisterComponent },
  { path: 'home', component: HomeComponent },
  { path: 'errors', component: TestErrorsComponent },
  { path: 'not-found', component: NotFoundComponent },
  { path: 'server-error', component: ServerErrorComponent },
  { path: 'create-lobby', component: CreateLobbyComponent },
  { path: 'account', component: AccountComponent },
  { path: 'roomcard', component: RoomcardComponent},
  { path: 'room-slider', component: RoomSliderComponent },
  { path: '**', component: NotFoundComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }