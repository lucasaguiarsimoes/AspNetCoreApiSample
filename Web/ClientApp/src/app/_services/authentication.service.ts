import { TokenService } from './token.service';
import { isNullOrUndefined } from '../_common/util';
import { ClaimTypeEnum } from 'src/app/_enums/ClaimTypeEnum';
import { JwtHelperService } from '@auth0/angular-jwt';
import { Injectable } from '@angular/core';
import { Observable, BehaviorSubject, of } from 'rxjs';
import { map } from 'rxjs/operators';
import { UserViewModel } from '../_models/UserViewModel';
import { HttpClient } from '@angular/common/http';
import { TokenViewModel } from '../_models/TokenViewModel';
import { LoginViewModel } from '../_models/LoginViewModel';
import { HttpErrorResponseViewModel } from '../_models/HttpErrorResponseViewModel';
import { MenuService } from './menu.service';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root',
})
export class AuthenticationService {

  private jwtHelperService: JwtHelperService;
  private url = '/api/Authentication/';
  private currentUserSubject: BehaviorSubject<UserViewModel>;
  public usuarioLogado: Observable<UserViewModel>;

  constructor(
    private http: HttpClient,
    private route: Router,
    private tokenService: TokenService,
    private menuService: MenuService) {
    // Pega o token do local storage
    const user = tokenService.getToken();
    this.jwtHelperService = new JwtHelperService();

    // Cria o subject do usuário logado atualmente
    // O Subject faz com que toda vez que seu valor for alterado através do 'next',
    // Todos que estiverem escutando através do 'subscribe' serão acionados/disparados novamente
    this.currentUserSubject = new BehaviorSubject<UserViewModel>(user);
    this.usuarioLogado = this.currentUserSubject.asObservable();
  }

  // Aciona o login do usuário de forma criptografada
  login(loginRequest: LoginViewModel) {
    // Aciona o servidor para processar a autenticação
    return this.http.post<TokenViewModel>(this.url + 'Login', loginRequest).pipe(
      map((token) => {
        // Sai se o login não foi bem sucedido através de um token gerado
        if (!token || !token.token) {
          throw new HttpErrorResponseViewModel(null, 'Falha na autenticação.');
        }

        this.setLocalUserStorage(loginRequest.usuario, token);
      })
    );
  }

  // Aciona logout, bastando remover o token do local storage
  logout() {
    // Remove o usuário do local storage
    this.tokenService.removeToken();
    this.currentUserSubject.next(null);
    this.menuService.loadMenu();
  }

  /**
   * Carrega o valor de uma claim específica do usuário logado
   */
  getClaimValue(claimType: ClaimTypeEnum): any | null {
    if (!this.isAuthenticated()) {
      return null;
    }

    // Faz o decode do token e pega o valor do claim solicitado
    const decodedJwtData = this.jwtHelperService.decodeToken(this.currentUserSubject.value.token);

    return !isNullOrUndefined(decodedJwtData[claimType]) ? decodedJwtData[claimType] : null;
  }

  /**
   * Realiza validações padrões no token atual
   */
  isAuthenticated(): boolean {
    const valorUsuarioLogado = this.currentUserSubject.value;

    // Sai se o usuário logado não existir
    if (!valorUsuarioLogado) {
      return false;
    }

    // Sai se o token do usuário já expirou
    if (this.jwtHelperService.isTokenExpired(valorUsuarioLogado.token)) {
      return false;
    }

    return true;
  }

  /**
   * Lida com a rota após a auntenticação
   */
  navigateAfterLogin(menuService: MenuService) {
    // Carrega o menu
    menuService.loadMenu();
    // Em caso de sucesso no login, navega para a rota default
    this.route.navigate(['']);
  }

  /**
   * Seta os dados do usuário na sessão do navegador
   */
  private setLocalUserStorage(usuario: string, token: TokenViewModel) {
    // Armazena o objeto no local storage do browser para que o usuário se mantenha logado mesmo com refreshes das páginas
    const user = this.tokenService.setToken(usuario, token);
    this.currentUserSubject.next(user);
  }
}
