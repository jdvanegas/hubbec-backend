using DataAccess.Account.Repository;
using Domain.Repository.Account;
using SimpleInjector;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Account.IoC
{
  public static class InjectorGeneralBootstrapper
  {
    public static void RegisterAccountRepository(this Container container)
    {
      container.Register<IUserRepository, UserRepository>();
    }

    public static void RegisterAccountRepositoryScoped(this Container container)
    {
      container.Register<IUserRepository, UserRepository>(Lifestyle.Scoped);
    }
  }
}
