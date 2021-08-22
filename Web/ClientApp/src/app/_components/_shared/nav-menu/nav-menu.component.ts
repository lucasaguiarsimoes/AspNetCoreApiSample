import { MenuItemViewModel } from './../../../_models/MenuItemViewModel';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ClaimTypeEnum } from 'src/app/_enums/ClaimTypeEnum';
import { AuthenticationService } from 'src/app/_services/authentication.service';
import { MenuService } from 'src/app/_services/menu.service';
import { Subject, Subscription } from 'rxjs';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.scss']
})
export class NavMenuComponent implements OnInit, OnDestroy  {

  /**
   * Controla se o menu está espandido no momento ou não
   */
  isMenuExpanded = false;

  /**
   * Itens de menu acessíveis para o usuário em questão
   */
  menuItens: MenuItemViewModel[] = [];

  /**
   * Armazena os subscriptions para aplicar o unsubscribe no momento de destruir este componente
   */
  private subscriptions: Subscription[] = [];

  constructor(
    private authenticationService: AuthenticationService,
    private menuService: MenuService,
    private route: Router
  ) { }

  ngOnInit(): void {
    // Prepara subscribe para que qualquer mudança no menu seja notificada para este método
    const subscription = this.menuService.menu$.subscribe(
      // Método acionado em caso de sucesso na notificação dos itens de menu
      (menuResponse: MenuItemViewModel[]) => {
        // Atualiza menu do usuário logado
        this.menuItens = menuResponse;
      },
      // Método acionado em caso de erro na notificação dos itens de menu
      (erro) => {
        console.error('Falha ao receber os itens de Menu', erro);
      }
    );

    // Insere na lista de subscriptions para fazer o unsubscribe ao destruir o componente
    this.subscriptions.push(subscription);

    // Faz a primeira atualização do menu
    this.menuService.loadMenu();
  }

  ngOnDestroy() {
    // Desfaz os subscribes realizados e ativos neste componente
    this.subscriptions.forEach((sub) => sub.unsubscribe());
  }

  /**
   * Retrai o menu, caso esteja espandido
   */
  collapse() {
    this.isMenuExpanded = false;
  }

  /**
   * Troca o menu expandido pelo contraído ou vice-versa
   */
  toggle() {
    this.isMenuExpanded = !this.isMenuExpanded;
  }

  /**
   * Redireciona a página para a rota do Swagger, fora do Angular App
   */
  redirectSwagger() {
    window.location.href = window.location.origin + "/swagger";
  }

  /**
   * Verifica se existe um usuário autenticado
   */
  isAuthenticated() {
    return this.authenticationService.isAuthenticated();
  }

  /**
   * Pega o nome do usuário logado no momento
   */
  getCurrentUserName() {
    // Pega o nome do usuário a partir do token, caso exista
    return this.authenticationService.getClaimValue(ClaimTypeEnum.Sub);
  }

  /**
   * Acionamento para fazer logout do sistema
   */
  logout() {
    // Remove a autenticação atual
    this.authenticationService.logout();

    // Redireciona para o login do sistema
    this.route.navigate(['/login']);
  }
}
