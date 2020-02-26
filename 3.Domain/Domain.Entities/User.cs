using Domain.Common.Enumerations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Entities
{
  [Table("User", Schema = "account")]
  public class User
  {
    public User()
    {
      Contact = new HashSet<Contact>();
      Contacts = new HashSet<Contact>();
    }

    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [StringLength(255)]
    public string Name { get; set; }
    [StringLength(255)]
    public string LastName { get; set; }
    public DocumentType DocumentType { get; set; }

    [StringLength(125)]
    public string DocumentNumber { get; set; }
    [StringLength(255)]
    public string Phone { get; set; }
    [StringLength(255)]
    public string Email { get; set; }
    public string Password { get; set; }
    public string Token { get; set; }
    public ICollection<Contact> Contact { get; set; }

    public ICollection<Contact> Contacts { get; set; }
    public User WithoutPassword()
    {
      Password = null;
      return this;
    }
  }
}
