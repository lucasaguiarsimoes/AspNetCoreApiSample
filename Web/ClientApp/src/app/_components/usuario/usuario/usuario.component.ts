import { PermissaoSistemaItem } from './../../../_models/PermissaoSistemaItem';
import { isNullOrUndefined } from 'src/app/_common/util';
import { UsuarioAddViewModel } from './../../../_models/UsuarioAddViewModel';
import { RoleService } from './../../../_services/role.service';
import { Component, ElementRef, EventEmitter, Input, OnInit, Output, ViewChild } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { PageModeEnum } from 'src/app/_enums/PageModeEnum';
import { ModalService } from 'src/app/_services/modal.service';
import { UsuarioService } from 'src/app/_services/usuario.service';
import { PermissaoSistemaEnum, PermissaoSistemaEnumLabel } from 'src/app/_enums/PermissaoSistemaEnum';
import { take, finalize, map } from 'rxjs/operators';
import { UsuarioResponseViewModel } from 'src/app/_models/UsuarioResponseViewModel';
import { UsuarioEditViewModel } from 'src/app/_models/UsuarioEditViewModel';

@Component({
  selector: 'app-usuario',
  templateUrl: './usuario.component.html',
  styleUrls: ['./usuario.component.scss']
})
export class UsuarioComponent implements OnInit {

  /**
   * Evento para notificação do termino de execução do Salvar
   */
  @Output() afterSave = new EventEmitter<any>();

  /**
   * Evento para notificação do acionamento do Cancelar
   */
  @Output() afterCancel = new EventEmitter<any>();

  /**
   * Evento para notificação de erros
   */
  @Output() afterError = new EventEmitter<any>();

  /**
   * ID do usuario carregado no modal
   */
  usuarioId: number | null;

  /**
   * Possibilita o uso do enum pelo html
   */
  permissoesSistema = this.getPermissoesItens();

  /**
   * Possibilita o uso do enum pelo html
   */
  pageModeEnum = PageModeEnum;

  /**
   * Modo de abertura do modal
   */
  pageMode: PageModeEnum;

  /**
   * Formulário para inputs de dados de usuário
   */
  formUsuario: FormGroup;

  /**
   * Referência do modal principal
   */
  private modalRef: NgbModalRef;

  /**
   * Corpo que va ser utilizado no modal
   */
  @ViewChild('modalBody') modalBody: ElementRef;

  /**
   * Rodapé que vai ser utilizado no modal
   */
  @ViewChild('modalFooter') modalFooter: ElementRef;

  /**
   * Referência do usuário carregado no início da edição do usuário no modal
   */
  usuarioCarregado: UsuarioResponseViewModel;

  /**
   * Informação se o usuário tem permissão de consulta de usuários
   */
  hasPermissaoConsulta: boolean;

   /**
    * Informação se o usuário tem permissão de Criação de usuários
    */
  hasPermissaoCria: boolean;

   /**
    * Informação se o usuário tem permissão de edição de usuários
    */
  hasPermissaoEdita: boolean;

  constructor(
    private fb: FormBuilder,
    private usuarioService: UsuarioService,
    private roleService: RoleService,
    private modalService: ModalService) { }

  ngOnInit(): void {
    // Verifica as permissões disponíveis
    this.hasPermissaoConsulta = this.roleService.hasRoles(PermissaoSistemaEnum.UsuarioConsulta);
    this.hasPermissaoCria = this.roleService.hasRoles(PermissaoSistemaEnum.UsuarioCria);
    this.hasPermissaoEdita = this.roleService.hasRoles(PermissaoSistemaEnum.UsuarioEdita);
  }

  /**
   * Pega todas as permissões existentes no sistema de forma tipada
   */
  private getPermissoesItens(): PermissaoSistemaItem[] {
    return Array
      // Transforma o map de enum/label para array
      .from(PermissaoSistemaEnumLabel)
      // Mapeia cada item para um objeto tipado de permissão com descrição
      .map(([value, description]) => {
        // Monta um objeto novo para cada item
        return <PermissaoSistemaItem> {
          value,
          description
        }
      });
  }

  /**
   * Define um nome de formControl para cada item de permissão
   */
  getPermissionFormControl(permission: PermissaoSistemaItem) {
    return 'permissao' + permission.value;
  }

  /**
   * Cria formGroup com controles de dados de Usuário
   */
  createFormUsuario() {
    // Cria dinamicamente o array de validators do campo de senha
    const validatorsSenha = [ Validators.maxLength(20) ];

    // A senha deve ser obrigatória caso um novo usuário esteja sendo criado
    if (this.pageMode === PageModeEnum.Create) {
      validatorsSenha.push(Validators.required);
    }

    // Monta o formulário dos campos de cadastro do usuário com seus respectivos validators
    this.formUsuario = this.fb.group({
      codigo: ['', [ Validators.required, Validators.maxLength(50) ] ],
      senha: ['', validatorsSenha ],
      nome: ['', [ Validators.required, Validators.maxLength(100) ] ],
      email: ['', [ Validators.maxLength(60), Validators.required, Validators.email ] ],
      expiracaoSenhaAtivada: [false, [] ],
    });

    // Monta a parte dinâmica do formulário com todas as permissões
    this.permissoesSistema.forEach((permission) => {

      // Monta o nome do formControl para cada permissão
      const formControlName = this.getPermissionFormControl(permission);

      // Inclui cada controle no formulário
      this.formUsuario.addControl(formControlName, new FormControl(false, []));
    });
  }


  /**
   * Abertura do modal solicitada a partir de um usuário específico existente (consulta ou edição)
   */
  async openModalUsuario(usuarioId: number) {
    // Se não há nenhuma permissão, não há o que fazer
    if (!this.hasPermissaoConsulta && !this.hasPermissaoEdita) {
      return;
    }

    // Não há o que fazer se não tiver usuário solicitado
    if (isNullOrUndefined(usuarioId)) {
      return;
    }

    // Armazena o ID do usuário fornecido
    this.usuarioId = usuarioId;

    // Define o modo de acesso do modal
    this.pageMode = this.hasPermissaoEdita ? PageModeEnum.Update : PageModeEnum.Read;

    // Recupera dados do usuario selecionado
    this.usuarioService
      // Aciona o método de busca ao backend
      .getById(usuarioId)
      // A partir do observable retornado, registra o que deve ser executado após a conclusão do carregamento através do pipe
      .pipe(
        // Força a executar o subscribe apenas uma vez, e aí desregistra o subscribe
        take(1)
      )
      // Registra uma 'escuta' (subscribe) para o que deve ser executado após a conclusão do carregamento através do subscribe
      .subscribe(
        // Método a ser executado em caso de request sem erros
        async (usuario: UsuarioResponseViewModel) => {
          // Armazena o objeto recebido do backend que representa o usuário carregado
          this.usuarioCarregado = usuario;

          // Abre modal para edição ou visualização
          await this.openModal();
        },
        // Método a ser executado se houve erro no request
        (error) => {
          // Insere um log no console com o erro retornado
          console.error(error);
        }
      );
  }

  /**
   * Abertura do modal solicitada para a criação de um novo usuário
   */
  async openModalNew(event: any) {
    // Não há o que fazer se não tiver permissão de criação
    if (!this.hasPermissaoCria) {
      return;
    }

    // Cria um novo objeto de usuário para ser preenchido
    this.usuarioCarregado = new UsuarioResponseViewModel();

    // Atualiza status do modal
    this.pageMode = PageModeEnum.Create;

    // Abre modal
    await this.openModal();
  }

  /**
   * Faz a abertura do modal a partir de um usuário e modo de acesso já definidos
   */
  private async openModal() {
    // Cria formulários com inputs de cadastro
    this.createFormUsuario();

    // Preenche os dados obtidos nos inputs
    this.fillFormUsuario(this.usuarioCarregado);

    // Define o título do modal
    const titulo = this.textoTituloModal();

    // Abre modal
    this.modalRef = this.modalService.openModalCadastro(titulo, this.modalBody, this.modalFooter);

    // Define os métodos a serem executados nos eventos de resolve e reject do modal
    const onResolved = () => this.cleanForms();
    const onRejected = () => this.cleanForms();

    // Limpa inputs e objeto no fechamento do modal através dos eventos citados
    this.modalRef.result.then(
      onResolved,
      onRejected
    );
  }

  /**
   * Reinicializa inputs do modal
   */
  private cleanForms() {
    // Reinicializa objeto do usuário
    this.usuarioCarregado = new UsuarioResponseViewModel();

    // Limpa inputs do form Usuario
    this.formUsuario.reset();
  }

  /**
   * Preenche os inputs do modal com os dados do usuário
   */
  private fillFormUsuario(usuario: UsuarioResponseViewModel) {
    // Não há o que fazer se o objeto do usuário for inválido
    if (isNullOrUndefined(usuario)) {
      return;
    }

    // Preenche os inputs do modal
    this.formUsuario.get('codigo').setValue(usuario.codigo);
    this.formUsuario.get('nome').setValue(usuario.nome);
    this.formUsuario.get('email').setValue(usuario.email);
    this.formUsuario.get('expiracaoSenhaAtivada').setValue(usuario.expiracaoSenhaAtivada);

    // Preenche os inputs das permissões
    this.permissoesSistema.forEach((permission) => {

      // Monta o nome do formControl para cada permissão
      const formControlName = this.getPermissionFormControl(permission);

      // Verifica se o usuário possui a permissão em questão
      const hasPermission = usuario.permissoes?.includes(permission.value) ?? false;

      // Insere no formulário se o usuário possui a permissão ou não
      this.formUsuario.get(formControlName).setValue(hasPermission);
    });
  }

  /**
   * Define o texto para o título do modal
   */
  private textoTituloModal(): string {
    switch (this.pageMode) {
      case PageModeEnum.Create:
        return 'Novo Usuário';
      case PageModeEnum.Update:
        return 'Edição de Usuário';
      default:
        return 'Consulta de Usuário';
    }
  }

  /**
   * Preenche o objeto de salvamento para criação do usuário com os dados dos inputs do modal
   */
  private fillObjetoUsuarioAdd(): UsuarioAddViewModel {
    return {
      codigo: this.formUsuario.get('codigo').value,
      senha: this.formUsuario.get('senha').value,
      nome: this.formUsuario.get('nome').value,
      email: this.formUsuario.get('email').value,
      expiracaoSenhaAtivada: this.formUsuario.get('expiracaoSenhaAtivada').value,
      permissoes: this.getPermissoesSelecionadas(),
    } as UsuarioAddViewModel;
  }

  /**
   * Preenche o objeto de salvamento para edição do usuário com os dados dos inputs do modal
   */
  private fillObjetoUsuarioEdit(): UsuarioEditViewModel {
    return {
      id: this.usuarioId,
      codigo: this.formUsuario.get('codigo').value,
      senha: this.formUsuario.get('senha').value,
      nome: this.formUsuario.get('nome').value,
      email: this.formUsuario.get('email').value,
      expiracaoSenhaAtivada: this.formUsuario.get('expiracaoSenhaAtivada').value,
      permissoes: this.getPermissoesSelecionadas(),
    } as UsuarioEditViewModel;
  }

  /**
   * Pega todos os tipos de permissões selecionados no modal
   */
   private getPermissoesSelecionadas(): PermissaoSistemaEnum[] {
    // Varre cada tipo de permissão existente para retornar cada item de permissão
    return this.permissoesSistema
      // Faz primeiro o filtro para pegar apenas as permissões concedidas pelo modal
      .filter((permission) => {
        // Monta o nome do formControl para cada permissão
        const formControlName = this.getPermissionFormControl(permission);

        // Verifica se o usuário possui a permissão em questão
        const hasPermission = this.formUsuario.get(formControlName).value as boolean;

        // Pega apenas as permissões concedidas
        return hasPermission;
    })
      // Mapeia as permissões concedidas para retornar apenas o item da permissão
      .map((permission) => permission.value);
  }

  /**
   * Ação de submit/conclusão do modal
   */
  onSubmit(event: any) {
    // Verifica se o formulário está válido
    if (this.formUsuario.invalid) {
      this.formUsuario.markAllAsTouched();
      return false;
    }

    // Através do modo da página decide qual webservice será utilizado.
    let usuarioRequest: UsuarioAddViewModel | UsuarioEditViewModel;
    let serviceAction: any;

    // Faz o request exclusivo para criação
    if (this.pageMode === PageModeEnum.Create) {
      // Cria um objeto de inclusão de usuário
      usuarioRequest = this.fillObjetoUsuarioAdd();

      // Define o action para o Create
      serviceAction = this.usuarioService.create;
    }

    // Faz o request exclusivo para edição
    if (this.pageMode === PageModeEnum.Update) {
      // Cria um objeto de edição de usuário
      usuarioRequest = this.fillObjetoUsuarioEdit();

      // Define o action para o Edit
      serviceAction = this.usuarioService.edit;
    }

    serviceAction
      // Faz a chamada de forma dinâmica através do apply, já que podem ser 2 métodos diferentes para request
      .apply(this.usuarioService, [usuarioRequest])
      // Conduz a chamada para o subscribe, inclusive para remover a 'escuta' do subscribe após a conclusão do request
      .pipe(take(1))
      // Escuta através do subscribe o response
      .subscribe(
        // Em caso de sucesso no request:
        (response) => {
          // Fecha o modal após o sucesso
          this.closeModal();

          // Aciona o evento pós-salvamento com sucesso
          this.afterSave.emit(response);
        },

        // Em caso de erro no request:
        (error: any) => {
          // Repassa o erro para o console apenas
          console.error(error);

          // Aciona o evento pós-erro
          this.afterError.emit(error);
        }
      );
  }

  /**
   * Ação de fechamento/cancelamento do modal
   */
  onCancel(event: any) {
    // Fecha o modal após solicitado
    this.closeModal();

    // Aciona o evento pós-fechamento concluído
    this.afterCancel.emit(event);
  }

  /**
   * Aciona o fechamento do modal
   */
  private closeModal() {
    this.modalRef.close();
    this.modalRef = null;
  }
}
