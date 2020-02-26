using Domain.Common.Enumerations;
using System;
using System.Collections.Generic;

namespace Domain.DataTransferObjects
{
  public class User
  {
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string LastName { get; set; }
    public DocumentType DocumentType { get; set; }
    public string DocumentNumber { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }

    public IEnumerable<User> Contacts { get; set; }
    public IEnumerable<User> Contact { get; set; }
  }
}
