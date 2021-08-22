import { AppRoutingModule } from './app-routing.module';
import { BrowserModule } from '@angular/platform-browser';
import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { AppComponent } from './app.component';
import { HomeComponent } from './_components/home/home.component';
import { LoginComponent } from './_components/login/login.component';
import { UnauthorizedComponent } from './_components/unauthorized/unauthorized.component';
import { InvalidInputFeedbackComponent } from './_components/_shared/invalid-input-feedback/invalid-input-feedback.component';
import { NavMenuComponent } from './_components/_shared/nav-menu/nav-menu.component';
import { AuthenticationService } from './_services/authentication.service';
import { MenuService } from './_services/menu.service';
import { RoleService } from './_services/role.service';
import { ThemeService } from './_services/theme.service';
import { TokenService } from './_services/token.service';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    LoginComponent,
    UnauthorizedComponent,
    InvalidInputFeedbackComponent,
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    AppRoutingModule,
    ReactiveFormsModule,
    NgbModule,
  ],
  providers: [
    AuthenticationService,
    ThemeService,
    MenuService,
    TokenService,
    RoleService,
  ],
  bootstrap: [AppComponent],
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
})
export class AppModule { }
