import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { take } from 'rxjs/operators';
import { AuthenticationService } from 'src/app/_services/authentication.service';
import { MenuService } from 'src/app/_services/menu.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {

  /**
   * Formulário com os inputs
   */
  formLogin: FormGroup;

  /**
   * URL para redirecionar após o login bem sucedido, se houver
   */
  public returnUrl: string;

  /**
   * Faz exibir ou não uma mensagem de alerta
   */
  public showAlertMessage: boolean;

  /**
   * Mensagem de alerta
   */
  public descriptionAlertMessage: string;

  constructor(
    private authenticationService: AuthenticationService,
    private formBuilder: FormBuilder,
    private menuService: MenuService,
    private activatedRoute: ActivatedRoute,
    private route: Router
  ) { }

  /**
   * Método acionado depois que o componente já foi inicializado, que ocorre em um momento diferente do construtor
   */
  ngOnInit(): void {
    // Inicializa os campos do formulário
    this.createForm();

    // Se a página de login foi acessada, força o logout
    this.authenticationService.logout();

    // Inicializa a informação se a página de login foi acessada por consequência de um redirecionamento automático
    let redirecionado = false;

    // Verifica se a página de login foi acessada com redirecionamento para outra página após realizar o login
    this.activatedRoute.queryParams.subscribe((params) => {
      redirecionado = params.redirecionado;
      this.returnUrl = params.returnUrl;
    });

    // Se foi redirecionado automaticamente para a página de login após tentar acessar outra página, notifica o usuário
    if (redirecionado) {
      // Exibe o alerta para o usuário
      this.showAlertMessage = true;
      this.descriptionAlertMessage = 'Login expirado ou inexistente.';
    }
  }

  /**
   * Cria o FormGroup (Formulário com os campos de login) a partir de um FormBuilder
   */
  createForm() {
    // O form builder faz a construção de um novo FormGroup para que ele seja criado com as regras necessárias aos inputs
    this.formLogin = this.formBuilder.group({
      // Para cada campo do formulário, define o valor inicial e as regras de validação
      usuario: ['', Validators.required],
      senha: ['', Validators.required],
    });
  }

  /**
   * Solicia o request para o login a partir dos campos preenchidos
   */
  onSubmit() {
    // Verifica se o formulário de login está válido
    if (!this.formLogin.valid) {
      // Marca todos os campos do formulário como 'touched' para que os inputs exibam seus respectivos validators não satisfeitos
      this.formLogin.markAllAsTouched();
      return;
    }

    // Tenta acionar o login com os dados preenchidos
    this.authenticationService
      // Ao passar o 'value' do form, como o form tem as mesmas propriedades do objeto de entrada do método, o método aceita a entrada
      .login(this.formLogin.value)
      // Aplica o Take para que o unsubscribe seja realizado após a primeira execução do subscribe
      .pipe(take(1))
      .subscribe(
        () => {
          // Recarrega o menu
          this.menuService.loadMenu();

          // Verifica se havia alguma rota prevista para ser acionada após o login
          if (this.returnUrl) {
            // Acessa a rota solicitada
            this.route.navigate([this.returnUrl]);
          } else {
            // Em caso de sucesso no login, navega para a rota default
            this.route.navigate(['']);
          }
        },

        // Prepara captura de falhas para exibição de um alerta
        (error: HttpErrorResponse) => {
          // Exibe a mensagem de falha na autenticação
          this.showAlertMessage = true;
          this.descriptionAlertMessage = error.error;
        }
      );
  }
}
