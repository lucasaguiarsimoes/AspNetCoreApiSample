import { RoleService } from './../../../_services/role.service';
import { UsuarioGetFilteredListResponseViewModel } from './../../../_models/UsuarioGetFilteredListResponseViewModel';
import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { PermissaoSistemaEnum } from 'src/app/_enums/PermissaoSistemaEnum';

@Component({
  selector: 'app-usuario-table',
  templateUrl: './usuario-table.component.html',
  styleUrls: ['./usuario-table.component.scss']
})
export class UsuarioTableComponent implements OnInit {

  /**
   * Usuários que se deseja exibir no table
   */
  @Input() usuarios: UsuarioGetFilteredListResponseViewModel[];


  /**
   * Evento do click a ser aplicado na célula de ação sobre o usuário do table
   */
  @Output() eventCellClick = new EventEmitter<number>();

  /**
   * Informação se o usuário tem permissão de consulta de usuários
   */
  hasPermissaoConsulta: boolean;

  /**
   * Informação se o usuário tem permissão de edição de usuários
   */
  hasPermissaoEdita: boolean;

  constructor(private roleService: RoleService) { }

  ngOnInit(): void {
    // Verifica as permissões disponíveis
    this.hasPermissaoConsulta = this.roleService.hasRoles(PermissaoSistemaEnum.UsuarioConsulta);
    this.hasPermissaoEdita = this.roleService.hasRoles(PermissaoSistemaEnum.UsuarioEdita);
  }

  /**
   * Método para o evento de click no botão de ação sobre o usuário do table
   */
  cellClick(event: any, usuarioId: number) {
    // Verifica se o parametro event é valido e se houver keypress apenas o Enter (13) é valido
    if (event && event.keyCode && event.keyCode !== 13) {
      return;
    }

    // Emite o evento registrado para o ID do usuário em questão
    this.eventCellClick.emit(usuarioId);
  }

  /**
   * Monta o texto de tooltip da ação de acordo com a permissão do usuário logado
   */
  getRowActionTooltip() {
    // Prioriza a permissão de edição
    if (this.hasPermissaoEdita) {
      return 'Editar';
    }

    // Verifica se ao menos tem permissão de consulta
    if (this.hasPermissaoConsulta) {
      return 'Consultar';
    }

    // Se chegou aqui, não há o que colocar
    return '';
  }

  /**
   * Define o ícone da ação de acordo com a permissão do usuário logado
   */
  getRowActionIconClass() {
    // Prioriza a permissão de edição
    if (this.hasPermissaoEdita) {
      return 'fa fa-pencil';
    }

    // Verifica se ao menos tem permissão de consulta
    if (this.hasPermissaoConsulta) {
      return 'fa fa-book';
    }

    // Se chegou aqui, não há o que colocar
    return '';
  }
}
