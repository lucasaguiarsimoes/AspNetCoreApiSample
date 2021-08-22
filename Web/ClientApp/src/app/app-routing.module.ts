import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule, Routes } from '@angular/router';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './_components/_shared/nav-menu/nav-menu.component';
import { HomeComponent } from './_components/home/home.component';
import { LoginComponent } from './_components/login/login.component';
import { UnauthorizedComponent } from './_components/unauthorized/unauthorized.component';
import { AuthGuard } from './_guards/auth.guard';
import { PermissaoSistemaEnum } from './_enums/PermissaoSistemaEnum';
import { ThemeResolver } from './_resolvers/theme.resolver';

const routes: Routes = [

  { path: 'login', component: LoginComponent, resolve: { theme: ThemeResolver, }, },
  { path: '', component: HomeComponent, canActivate: [AuthGuard], resolve: { theme: ThemeResolver }, },
  { path: 'unauthorized', component: UnauthorizedComponent, canActivate: [AuthGuard], resolve: { theme: ThemeResolver }, },
  // { path: 'redefinicaosenha', component: UsuarioResetPasswordComponent },
  { path: 'usuario', loadChildren: () => import('./_modules/usuario/usuario.module').then((m) => m.UsuarioModule), canActivate: [AuthGuard],
  data: { roles: [PermissaoSistemaEnum.UsuarioConsulta] }, },
  { path: '**', redirectTo: '' },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  providers: [AuthGuard],
  exports: [RouterModule],
})
export class AppRoutingModule {}
