using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess
{
  public class HubbecContext : DbContext
  {
    private const string _connectionString = @"Server=.;Database=hubbec;Trusted_Connection=True;";
    public HubbecContext() : base() { }
    public HubbecContext(DbContextOptions<HubbecContext> options) : base(options)
    {

    }

    public DbSet<User> Users { get; set; }
    public DbSet<Contact> Contacts { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
      optionsBuilder.UseSqlServer(_connectionString);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      modelBuilder.Entity<Contact>()
        .HasKey(c => new { c.FirstUserId, c.SecondUserId });
      modelBuilder.Entity<Contact>()
        .HasOne(c => c.FirstUser)
        .WithMany(u => u.Contacts)
        .HasForeignKey(c => c.FirstUserId);
      modelBuilder.Entity<Contact>()
        .HasOne(c => c.SecondUser)
        .WithMany(u => u.Contact)
        .HasForeignKey(c => c.SecondUserId);

      #region ModuleSeed
      //modelBuilder.Entity<Module>().HasData(
      //  new Module
      //  {
      //    Id = Guid.Parse("8edcd064-fb62-481e-b71e-6b681e21d86b"),
      //    Order = 1,
      //    Label = "JEANS",
      //    Url = "#",
      //  },
      //  new Module
      //  {
      //    Order = 1,
      //    Label = "Holgados",
      //    Url = "#",
      //    ParentModuleId = Guid.Parse("8edcd064-fb62-481e-b71e-6b681e21d86b")
      //  },
      //  new Module
      //  {
      //    Order = 2,
      //    Label = "Apretados",
      //    Url = "#",
      //    ParentModuleId = Guid.Parse("8edcd064-fb62-481e-b71e-6b681e21d86b")
      //  },
      //  new Module
      //  {
      //    Order = 2,
      //    Label = "DENIM",
      //    Url = "#"
      //  },
      //  new Module
      //  {
      //    Order = 3,
      //    Label = "ROPA",
      //    Url = "#"
      //  },
      //  new Module
      //  {
      //    Order = 4,
      //    Label = "ACCESORIOS",
      //    Url = "#"
      //  },
      //  new Module
      //  {
      //    Order = 5,
      //    Label = "BÁSICOS INFALTABLES",
      //    Url = "#"
      //  },
      //  new Module
      //  {
      //    Order = 6,
      //    Label = "NOVEDADES",
      //    Url = "#"
      //  },
      //  new Module
      //  {
      //    Order = 7,
      //    Label = "REBAJAS",
      //    Url = "#"
      //  }
      //);
      #endregion              
    }
  }
}
