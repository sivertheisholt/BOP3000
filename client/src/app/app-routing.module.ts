import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AccountComponent } from './components/account/account.component';
import { CreateLobbyComponent } from './components/create-lobby/create-lobby.component';
import { NotFoundComponent } from './components/errors/not-found/not-found.component';
import { ServerErrorComponent } from './components/errors/server-error/server-error.component';
import { TestErrorsComponent } from './components/errors/test-errors/test-errors.component';
import { HomeComponent } from './components/home/home.component';
import { RegisterComponent } from './components/register/register.component';
import { StartComponent } from './components/start/start.component';
import { UnAuthGuard } from './_guards/unAuth.guard';
import { AuthGuard } from './_guards/auth.guard';
import { LobbyComponent } from './components/lobby/lobby.component';
import { FindLobbyComponent } from './components/find-lobby/find-lobby.component';
import { GameLobbyComponent } from './components/find-lobby/game-lobby/game-lobby.component';
import { LobbiesResolver } from './_resolvers/lobbies-resolver.service';
import { SettingsComponent } from './components/settings/settings.component';
import { ForgottenPasswordComponent } from './components/forgotten-password/forgotten-password.component';
import { GamesResolver } from './_resolvers/games-resolver.service';
import { CountryResolver } from './_resolvers/country-resolver.service';

const routes: Routes = [
  { path: '', component: StartComponent, canActivate: [AuthGuard] },
  { path: 'register', component: RegisterComponent, canActivate: [AuthGuard] },
  { path: 'home', component: HomeComponent, canActivate: [UnAuthGuard] },
  { path: 'forgotten-password', component: ForgottenPasswordComponent, canActivate: [AuthGuard] },
  { path: 'create-lobby', component: CreateLobbyComponent, canActivate: [UnAuthGuard] },
  { path: 'account', component: AccountComponent, canActivate: [UnAuthGuard] },
  { path: 'lobby', component: LobbyComponent, canActivate:[UnAuthGuard] },
  { path: 'find-lobby', component: FindLobbyComponent, canActivate: [UnAuthGuard], resolve: {games: GamesResolver}},
  { path: 'game-lobby/:id', component: GameLobbyComponent, canActivate: [UnAuthGuard], resolve: {posts: LobbiesResolver}},
  { path: 'settings', component: SettingsComponent, canActivate: [UnAuthGuard], resolve: {countries: CountryResolver} },
  { path: 'errors', component: TestErrorsComponent },
  { path: 'not-found', component: NotFoundComponent },
  { path: 'server-error', component: ServerErrorComponent },
  { path: '**', component: NotFoundComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }