using Application.Interfaces.Definition;
using Domain.Common.Configuration;
using Domain.Common.Extensions;
using Domain.Entities;
using Domain.Repository.Account;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Definition { 
  public class UserService : ApplicationService<User>,  IUserService
  {
    // users hardcoded for simplicity, store in a db with hashed passwords in production applications

    private readonly AppSettings _appSettings;

    public UserService(IOptions<AppSettings> appSettings, IUserRepository repository) : base(repository)
    {
      _appSettings = appSettings.Value;
    }

    public async Task AddContact(Guid userId, Guid contactId)
    {
      var user = await Queryable()
        .Include("Contacts.SecondUser")
        .FirstOrDefaultAsync(x => x.Id == userId);

      user.Contacts.Add(new Contact { FirstUserId = userId, SecondUserId = contactId });

    }

    public User Authenticate(string email, string password)
    {
      password = password.EncryptMD5();
      var user = Queryable(true).SingleOrDefault(x => x.Email == email && x.Password == password);

      // return null if user not found
      if (user == null)
        return null;

      // authentication successful so generate jwt token
      var tokenHandler = new JwtSecurityTokenHandler();
      var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
      var tokenDescriptor = new SecurityTokenDescriptor
      {
        Subject = new ClaimsIdentity(new Claim[]
          {
            new Claim(ClaimTypes.Name, user.Id.ToString())
          }),
        Expires = DateTime.UtcNow.AddDays(7),
        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
      };
      var token = tokenHandler.CreateToken(tokenDescriptor);
      user.Token = tokenHandler.WriteToken(token);

      //await Update(user);

      return user.WithoutPassword();
    }

    public async Task<Domain.Common.OperationHandling.OperationResult<User>> Register(User user)
    {
      if (Queryable(true).Any(x => x.Email == user.Email && x.DocumentNumber == user.DocumentNumber && x.DocumentType == user.DocumentType))
        return new Domain.Common.OperationHandling.OperationResult<User>();

      return await Create(user);
    }

    public async Task<Domain.DataTransferObjects.User> GetUserWithContacts(Guid userId)
    {
      var user = await Queryable(true)
        .Include("Contacts.SecondUser")
        .FirstOrDefaultAsync(x => x.Id == userId);

      if (user == null) return null;

      var data = new Domain.DataTransferObjects.User
      {
        Id = user.Id,
        Email = user.Email,
        Phone = user.Phone,
        Name = user.Name,
        LastName = user.LastName,
        DocumentType = user.DocumentType,
        DocumentNumber = user.DocumentNumber,
        Contacts = user.Contact.Select(x => new Domain.DataTransferObjects.User
        {
          Id = x.SecondUser.Id,
          Name = x.SecondUser.Name,
          LastName = x.SecondUser.LastName,
          Email = x.SecondUser.Email,
          Phone = x.SecondUser.Phone
        })
      };

      return data;
    }
  }

}

