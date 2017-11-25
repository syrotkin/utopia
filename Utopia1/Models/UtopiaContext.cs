using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Utopia1.Models
{
    public class UtopiaContext : DbContext
    {
        public UtopiaContext() : base("UtopiaContext")
        {
        }

        public DbSet<Group> Groups { get; set; }

        public DbSet<Room> Rooms { get; set; }
    }
}