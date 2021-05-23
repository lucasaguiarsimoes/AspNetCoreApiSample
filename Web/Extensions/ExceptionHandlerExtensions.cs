using AspNetCoreApiSample.Domain.Exceptions;
using AspNetCoreApiSample.Web.ViewModels;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AspNetCoreApiSample.Web.Extensions
{
    /// <summary>
    /// Classe de extensão para preparar o builder da aplicação para utilizar um handler de exceptions para requests
    /// </summary>
    public static class ExceptionHandlerExtensions
    {
        /// <summary>
        /// Prepara um handler de exceptions para requests realizados aos controllers
        /// </summary>
        public static void UseCustomExceptionHandler(this IApplicationBuilder app)
        {
            // Inclui o handler de exceptions
            app.UseExceptionHandler(new ExceptionHandlerOptions()
            {
                AllowStatusCode404Response = true,
                ExceptionHandler = async (context) =>
                {
                    // Utiliza um status code default de internal error
                    context.Response.StatusCode = 500;
                    context.Response.ContentType = "application/json";

                    // Resgata o exception ocorrido
                    IExceptionHandlerFeature feature = context.Features.Get<IExceptionHandlerFeature>();

                    // Pega o exception capturado
                    Exception? exc = feature?.Error;

                    // Existe a possibilidade neste momento de recuperar o logger para gerar logs para o exception capturado
                    //ILogger logger = context.RequestServices.GetRequiredService<ILogger<Startup>>();

                    // Cria o ViewModel do erro ocorrido
                    ErrorViewModel errorModel = CreateError(context, exc);

                    // Atualiza o status code de retorno do response
                    context.Response.StatusCode = errorModel.status;

                    // Inclui o objeto no response
                    await context.Response.WriteAsJsonAsync(errorModel);
                }
            });

        }

        /// <summary>
        /// Cria o view model de erro para o HttpResponse
        /// </summary>
        private static ErrorViewModel CreateError(HttpContext context, Exception? exc)
        {
            return exc switch
            {
                // Exception de entidade conflito de propriedades únicas duplicadas
                EntityDuplicatedException custom => DefaultError(StatusCodes.Status422UnprocessableEntity, exc),

                // Exception de entidade obrigatória não encontrada
                EntityNotFoundException custom => DefaultError(StatusCodes.Status422UnprocessableEntity, exc),

                // Exception de valor único no sistema não respeitado
                EntityUniqueViolatedException custom => DefaultError(StatusCodes.Status422UnprocessableEntity, exc),

                // Exception de não autorizado
                UnauthorizedAccessException custom => DefaultError(StatusCodes.Status401Unauthorized, exc),

                // Falha padrão
                _ => DefaultError(StatusCodes.Status500InternalServerError, exc),
            };
        }

        /// <summary>
        /// Cria um objeto padrão de erro para o response
        /// </summary>
        private static ErrorViewModel DefaultError(int httpStatusCode, Exception? exc)
        {
            return new ErrorViewModel(httpStatusCode, httpStatusCode == 500 ? exc!.ToString() : exc!.Message);
        }
    }
}
