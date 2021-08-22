import { RoleService } from './../_services/role.service';
import { CanActivate, Router, ActivatedRouteSnapshot, RouterStateSnapshot, CanActivateChild } from '@angular/router';
import { AuthenticationService } from '../_services/authentication.service';
import { Injectable, isDevMode } from '@angular/core';
import { ClaimTypeEnum } from '../_enums/ClaimTypeEnum';
import { PermissaoSistemaEnum } from '../_enums/PermissaoSistemaEnum';
import { isNullOrUndefined } from '../_common/util';
import { map, take } from 'rxjs/operators';

@Injectable({ providedIn: 'root' })
export class AuthGuard implements CanActivate, CanActivateChild {
  constructor(
    private router: Router,
    private authenticationService: AuthenticationService,
    private roleService: RoleService
  ) {}

  async canActivateChild(childRoute: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
    return await this.canActivate(childRoute, state);
  }

  async canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
    return await this.isRouteValid(route, state);
  }

  /**
   * Valida o acesso à rota
   */
  private isRouteValid(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
    return this.isTokenValid(route, state);
  }

  /**
   * Realiza validação do token
   */
  private isTokenValid(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
    // Verifica se o token jwt expirou
    const isAuthenticated = this.authenticationService.isAuthenticated();

    // Se ainda não expirou, verifica os roles para as permissões
    if (isAuthenticated) {
      if (!this.hasRouteRoles(route.data.roles)) {
        // Se o usuário não possui ao menos uma das roles necessárias, direciona para a tela de não autorizado
        this.router.navigate(['unauthorized']);
        return false;
      }

      // Possui acesso à rota
      return true;
    }

    const loginQueryParams: any = {};

    // Se o usuário estava em uma página válida, envia a URL da página em que ele estava para que possa voltar ao fazer login
    if (state.url !== '/' && state.url.toLowerCase() !== '/unauthorized') {
      loginQueryParams.redirecionado = true;
      loginQueryParams.returnUrl = state.url;
    }

    // Se o sistema não está pronto para uso, descarta token existente
    this.authenticationService.logout();

    // Se chegou aqui, não está logado ou o token expirou. Então redireciona para a página de login
    this.router.navigate(['login'], { queryParams: loginQueryParams });
    return false;
  }

  /** Valida se o usuário possui as roles necessárias para acessar a rota */
  private hasRouteRoles(roles: PermissaoSistemaEnum | PermissaoSistemaEnum[]): boolean {
    // Se não exigir roles, essa verificação deve sempre retornar que vai permitir acesso
    if (isNullOrUndefined(roles)) {
      return true;
    }

    // Verifica se a rota possui alguma restrição de role
    return this.roleService.hasRoles(...[roles].flatMap((x) => x));
  }
}
