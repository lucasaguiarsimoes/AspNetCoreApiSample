import { ElementRef, Injectable } from '@angular/core';
import { NgbModal, NgbModalOptions, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { ModalTemplateComponent } from '../_components/_shared/modal-template/modal-template.component';

@Injectable({
  providedIn: 'root'
})
export class ModalService {
  constructor(private modal: NgbModal) {}

  /**
   * Opções padrões do modal de acordo com as opções previstas pelo NgbModalOptions
   */
  private internalDefaultOptions: NgbModalOptions = {
    backdrop: 'static',
    keyboard: false,
    size: 'xl',
    scrollable: true,
  };

  /**
   * Método para acessar as opções padrões do modal
   */
  get defaultOptions(): NgbModalOptions {
    return Object.assign({}, this.internalDefaultOptions);
  }


  /**
   * Método para abertura de modal de Cadastro de Entidades
   */
  public openModalCadastro(
    titulo?: string,
    modalBody?: ElementRef,
    modalFooter?: ElementRef,
    beforeDismiss?: () => boolean | Promise<boolean>
  ): NgbModalRef {

    // Configurações do modal padrão de acordo com as opções previstas pelo NgbModalOptions
    const modalOptions = {
      backdrop: 'static',
      keyboard: false,
      size: 'xl',
      scrollable: true,
      beforeDismiss,
    } as NgbModalOptions;

    // Aciona método padrão
    return this.openModal(modalOptions, titulo, modalBody, modalFooter);
  }

  // Método padrão para abertura de modal
  public openModal(
    modalOptions: NgbModalOptions = this.internalDefaultOptions,
    titulo?: string,
    modalBody?: ElementRef,
    modalFooter?: ElementRef
  ): NgbModalRef {

    // Abre modal
    const modalRef: NgbModalRef = this.modal.open(ModalTemplateComponent, modalOptions);

    // Atualiza título do modal
    modalRef.componentInstance.titulo = titulo;

    // Atualiza o corpo do modal com o template implementado
    modalRef.componentInstance.templateBody = modalBody;

    // Atualiza o footer do modal com o template implementado
    modalRef.componentInstance.templateFooter = modalFooter;

    return modalRef;
  }
}
