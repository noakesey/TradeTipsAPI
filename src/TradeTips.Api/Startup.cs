using AutoMapper;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TradeTips.Features.Profiles;
using TradeTips.Domain;
using TradeTips.Security;

namespace TradeTips.Api
{
    public class Startup
    {
        public const string DEFAULT_DATABASE_CONNECTIONSTRING = "Server=WS2019\\SQLExpress;database=TradeTipsAPI;trusted_connection=true;";

        #region Entry points

        private readonly IConfiguration _config;
        public Startup(IConfiguration config)
        {
            _config = config;
        }

        // Add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddLocalization(x => x.ResourcesPath = "Resources");
            services.AddMvc(opt =>
            {
                opt.Conventions.Add(new GroupByApiRootConvention());
                opt.Filters.Add(typeof(ValidatorActionFilter));
                opt.EnableEndpointRouting = false;
            })
                //To prevent "A possible object cycle was detected which is not supported."
                .AddNewtonsoftJson(opt =>
                {
                    opt.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                })
                .AddJsonOptions(opt =>
                {
                    opt.JsonSerializerOptions.IgnoreNullValues = true;

                })
                .AddFluentValidation(cfg =>
                {
                    cfg.RegisterValidatorsFromAssemblyContaining<TradeTips.Features.Articles.ListHandler>();
                });

            services.AddMediatR(typeof(TradeTips.Features.Articles.ListHandler).Assembly);

            services.AddAutoMapper(typeof(TradeTips.Features.Profiles.MappingProfile).Assembly);

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(DBContextTransactionPipelineBehavior<,>));
            services.AddScoped<IPasswordHasher, PasswordHasher>();
            services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
            services.AddScoped<ICurrentUserAccessor, CurrentUserAccessor>();
            services.AddScoped<IProfileReader, ProfileReader>();

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationPipelineBehavior<,>));

            services.AddOptions();

            services.AddDbContext<TradeTipsContext>(options =>
            {
                options.UseSqlServer(DEFAULT_DATABASE_CONNECTIONSTRING);
            });

            ConfigureSecurity(services);
            ConfigureSwagger(services);
        }

        /// <summary>
        /// Configure the request pipeline.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="loggerFactory"></param>
        public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory)
        {
            this.ConfigureLogging(loggerFactory);

            app.UseMiddleware<ErrorHandlingMiddleware>();

            app.UseCors(builder =>
                builder
                    .AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod());

            app.UseMvc();

            // Enable middleware to serve generated Swagger as a JSON endpoint
            app.UseSwagger(c =>
            {
                c.RouteTemplate = "swagger/{documentName}/swagger.json";
            });

            // Enable middleware to serve swagger-ui assets(HTML, JS, CSS etc.)
            app.UseSwaggerUI(x =>
            {
                x.SwaggerEndpoint("/swagger/v1/swagger.json", "TradeTips API V1");
            });

            using (var scope = app.ApplicationServices.CreateScope())
            {
                scope.ServiceProvider.GetRequiredService<TradeTipsContext>().Database.EnsureCreated();
            }
        }

        #endregion

        #region Helper methods

        /// <summary>
        /// Add Swagger support
        /// </summary>
        /// <param name="services"></param>
        private void ConfigureSwagger(IServiceCollection services)
        {
            // Inject an implementation of ISwaggerProvider with defaulted settings applied
            services.AddSwaggerGen(x =>
            {
                x.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please insert JWT with Bearer into field",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    BearerFormat = "JWT"
                });

                x.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {   new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
                    },
                    new string[] {}}
                });

                x.EnableAnnotations();
                x.SwaggerDoc("v1", new OpenApiInfo { Title = "Trade Tips", Version = "v1" });
                x.CustomSchemaIds(y => y.FullName);
                x.DocInclusionPredicate((version, apiDescription) => true);
                x.TagActionsBy(y => new List<string>()
                {
                    y.GroupName
                });
            });
        }

        /// <summary>
        /// Add JWT Support
        /// </summary>
        /// <param name="services"></param>
        private void ConfigureSecurity(IServiceCollection services)
        {
            services.AddCors();

            var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("somethinglongerforthisdumbalgorithmisrequired"));
            var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
            var issuer = "issuer";
            var audience = "audience";

            services.Configure<JwtIssuerOptions>(options =>
            {
                options.Issuer = issuer;
                options.Audience = audience;
                options.SigningCredentials = signingCredentials;
            });

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signingCredentials.Key,
                ValidateIssuer = true,
                ValidIssuer = issuer,
                ValidateAudience = true,
                ValidAudience = audience,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = tokenValidationParameters;
                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = (context) =>
                        {
                            var token = context.HttpContext.Request.Headers["Authorization"];
                            if (token.Count > 0 && token[0].StartsWith("Token ", StringComparison.OrdinalIgnoreCase))
                            {
                                context.Token = token[0].Substring("Token ".Length).Trim();
                            }

                            return Task.CompletedTask;
                        }
                    };

                });

        }

        /// <summary>
        /// Use Serilog logging
        /// </summary>
        /// <param name="loggerFactory"></param>
        private void ConfigureLogging(ILoggerFactory loggerFactory)
        {
            var log = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .Enrich.FromLogContext()
                .WriteTo.Console(outputTemplate: "{Timestamp:HH:mm:ss} [{Level}] {SourceContext} {Message}{NewLine}{Exception}", theme: AnsiConsoleTheme.Code)
                .CreateLogger();

            loggerFactory.AddSerilog(log);
            Log.Logger = log;
        }

        #endregion
    }
}