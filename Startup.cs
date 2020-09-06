using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.IO;
using NFCE.API.Interfaces;
using NFCE.API.Services;
using NFCE.API.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using NFCE.API.Extensions;
using Microsoft.AspNetCore.Mvc;
using NFCE.API.Interfaces.Services;
using NFCE.API.Interfaces.Repositories;
using System.Collections.Generic;
using NFCE.API.Helpers;
using NFCE.API.Models;

namespace NFCE.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddMvc();
            services.Configure<ApiBehaviorOptions>(x =>
            {
                // x.SuppressModelStateInvalidFilter = true;
                x.InvalidModelStateResponseFactory = (actionContext) => ResponseHelper.TratamentoModelState(actionContext);
            });
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "NFCE API",
                    Version = "v1",
                    Contact = new OpenApiContact
                    {
                        Email = "ailton.godinho@hotmail.com",
                        Name = "Ailton Godinho"
                    },
                    Description = "API para controle do aplicativo \"NFCE Agora!\""
                });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "Entre com a chave de autorização",
                    In = ParameterLocation.Header,
                    Name = "Authorization",
                    Scheme = "Bearer",
                    Type = SecuritySchemeType.ApiKey,
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header
                        },
                        new List<string>()
                    }
                });
                string xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                string xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            #region JWT
            var sigingConfigurations = new SigningExtension();

            services.AddAuthentication(authOptions =>
            {
                authOptions.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                authOptions.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(bearerOptions =>
            {
                bearerOptions.RequireHttpsMetadata = false;
                bearerOptions.SaveToken = true;
                bearerOptions.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    IssuerSigningKey = sigingConfigurations.Chave,
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    ValidAudience = Configuration.GetValue<string>("TokenConfiguration:Audience"),
                    ValidIssuer = Configuration.GetValue<string>("TokenConfiguration:Issuer"),
                    //  Valida o token recebido
                    ValidateIssuerSigningKey = true,
                    //  Verifica se ainda é valido
                    ValidateLifetime = true,
                    // Tempo de tolerância para a expiração de um token (utilizado
                    // caso haja problemas de sincronismo de horário entre diferentes
                    // computadores envolvidos no processo de comunicação)
                    ClockSkew = TimeSpan.Zero,
                };
            });

            // Ativa o uso do token como forma de autorizar o acesso
            // a recursos deste projeto
            // services.AddAuthorization(auth =>
            // {
            //     auth.AddPolicy("Bearer", new AuthorizationPolicyBuilder()
            //         .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
            //         .RequireAuthenticatedUser().Build());
            // });

            #endregion

            var ibge = new IBGEService(Configuration);
            var viaCEP = new ViaCEPService(Configuration);

            services.AddHttpContextAccessor();
            services.AddControllersWithViews(c =>
            {
                c.Filters.Add(typeof(ControllerActionFilter));
            });
            #region Services
            services.AddScoped(typeof(IBaseService<>), typeof(BaseService<>));
            services.AddScoped<IAgregadoService, AgregadoService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IComprasService, ComprasService>();
            services.AddScoped<IComprasProdutoService, ComprasProdutoService>();
            services.AddScoped<IEmissorService, EmissorService>();
            services.AddScoped<IExtracaoService, ExtracaoService>();
            services.AddScoped<IItemService, ItemService>();
            services.AddScoped<INotaService, NotaService>();
            services.AddScoped<IPagamentoService, PagamentoService>();
            services.AddScoped<IProdutoService, ProdutoService>();
            services.AddScoped<IUsuarioService, UsuarioService>();
            services.AddScoped<ILocalidadeService, LocalidadeService>();
            services.AddScoped<ISaldosService, SaldosService>();
            services.AddSingleton(ibge);
            services.AddSingleton(viaCEP);
            services.AddSingleton(new ConfiguracaoModel());
            services.AddSingleton(sigingConfigurations);
            #endregion
            #region Repositories
            services.AddScoped(typeof(IRepositoryBase<>), typeof(RepositoryBase<>));
            services.AddScoped<IAuthRepository, AuthRepository>();
            services.AddScoped<IComprasRepository, ComprasRepository>();
            services.AddScoped<IComprasProdutoRepository, ComprasProdutoRepository>();
            services.AddScoped<IEmissorRepository, EmissorRepository>();
            services.AddScoped<IItemRepository, ItemRepository>();
            services.AddScoped<INotaRepository, NotaRepository>();
            services.AddScoped<IPagamentoRepository, PagamentoRepository>();
            services.AddScoped<IProdutoRepository, ProdutoRepository>();
            services.AddScoped<IUsuarioRepository, UsuarioRepository>();
            services.AddScoped<ILocalidadeRepository, LocalidadeRepository>();
            services.AddScoped<ISaldosRepository, SaldosRepository>();
            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "NFCE API");
                c.RoutePrefix = "swagger";
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
