using System.IO;
using System.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using MyPrimerWebApi.Data;
using MyPrimerWebApi.helpers;
using MyPrimerWebApi.Models;
using MyPrimerWebApi.Services;
using MyPrimerWebApi.Utils;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.OpenApi.Models;
using System.Reflection;

//Esto me sirve para esablecer informacion por convecion para la descripcion de los endpoins del webApi en Swagger
[assembly: ApiConventionType(typeof(DefaultApiConventions))]
namespace MyPrimerWebApi
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
            services.AddScoped<IPrepDb, PrepDb>();

            services.AddControllers(options =>
            {
                options.Filters.Add(new MiFiltroDeException());
                options.Conventions.Add(new ApiExplorerGroupPerVersionConvention());
            })
            .AddNewtonsoftJson(
                options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                );

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddResponseCaching();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"])),
                    ClockSkew = TimeSpan.Zero,
                };
            });

            services.AddCors(options =>
            {
                options.AddPolicy("PermitirApiRequest", (builder) =>
                {
                    // Con esto habilito una polita de CORS para ser utilizada en algun controlador 
                    builder.WithOrigins("https://www.apirequest.io").AllowAnyMethod().WithHeaders("*");
                });
            });

            services.AddScoped<MiFiltroDeAccion>();

            services.AddAutoMapper(typeof(Startup));

            services.AddIdentity<ApplicationUser, IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

            services.AddDataProtection();

            services.AddScoped<IHashService, HashService>();
            services.AddScoped<HateoasAuthorFilterAttribute>();
            services.AddScoped<HateoasAuthorsFilterAttribute>();
            services.AddScoped<GeneradorEnlaces>();
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();

            services.AddSwaggerGen(config =>
            {
                config.SwaggerDoc("v1", new OpenApiInfo()
                {
                    Version = "v1",
                    Title = "Mi Web API",
                    Description = "Esta es una descripcion del web API",
                    TermsOfService = new UriBuilder("http://nelsonmarroblog.com").Uri,
                    License = new OpenApiLicense() { Name = "MIT", Url = new UriBuilder("https://opensource.org/licenses/MIT").Uri },
                    Contact = new OpenApiContact() { Name = "Nelson Marro", Email = "nelsonmarro99@gmail.com", Url = new UriBuilder("http://nelsonmarroblog.com").Uri }
                });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

                config.IncludeXmlComments(xmlPath);

                config.SwaggerDoc("v2", new OpenApiInfo() { Title = "Mi Web API", Version = "v2" });
            });
        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IPrepDb prepDb)
        {
            app.UseSwagger();
            app.UseSwaggerUI(config =>
            {
                config.SwaggerEndpoint("/swagger/v1/swagger.json", "Mi API v1");
                config.SwaggerEndpoint("/swagger/v2/swagger.json", "Mi API v2");
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            prepDb.Initialize();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            // app.UseResponseCaching();

            // app.UseCors();
            //Con esto habilitos los CORS a nivel gobal para todos los endpoints
            // app.UseCors(builder => builder.WithOrigins("https://www.apirequest.io").AllowAnyMethod().WithHeaders("*"));// Con el asterisco indico que voy a permitir todas las cabeceras que se manden

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}