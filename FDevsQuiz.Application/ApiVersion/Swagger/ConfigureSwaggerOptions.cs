﻿using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.IO;

namespace FDevsQuiz.Application.ApiVersion.Swagger
{
    public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
    {
        private readonly IApiVersionDescriptionProvider _provider;

        public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider) => _provider = provider;

        public void Configure(SwaggerGenOptions options)
        {
            foreach (var description in _provider.ApiVersionDescriptions)
            {
                options.SwaggerDoc(description.GroupName, CreateInfoForApiVersion(description));
                //Essa linha deve ser inserida em casos que há classes com mesmo nome em namespaces diferentes
                options.CustomSchemaIds(x => x.FullName);
            }

            //Obtendo o diretório e depois o nome do arquivo .xml de comentários
            var applicationBasePath = PlatformServices.Default.Application.ApplicationBasePath;
            var applicationName = PlatformServices.Default.Application.ApplicationName;
            var xmlDocumentPath = Path.Combine(applicationBasePath, $"{applicationName}.xml");

            //Caso exista arquivo então adiciona-lo
            if (File.Exists(xmlDocumentPath))
                options.IncludeXmlComments(xmlDocumentPath);

            //options.AddFluentValidationRules();

            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = @"API utiliza autenticação. Informe o token de acesso no campo abaixo.
                                Exemplo: 'Bearer 12345abcdef...'",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement()
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        },
                        Scheme = "oauth2",
                        Name = "Bearer",
                        In = ParameterLocation.Header
                    },
                    new List<string>()
                }
            });
        }

        static OpenApiInfo CreateInfoForApiVersion(ApiVersionDescription description)
        {
            var info = new OpenApiInfo
            {
                Title = "FDevs - Quiz",
                Version = description.ApiVersion.ToString(),
                Description = "FDevs - Quiz. Recursos disponiveis para implementação do QUIZ",
                Contact = new OpenApiContact
                {
                    Name = "Odézio Pereira",
                    Url = new Uri("https://fdevs-quiz.com")
                }
            };

            if (description.IsDeprecated)
                info.Description += " - Esta versão da API está descontinuada.";

            return info;
        }
    }
}

