import { RoleService } from './role.service';
import { HttpClient } from '@angular/common/http';
import { Injectable, OnDestroy } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { ClaimTypeEnum } from '../_enums/ClaimTypeEnum';
import { MenuItemViewModel } from '../_models/MenuItemViewModel';
import { AuthenticationService } from './authentication.service';
import { PermissaoSistemaEnum } from '../_enums/PermissaoSistemaEnum';

@Injectable({
  providedIn: 'root'
})
export class MenuService implements OnDestroy {

  /**
   * Itens de menu padrão que não exigem permissões ou autenticação
   */
   private readonly DEFAULT_MENU: MenuItemViewModel[] = [];

   /**
   * Fonte para menu$. Controla as notificações para componentes que usam informações referente
   * ao menu da aplicação.
   */
  private sourceMenuSubject = new BehaviorSubject<MenuItemViewModel[]>(this.DEFAULT_MENU);

  /**
   * Fornece um observable que contém os itens do menu da aplicação
   * Caso as informações do menu sejam atualizados enquanto a aplicação estiver rodando
   * este observable notificará todos seus subscribers.
   */
  get menu$(): Observable<MenuItemViewModel[]> {
    // Retorna um observable do Subject para que toda vez que o Subject for alterado, notifique todos os subscribers
    return this.sourceMenuSubject.asObservable();
  }

  constructor(private roles: RoleService) { }

  /**
   * Caso a instância deste serviço seja destruída, todos os seus subjects
   * serão marcados como completo e não receberam mais notificações. Somente
   * será aceito novas notificações caso seja criada uma nova instância deste service.
   * **Obs: Este service é singleton.**
   */
   ngOnDestroy() {
    this.sourceMenuSubject.complete();
  }

  /**
   * Carrega todos os itens de menu que o usuário tem permissão
   */
  loadMenu() {
    // Monta os itens de menu acessíveis para o usuário
    const menuItens: MenuItemViewModel[] = this.montaMenu();

    // Notifica os componentes que escutam as mudanças do subject para que o menu seja atualizado/notificado
    this.sourceMenuSubject.next(menuItens);
  }

  /**
   * Monta os itens de menu que o usuário tem permissão para acessar
   */
  private montaMenu(): MenuItemViewModel[] {
    // Cria um novo array para inicializar os itens de menu que o usuário pode acessar
    const menuItens: MenuItemViewModel[] = [];

    // Inclui o item de menu apenas se o usuário tiver a permissão específica
    if (this.roles.hasRoles(PermissaoSistemaEnum.UsuarioConsulta)) {
      menuItens.push({name: 'Usuários', route: '/usuario'});
    }

    return menuItens;
  }
}
