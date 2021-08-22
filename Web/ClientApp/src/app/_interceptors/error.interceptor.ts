import { Injectable } from '@angular/core';
import { HttpRequest, HttpHandler, HttpEvent, HttpInterceptor, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { Router } from '@angular/router';
import { AuthenticationService } from '../_services/authentication.service';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {
  constructor(private authenticationService: AuthenticationService, private router: Router) {}

  /**
   * Interceptador de responses HTTP para tratar determinadas situações de forma específica
   */
  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    return next.handle(request).pipe(
      catchError((response: HttpErrorResponse) => {
        // Trata http codes específicos de forma diferenciada
        switch (response.status) {
          case 401:
            this.handleUnauthorized();
            break;
        }

        return throwError(response);
      })
    );
  }

  /**
   * Não autenticado corretamente (Unauthorized)
   */
  private handleUnauthorized() {
    // Auto logout if 401 response returned from api
    this.authenticationService.logout();

    // Não precisa redirecionar se já estiver na página de login
    if (this.router.url !== '/login') {
      // Força o reload na página para o direcionamento
      location.reload();
    }
  }
}
