namespace DAL.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<DAL.RecipeContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(DAL.RecipeContext context)
        {
            //  This method will be called after migrating to the latest version.
            //User user = new User
            //{
            //    Name = "Bárbara",
            //    LastName = "Coscolim",
            //    Email = "bafantinelli@hotmail.com",
            //    Username = "bafantinelli",
            //    Password = "01234Oi*",
            //    Admin = true
            //};

            //context.User.Add(user);

            //Department department = new Department
            //{
            //    DepartmentName = "TI"
            //};

            //context.Department.Add(department);

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method
            //  to avoid creating duplicate seed data.
        }
    }
}
