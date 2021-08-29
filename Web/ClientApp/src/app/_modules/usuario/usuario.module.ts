import { UsuarioRoutingModule } from './usuario-routing.module';
import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { UsuarioComponent } from '../../_components/usuario/usuario/usuario.component';
import { UsuarioListComponent } from '../../_components/usuario/usuario-list/usuario-list.component';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { AppComponent } from 'src/app/app.component';
import { AuthenticationService } from 'src/app/_services/authentication.service';
import { MenuService } from 'src/app/_services/menu.service';
import { RoleService } from 'src/app/_services/role.service';
import { ThemeService } from 'src/app/_services/theme.service';
import { UsuarioService } from 'src/app/_services/usuario.service';
import { UsuarioTableComponent } from '../../_components/usuario/usuario-table/usuario-table.component';
import { UsuarioCardComponent } from '../../_components/usuario/usuario-card/usuario-card.component';
import { UsuarioCardListComponent } from '../../_components/usuario/usuario-card-list/usuario-card-list.component';
import { JwtInterceptor } from 'src/app/_interceptors/jwt.interceptor';
import { ErrorInterceptor } from 'src/app/_interceptors/error.interceptor';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { InvalidInputFeedbackComponent } from 'src/app/_components/_shared/invalid-input-feedback/invalid-input-feedback.component';

@NgModule({
  declarations: [
    UsuarioComponent,
    UsuarioListComponent,
    UsuarioTableComponent,
    UsuarioCardComponent,
    UsuarioCardListComponent,
    InvalidInputFeedbackComponent,
  ],
  imports: [
    CommonModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    UsuarioRoutingModule,
    NgbModule,
  ],
  providers: [
    AuthenticationService,
    ThemeService,
    MenuService,
    UsuarioService,
    RoleService,
    { provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true },
    { provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true },
  ],
  bootstrap: [AppComponent],
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
})
export class UsuarioModule { }
