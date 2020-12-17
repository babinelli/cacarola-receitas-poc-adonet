using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace DAL
{
    public class RecipeContext : DbContext
    {

        public RecipeContext()
            : base ("RecipeContext")
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Desativar a pluralização das tabelas
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }

        // Quantos DbSet quanto o número de tabelas (classes)
        public DbSet<User> User { get; set; }
        public DbSet<Department> Department { get; set; }


    }
}
