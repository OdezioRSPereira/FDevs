using DbUp;
using FDevsQuiz.Application.Identity;
using FDevsQuiz.Application.Middleware;
using FDevsQuiz.Domain.Interface;
using FDevsQuiz.Domain.Repository;
using FDevsQuiz.Domain.Services;
using FDevsQuiz.Infra.Data.Context;
using FDevsQuiz.Infra.Data.Repository;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerUI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace FDevsQuiz
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private static string GetXmlCommentsPath()
        {
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, String.Format("{0}.xml", AppName()));
        }

        private static string AppName()
        {
            return Assembly.GetCallingAssembly().GetName().Name;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IDbContext, DbContext>();

            services.AddScoped<IContatoRepository, ContatoRepository>();
            services.AddScoped<IUsuarioRepository, UsuarioRepository>();
            services.AddScoped<INivelRepository, NivelRepository>();
            services.AddScoped<IQuizRepository, QuizRepository>();
            services.AddScoped<IPerguntaRepository, PerguntaRepository>();
            services.AddScoped<IAlternativaRepository, AlternativaRepository>();
            services.AddScoped<IRespostaRepository, RespostaRepository>();
            services.AddScoped<IUsuario, UsuarioContext>();
            services.AddScoped<AutenticacaoService>();
            services.AddScoped<QuizService>();

            services.AddApiVersioning(o =>
            {
                o.ReportApiVersions = true;
                o.AssumeDefaultVersionWhenUnspecified = true;
                o.DefaultApiVersion = new ApiVersion(1, 0);

                o.ApiVersionReader = ApiVersionReader.Combine(
                    new HeaderApiVersionReader("x-api-version"),
                    new QueryStringApiVersionReader(),
                    new UrlSegmentApiVersionReader());
            });

            var key = Encoding.ASCII.GetBytes(Configuration.GetValue<string>("secret"));
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "FDevs Quiz - API",
                    Version = "v1",
                    Description = "Documentação da API do FDevs Quiz."
                });

              //  c.IncludeXmlComments(GetXmlCommentsPath());

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Type = SecuritySchemeType.ApiKey,
                    In = ParameterLocation.Header,
                    Name = "Authorization",
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new List<string>()
                    }
                });
            });

            services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            }); ;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var connectionString = Configuration.GetConnectionString("FDevsQuizConnection");

            EnsureDatabase.For.SqlDatabase(connectionString);

            var upgrader = DeployChanges.To.SqlDatabase(connectionString)
                .WithScriptsFromFileSystem(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "Script"))
                .Build();

            var result = upgrader.PerformUpgrade();

            if (!result.Successful)
                throw new Exception("Falha ao atualizar o banco de dados.");

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

            app.UseMiddleware(typeof(ExceptionMiddleware));

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            // Habilita o uso do Swagger
            app.UseSwagger();

            // Habilitar o middleware para servir o swagger-ui (HTML, JS, CSS, etc.), 
            // Especificando o Endpoint JSON Swagger.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("./swagger/v1/swagger.json", "V1");
                c.RoutePrefix = string.Empty;
                c.DocExpansion(DocExpansion.None);
            });
        }
    }
}
