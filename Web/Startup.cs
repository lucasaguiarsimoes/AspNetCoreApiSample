using AspNetCoreApiSample.Database;
using AspNetCoreApiSample.Database.Connection;
using AspNetCoreApiSample.Database.Connection.Interface;
using AspNetCoreApiSample.Database.Context;
using AspNetCoreApiSample.Database.Context.Interface;
using AspNetCoreApiSample.Database.Repositories.InMemory;
using AspNetCoreApiSample.Database.Repository;
using AspNetCoreApiSample.Repository;
using AspNetCoreApiSample.Repository.Interface;
using AspNetCoreApiSample.Service.Interface;
using AspNetCoreApiSample.Service.Services;
using AspNetCoreApiSample.Web.AutoMapper;
using AspNetCoreApiSample.Web.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;

namespace AspNetCoreApiSample.Web
{
    public class Startup
    {
        /// <summary>
        /// Ambiente de execução da aplicação Web
        /// </summary>
        private IWebHostEnvironment Environment { get; }

        /// <summary>
        /// Configuração do aplicativo
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
            // Registra todos os serviços no container de injeção de dependência
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
                // Configuração do Swagger
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
        /// Método que registra todos os serviços do sistema no container de injeção de dependência
        /// </summary>
        private void RegisterServices(IServiceCollection services)
        {
            // Todos os serviços de injeção de dependência são registrados através do IServiceCollection fornecido pelo ASP.NET Core
            services

                // Registra profiles para o AutoMapper
                .AddAutoMapper((provider, cfg) =>
                {
                    cfg.AddProfile<DomainToViewModelMappingProfile>();
                    cfg.AddProfile<ViewModelToDomainMappingProfile>();
                }, typeof(Startup))

                // Registra serviços do sistema
                .AddScoped<IUsuarioService, UsuarioService>()
                .AddScoped<ITokenJwtService, TokenJwtService>()
                .AddScoped<IAuthenticationService, AuthenticationService>()
                .AddScoped<ICryptographyService, CryptographySHA256Service>()

                // Repositorio para acesso InMemory de usuários caso se deseje não utilizar o banco de dados no sistema
                //.AddSingleton<IUsuarioRepository, UsuarioRepositoryInMemory>()

                // Registra repositories para acesso às entidades do sistema no banco de dados
                .AddScoped<IUsuarioRepository, UsuarioRepository>()
                .AddScoped<IUsuarioPermissaoRepository, UsuarioPermissaoRepository>()

                // Registra os serviços principais de acesso ao banco de dados
                .AddScoped<IUnitOfWork, UnitOfWork>()
                .AddDbContext<IEntityContext, DefaultDbContext>(ServiceLifetime.Scoped, ServiceLifetime.Scoped)
                .AddTransient<IConnection, SQLServerConnection>(s => new SQLServerConnection(this.Configuration.GetConnectionString("SQLServerSample")))

                // Inclui logger para o uso pelo EF Core
                .AddLogging(builder => builder
                .AddConsole()
                .AddFilter(DbLoggerCategory.Database.Command.Name, LogLevel.Information))

                // Registra autenticação no sistema via token JWT
                .AddJwtAuthorization();

            // Faz uma construção específica dos services para um provider para executar os migrations do sistema
            IServiceProvider serviceProvider = services.BuildServiceProvider();

            // Executa os migrations pendentes, caso existam
            serviceProvider.GetRequiredService<IUnitOfWork>().MigrateAsync(new CancellationToken()).Wait();
        }
    }
}
