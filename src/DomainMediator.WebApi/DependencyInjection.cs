using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Text;
using DomainMediator.WebApi.Jwt;
using DomainMediator.WebApi.Middlewares;
using DomainMediator.WebApi.WebRequests;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace DomainMediator.WebApi;

[ExcludeFromCodeCoverage]
public static class DependencyInjection
{
    public static void ConfigureDomainWebApiProgram(this WebApplicationBuilder builder, string apiVersion,
        bool requireAuthenticatedUser = true)
    {
        var configuration = builder.Configuration;
        var services = builder.Services;

        services.AddWebApiDependencies(configuration);

        services.AddEndpointsApiExplorer();

        ConfigureCors();

        ConfigureSwagger();

        ConfigureAuthServices();

        #region Local methods

        void ConfigureCors()
        {
            var enabledUrls = configuration["CorsOrigins"]?.Split(";") ?? [];
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                    policy => { policy.WithOrigins(enabledUrls).AllowAnyHeader().AllowAnyMethod(); });
            });
        }

        void ConfigureSwagger()
        {
            builder.Services.AddSwaggerGen(option =>
            {
                option.SwaggerDoc(apiVersion,
                    new OpenApiInfo { Title = builder.Environment.ApplicationName, Version = apiVersion });
                option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please, insert a valid token:",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });
                option.AddSecurityRequirement(new OpenApiSecurityRequirement
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
                        Array.Empty<string>()
                    }
                });
            });
        }

        void ConfigureAuthServices()
        {
            var jwtConfiguration = new JwtConfigurationImp(configuration);
            if (jwtConfiguration.Key != null)
            {
                services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                }).AddJwtBearer(o =>
                {
                    o.RequireHttpsMetadata = false;
                    o.SaveToken = false;
                    o.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidIssuer = jwtConfiguration.Issuer,
                        ValidAudience = jwtConfiguration.Audience,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfiguration.Key)),
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero
                    };
                });

                if (requireAuthenticatedUser)
                    services.AddAuthorizationBuilder()
                        .SetFallbackPolicy(new AuthorizationPolicyBuilder()
                            .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                            .RequireAuthenticatedUser()
                            .Build());
                else
                    services.AddAuthorization();
            }
            else
            {
                Console.WriteLine("Proceeding without authentication...");
            }
        }

        #endregion
    }

    public static void AddWebApiDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddArchitectureDependencies();

        services.AddScoped<DomainApi, DomainApiImp>();

        services.AddSingleton<IJwtConfiguration>(new JwtConfigurationImp(configuration));
        services.AddScoped<IJwtService, JwtServiceImp>();

        services.RegisterAssemblyForAllPackages(Assembly.GetExecutingAssembly());
    }

    public static WebApplication BuildDomainWebApiProgram(this WebApplicationBuilder builder,
        bool addAuthorization = true)
    {
        var app = builder.Build();

        if (!app.Environment.IsProduction())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseCors();

        if (addAuthorization)
        {
            app.UseAuthentication();
            app.UseAuthorization();
        }

        app.UseCorrelationIdMiddleware();
        app.UseErrorMiddleware();

        return app;
    }
}
