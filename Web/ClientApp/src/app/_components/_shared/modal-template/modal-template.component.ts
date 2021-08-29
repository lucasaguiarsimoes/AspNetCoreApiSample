import { AfterViewInit, Component, EventEmitter, Input, OnInit, Output, TemplateRef } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-modal-template',
  templateUrl: './modal-template.component.html',
  styleUrls: ['./modal-template.component.scss']
})
export class ModalTemplateComponent implements OnInit, AfterViewInit {

  /**
   * Título do header do modal
   */
  @Input() titulo: string;

  /**
   * Template para o corpo do modal
   */
  @Input() templateBody: TemplateRef<any>;

  /**
   * Template para o rodapé do modal
   */
  @Input() templateFooter: TemplateRef<any>;

  /**
   * Evento disparado depois que o modal finaliza sua inicialização
   */
  @Output() afterViewInit = new EventEmitter();

  constructor(public modal: NgbActiveModal) {}

  ngOnInit() {}

  /**
   * Evento do próprio Angular (via interface AfterViewInit) disparado
   */
  ngAfterViewInit() {
    // Redireciona e emite o evento de outupt do componente
    Promise.resolve().then(() => {
      this.afterViewInit.emit();
    });
  }
}
