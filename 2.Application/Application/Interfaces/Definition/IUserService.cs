using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Definition
{
  public interface IUserService : IApplicationService<User>
  {
    User Authenticate(string email, string password);
    Task<Domain.DataTransferObjects.User> GetUserWithContacts(Guid userId);
    Task AddContact(Guid userId, Guid contactId);
  }
}
