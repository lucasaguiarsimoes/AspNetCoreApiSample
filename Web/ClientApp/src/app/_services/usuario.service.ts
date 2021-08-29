import { UsuarioGetFilteredListResponseViewModel } from './../_models/UsuarioGetFilteredListResponseViewModel';
import { UsuarioGetFilteredListRequestViewModel } from './../_models/UsuarioGetFilteredListRequestViewModel';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { UsuarioResponseViewModel } from '../_models/UsuarioResponseViewModel';
import { UsuarioAddViewModel } from '../_models/UsuarioAddViewModel';
import { UsuarioEditViewModel } from '../_models/UsuarioEditViewModel';
import { UsuarioRemoveViewModel } from '../_models/UsuarioRemoveViewModel';

@Injectable({
  providedIn: 'root'
})
export class UsuarioService {

  /**
   * Caminho relativo para acesso aos métodos de usuário da API
   */
  urn = 'api/Usuario';

  constructor(private http: HttpClient) {}

  /**
   * Faz uma busca para carregar uma lista de usuários a partir de filtros definidos no request
   */
  getFilteredList(request: UsuarioGetFilteredListRequestViewModel): Observable<UsuarioGetFilteredListResponseViewModel[]> {
    return this.http.post<UsuarioGetFilteredListResponseViewModel[]>(`${this.urn}/GetFilteredList`, request);
  }

  /**
   * Faz a busca de um usuário através do seu ID
   */
  getById(request: number): Observable<UsuarioResponseViewModel> {
    return this.http.get<UsuarioResponseViewModel>(`${this.urn}/GetById/${request}`);
  }

  /**
   * Solicita a criação de um novo usuário
   */
  create(request: UsuarioAddViewModel): Observable<number> {
    return this.http.post<number>(`${this.urn}/Create`, request);
  }

  /**
   * Solicita a edição de um usuário existente
   */
  edit(request: UsuarioEditViewModel): Observable<boolean> {
    return this.http.put<boolean>(`${this.urn}/Edit`, request);
  }

  /**
   * Solicita a exclusão de um usuário existente
   */
   remove(request: UsuarioRemoveViewModel): Observable<boolean> {
    return this.http.post<boolean>(`${this.urn}/Remove`, request);
  }
}
