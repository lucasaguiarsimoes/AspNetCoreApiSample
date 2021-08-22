import { NgModule } from '@angular/core';
import { UsuarioListComponent } from '../../_components/usuario/usuario-list/usuario-list.component';
import { RouterModule, Routes } from '@angular/router';
import { ThemeResolver } from 'src/app/_resolvers/theme.resolver';

const routes: Routes = [
  { path: '', component: UsuarioListComponent, resolve: { theme: ThemeResolver }, },
];

@NgModule({
  imports: [ RouterModule.forChild(routes) ],
  exports: [RouterModule],
})
export class UsuarioRoutingModule { }
