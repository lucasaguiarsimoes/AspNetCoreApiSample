import { Injectable } from '@angular/core';
import { isNullOrEmpty } from '../_common/util';
import { TokenViewModel } from '../_models/TokenViewModel';
import { UserViewModel } from '../_models/UserViewModel';

@Injectable({
  providedIn: 'root'
})
export class TokenService {

  /**
   * Chave fixa para o identificador do local storage que armazena o token
   */
  private localStorageID = 'sampleapi_token';

  constructor() { }

  /**
   * Retorna o token do local storage, se existir
   */
  getToken(): UserViewModel {
    // Pega o token atual do storage
    const currentToken = localStorage.getItem(this.localStorageID);

    // Se encontrou um token válido, deserializa o objeto esperando um json
    return isNullOrEmpty(currentToken) ? null : JSON.parse(currentToken);
  }

  /**
   * Seta os dados do usuário na sessão do navegador
   */
  setToken(usuario: string, token: TokenViewModel): UserViewModel {
    // Prepara o objeto do usuário autenticado
    const user = new UserViewModel();

    // Monta o objeto com os detalhes do usuário e o token jwt
    user.usuario = usuario;
    user.token = token.token;

    // Armazena o objeto no local storage do browser para que o usuário se mantenha logado mesmo com refreshes das páginas
    localStorage.setItem(this.localStorageID, JSON.stringify(user));
    return user;
  }

  /**
   * Remove o token atual do local storage
   */
  removeToken() {
    localStorage.removeItem(this.localStorageID);
  }
}
