using Domain.Common.Enumerations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Entities
{
  [Table("Contact", Schema = "account")]
  public class Contact
  {
    [Key, ForeignKey("FirstUser"), Column(Order = 1)]
    public Guid FirstUserId { get; set; }

    [Key, ForeignKey("SecondUser"), Column(Order = 2)]
    public Guid SecondUserId { get; set; }

    public virtual User FirstUser { get; set; }
    public virtual User SecondUser { get; set; }
    public RelationType RelationType { get; set; }
  }
}
