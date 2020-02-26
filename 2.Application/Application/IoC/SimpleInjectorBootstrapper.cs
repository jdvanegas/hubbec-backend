using Application.Interfaces.Definition;
using Application.Services.Definition;
using SimpleInjector;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.IoC
{
  public static class SimpleInjectorBootstrapper
  {
    public static void RegisterServices(this Container container)
    {
      container.Register<IUserService, UserService>();
    }

    public static void RegisterServicesScoped(this Container container)
    {
      container.Register<IUserService, UserService>(Lifestyle.Scoped);
    }
  }
}
