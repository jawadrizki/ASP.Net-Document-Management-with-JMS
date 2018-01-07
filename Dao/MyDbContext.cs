using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DocumentsManagment.Dao
{
    public class MyDbContext : DbContext
    {
        public DbSet<Document> Documents { get; set; }
        public DbSet<Famille> Familles { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder dbContextOptionsBuilder)
        {

            dbContextOptionsBuilder.UseSqlServer("Server=JR-PC;Database=documentsdata;Trusted_Connection=True");
        }
    }
}
