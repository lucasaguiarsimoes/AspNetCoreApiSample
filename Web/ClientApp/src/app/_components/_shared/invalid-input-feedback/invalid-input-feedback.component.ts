import { Component, Input, OnInit, TemplateRef } from '@angular/core';
import { AbstractControl, FormGroup } from '@angular/forms';
import { isNullOrEmpty, isNullOrUndefined } from 'src/app/_common/util';

@Component({
  selector: 'app-invalid-input-feedback',
  templateUrl: './invalid-input-feedback.component.html',
  styleUrls: ['./invalid-input-feedback.component.scss']
})
export class InvalidInputFeedbackComponent implements OnInit {

  /**
   * Form que o input associado pertence
   */
  @Input() form: FormGroup;

  /**
   * Nome do Controle que corresponde a um input
   */
  @Input() controlName: string;

  /**
   * Controle que corresponde a um input
   */
  get control(): AbstractControl {
    return this.getControl();
  }

  constructor() {}

  ngOnInit() {}

  /**
   * Retorna os erros do controle do input em questão
   */
  get getControlErrors(): any[] {
    // Pega o objeto que representa o controle do input
    const controle = this.getControl();

    // Já sai se não houverem erros
    if (!controle || !controle.errors) {
      return [];
    }

    // Retorna a lista de erros existentes no controle
    return Object.keys(controle.errors);
  }

  /**
   * Pega o controle a partir do form
   */
  private getControl(): AbstractControl {
    // Não há o que fazer se não houver form
    if (isNullOrUndefined(this.form) || isNullOrEmpty(this.controlName) ) {
      return null;
    }

    // Procura o controle do form a partir do nome do controle
    return this.form.get(this.controlName);
  }

  /**
   * Verifica se a descrição do erro é válida
   */
  errorDescriptionValid(name: string): boolean {
    // O erro apenas é válido para exibição se for do tipo string
    return typeof this.getControl()?.errors[name] === 'string';
  }

  /**
   * Retorna o conteúdo da descrição do erro do input
   */
  getErrorDescription(name: string): string {
    return this.getControl()?.errors[name];
  }

  /**
   * Verifica se o input em questão possui algum erro de formulário
   */
  hasError(): boolean {
    // Pega o controle do input
    const control = this.getControl();

    // Garante que o controle é válido e existente
    if (isNullOrUndefined(control)) {
      return false;
    }

    // Apenas exibe o erro se o input estiver inválido e alterado
    return control.invalid && (control.dirty || control.touched);
  }
}
