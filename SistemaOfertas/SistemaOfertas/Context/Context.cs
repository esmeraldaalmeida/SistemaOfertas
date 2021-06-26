using SistemaOfertas.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace SistemaOfertas.Context
{
    public class Context : DbContext
    {
        public DbSet<Cliente> Cliente { get; set; }
        public DbSet<Oferta> Oferta { get; set; }
        public DbSet<Produto> Produto { get; set; }
        public DbSet<Status> Status { get; set; }
    }
}