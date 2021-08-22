import { UsuarioRoutingModule } from './usuario-routing.module';
import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { UsuarioComponent } from '../../_components/usuario/usuario/usuario.component';
import { UsuarioListComponent } from '../../_components/usuario/usuario-list/usuario-list.component';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { AppComponent } from 'src/app/app.component';
import { AuthenticationService } from 'src/app/_services/authentication.service';
import { MenuService } from 'src/app/_services/menu.service';
import { RoleService } from 'src/app/_services/role.service';
import { ThemeService } from 'src/app/_services/theme.service';
import { UsuarioService } from 'src/app/_services/usuario.service';

@NgModule({
  declarations: [
    UsuarioComponent,
    UsuarioListComponent,
  ],
  imports: [
    CommonModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    UsuarioRoutingModule,
  ],
  providers: [
    AuthenticationService,
    ThemeService,
    MenuService,
    UsuarioService,
    RoleService,
  ],
  bootstrap: [AppComponent],
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
})
export class UsuarioModule { }
