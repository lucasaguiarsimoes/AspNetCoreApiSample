using AspNetCoreApiSample.Repository.Interface;
using AspNetCoreApiSample.Repository.Repositories;
using AspNetCoreApiSample.Service.Interface;
using AspNetCoreApiSample.Service.Services;
using AspNetCoreApiSample.Web.AutoMapper;
using AspNetCoreApiSample.Web.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace AspNetCoreApiSample.Web
{
    public class Startup
    {
        /// <summary>
        /// Ambiente de execu��o da aplica��o Web
        /// </summary>
        private IWebHostEnvironment Environment { get; }

        /// <summary>
        /// Configura��o do aplicativo
        /// </summary>
        public IConfiguration Configuration { get; }

        public Startup(IWebHostEnvironment env, IConfiguration configuration)
        {
            this.Configuration = configuration;
            this.Environment = env;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Registra todos os servi�os no container de inje��o de depend�ncia
            RegisterServices(services);

            services.AddControllersWithViews();

            if (this.Environment != null && this.Environment.IsDevelopment())
            {
                // Adiciona Swagger
                services.AddSwaggerDocumentation();
            }

            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Utiliza o handler customizado de exceptions para tratamento de responses
            app.UseCustomExceptionHandler();

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            if (!env.IsDevelopment())
            {
                app.UseSpaStaticFiles();
            }

            if (env.IsDevelopment())
            {
                // Configura��o do Swagger
                app.UseSwaggerDocumentation();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
            });

            app.UseSpa(spa =>
            {
                // To learn more about options for serving an Angular SPA from ASP.NET Core,
                // see https://go.microsoft.com/fwlink/?linkid=864501

                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseAngularCliServer(npmScript: "start");
                }
            });
        }

        /// <summary>
        /// M�todo que registra todos os servi�os do sistema no container de inje��o de depend�ncia
        /// </summary>
        private void RegisterServices(IServiceCollection services)
        {
            // Todos os servi�os de inje��o de depend�ncia s�o registrados atrav�s do IServiceCollection fornecido pelo ASP.NET Core
            services

                // Registra profiles para o AutoMapper
                .AddAutoMapper((provider, cfg) =>
                {
                    cfg.AddProfile<DomainToViewModelMappingProfile>();
                    cfg.AddProfile<ViewModelToDomainMappingProfile>();
                }, typeof(Startup))

                // Registra servi�os do sistema
                .AddScoped<IUsuarioService, UsuarioService>()
                .AddScoped<ITokenJwtService, TokenJwtService>()
                .AddScoped<IAuthenticationService, AuthenticationService>()

                // Registra reposit�rios do sistema
                // Cria como Singleton para que os dados em memoria existam enquanto o sistema estiver executando para simular um banco de dados InMemory
                .AddSingleton<IUsuarioRepository, UsuarioRepositoryInMemory>()

                // Registra autentica��o no sistema via token JWT
                .AddJwtAuthorization();
        }
    }
}
