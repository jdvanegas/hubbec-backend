using Domain.Entities;
using Domain.Repository.Account;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Account.Repository
{
  public class UserRepository : Repository<User>, IUserRepository
  {
  }
}
