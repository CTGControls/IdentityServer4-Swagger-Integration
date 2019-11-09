using System.Collections.Generic;
using System.Linq;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using Newtonsoft.Json;


using Microsoft.OpenApi.Models;


namespace Api.Swashbuckle
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers(option => option.EnableEndpointRouting = false)
                .AddNewtonsoftJson();

            services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
                .AddIdentityServerAuthentication(options =>
                {
                    options.Authority = "https://localhost:5011"; // auth server base endpoint (will use to search for disco doc)
                    options.ApiName = "demo_api"; // required audience of access tokens
                    options.RequireHttpsMetadata = false; // dev only!
                });

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "Protected API", Version = "v1"});

                options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.OAuth2,
                    Flows = new OpenApiOAuthFlows
                    {
                        Implicit = new OpenApiOAuthFlow
                        {
                            AuthorizationUrl = new System.Uri("https://localhost:5011/connect/authorize"),
                            Scopes = new Dictionary<string, string>
                            {
                                { "ffmapi", "Implicit Flow for the FFM API" }
                            }
                        }
                    }
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "oauth2" }
                        },
                        new[] { "ffmapi" }
                    }
                });
            });
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseDeveloperExceptionPage();

            app.UseAuthentication();
            

            
            app.UseRouting();
            app.UseAuthorization();

            // Swagger JSON Doc
            app.UseSwagger();

            // Swagger UI
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                //options.RoutePrefix = string.Empty;

                options.OAuthClientId("ffmapi_Implicit");
                options.OAuthAppName("Implicit Flow for the FFM API"); // presentation purposes only
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

        }
    }

}
