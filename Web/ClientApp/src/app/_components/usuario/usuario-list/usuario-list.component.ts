import { UsuarioGetFilteredListRequestViewModel } from './../../../_models/UsuarioGetFilteredListRequestViewModel';
import { Component, Input, OnInit } from '@angular/core';
import { UsuarioGetFilteredListResponseViewModel } from 'src/app/_models/UsuarioGetFilteredListResponseViewModel';
import { UsuarioService } from 'src/app/_services/usuario.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { finalize, take } from 'rxjs/operators';
import { PermissaoSistemaEnum } from 'src/app/_enums/PermissaoSistemaEnum';
import { RoleService } from 'src/app/_services/role.service';

@Component({
  selector: 'app-usuario-list',
  templateUrl: './usuario-list.component.html',
  styleUrls: ['./usuario-list.component.scss']
})
export class UsuarioListComponent implements OnInit {

  /**
   * Lista de usuários carregados para exibição na listagem
   */
  usuarios: UsuarioGetFilteredListResponseViewModel[];

  /**
   * Formulário com os inputs
   */
  formFilters: FormGroup;

  /**
   * Exibe ou não a lista de itens de response da busca
   */
  showResponseList = false;

  /**
   * Informação se o usuário tem permissão de Criação de usuários
   */
  hasPermissaoCria: boolean;

  constructor(
    private usuarioService: UsuarioService,
    private fb: FormBuilder,
    private roleService: RoleService) { }

  ngOnInit(): void {
    // Cria formulário dos filtros
    this.formFilters = this.createFormFilters();

    // Verifica as permissões disponíveis
    this.hasPermissaoCria = this.roleService.hasRoles(PermissaoSistemaEnum.UsuarioCria);
  }

  /**
   * Cria formulário dos Filtros
   */
  createFormFilters(): FormGroup {
    return this.fb.group({
      codigoFiltro: ['', Validators.maxLength(50)],
      nomeFiltro: ['', Validators.maxLength(100)],
      emailFiltro: ['', Validators.maxLength(60)],
    });
  }

  /**
   *
   */
  onSubmitFilter() {
    // Garante que o formulário de filtros está válido
    if (!this.formFilters.valid) {
      return;
    }

    // Pega os dados do formulário
    const codigo = this.formFilters.get('codigoFiltro').value;
    const nome = this.formFilters.get('nomeFiltro').value;
    const email = this.formFilters.get('emailFiltro').value;

    // Cria objeto de request
    // Como as variáveis tem o mesmo nome da propriedade do request, não há necessidade de fazer 'codigo: codigo'
    const request = {
      codigo,
      nome,
      email,
    } as UsuarioGetFilteredListRequestViewModel;

    // Limpa lista atual de usuários
    this.usuarios = [];

    this.usuarioService
      // Aciona o método de busca ao backend
      .getFilteredList(request)
      // A partir do observable retornado, registra o que deve ser executado após a conclusão do carregamento através do pipe
      .pipe(
        // Força a executar o subscribe apenas uma vez, e aí desregistra o subscribe
        take(1),
        // Funciona como um 'finally', sempre executado seja em caso de sucesso ou de erro
        finalize(() => {
          // Exibe o resultado da busca
          this.showResponseList = true;
        }))
      // Registra uma 'escuta' (subscribe) para o que deve ser executado após a conclusão do carregamento através do subscribe
      .subscribe(
        // Método a ser executado em caso de request sem erros
        (usuarios: UsuarioGetFilteredListResponseViewModel[]) => {
          // Atualiza a lista de usuários atual
          this.usuarios = usuarios;
        },
        // Método a ser executado se houve erro no request
        (error) => {
          // Insere um log no console com o erro retornado
          console.error(error);
        }
      );
  }

}
