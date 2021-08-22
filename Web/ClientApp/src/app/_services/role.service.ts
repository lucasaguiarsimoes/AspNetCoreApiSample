import { Injectable } from '@angular/core';
import { JwtHelperService } from '@auth0/angular-jwt';
import { ClaimTypeEnum } from '../_enums/ClaimTypeEnum';
import { PermissaoSistemaEnum } from '../_enums/PermissaoSistemaEnum';
import { AuthenticationService } from './authentication.service';
import { TokenService } from './token.service';

@Injectable({
  providedIn: 'root'
})
export class RoleService {

  constructor(private tokenService: TokenService) { }

  /**
   * Verifica se o usuário atual possui pelo menos uma das roles informadas.
   * Não leva em consideração se é do tipo Geral, UP/Área, UP ou Área.
   *
   * @usageNotes Pode ser usada para verifica se o usuário possui no
   * mínimo a role para uma UP/Área, UP ou Área.
   */
   hasRoles(...roles: PermissaoSistemaEnum[]): boolean {
    // Pega o usuário logado no sistema
    const currentToken = this.tokenService.getToken();
    const jwtHelper = new JwtHelperService();

    // Sai se o usuário logado não existir
    if (!currentToken) {
      return false;
    }

    // Sai se o token do usuário já expirou
    if (jwtHelper.isTokenExpired(currentToken.token)) {
      return false;
    }

    // Se nenhum role foi requisitado, sai com sucesso
    if (!roles || roles.length === 0) {
      return true;
    }
    // Faz o decode do token e pega os roles existentes nele
    const decodedJwtData = jwtHelper.decodeToken(currentToken.token);
    const rolesJwt: string = decodedJwtData[ClaimTypeEnum.Role];

    // Sai se não houverem roles
    if (!rolesJwt) {
      return false;
    }

    // Retorna sucesso se algum dos roles representados é encontrado (condição OR)
    if (roles.some((val: PermissaoSistemaEnum) => rolesJwt.includes(val.toString()))) {
      return true;
    }

    return false;
  }
}
