using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrossCutting.IoC;
using Domain.Common.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using SimpleInjector;
using SimpleInjector.Integration.AspNetCore.Mvc;
using SimpleInjector.Lifestyles;

namespace HubbecAPI
{
  public class Startup
  {

    private readonly Container _container = new Container();

    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
      services.AddCors();
      services.AddControllers();
      
      IntegrateSimpleInjector(services);

      // configure strongly typed settings objects
      var appSettingsSection = Configuration.GetSection("AppSettings");
      services.Configure<AppSettings>(appSettingsSection);

      // configure jwt authentication
      var appSettings = appSettingsSection.Get<AppSettings>();
      var key = Encoding.ASCII.GetBytes(appSettings.Secret);
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

      // configure DI for application services
      //services.AddScoped<IUserService, UserService>();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      InitializeContainer();

      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }

      app.UseRouting();

      // global cors policy
      app.UseCors(x => x
          .AllowAnyOrigin()
          .AllowAnyMethod()
          .AllowAnyHeader());

      app.UseAuthentication();
      app.UseAuthorization();

      app.UseEndpoints(endpoints => {
        endpoints.MapControllers();
      });

      app.UseHttpsRedirection();
    }

    private void IntegrateSimpleInjector(IServiceCollection services)
    {
      _container.Options.DefaultScopedLifestyle = new AsyncScopedLifestyle();

      services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

      services.AddSingleton<IControllerActivator>(
          new SimpleInjectorControllerActivator(_container));
      services.AddSingleton<IViewComponentActivator>(
          new SimpleInjectorViewComponentActivator(_container));

      services.AddSimpleInjector(_container, o =>
      {
        o.AddAspNetCore().AddControllerActivation();
        o.AddAspNetCore().AddViewComponentActivation();
        o.AutoCrossWireFrameworkComponents = true;
      });
      services.UseSimpleInjectorAspNetRequestScoping(_container);
    }

    private void InitializeContainer()
    {
      // Add application services. For instance:
      _container.RegisterDependenciesScoped();
    }
  }
}
