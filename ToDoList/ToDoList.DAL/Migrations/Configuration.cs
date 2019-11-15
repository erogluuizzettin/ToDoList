namespace ToDoList.DAL.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using ToDoList.Model;

    internal sealed class Configuration : DbMigrationsConfiguration<ToDoList.DAL.Concrete.EntityFramework.ToDoListDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(ToDoList.DAL.Concrete.EntityFramework.ToDoListDbContext context)
        {
            List<UserRole> userRoles = new List<UserRole>();
            userRoles.AddRange(new[] {
                new UserRole { RoleName="Admin" },
                new UserRole { RoleName="Standart" }
            });

            context.UserRoles.AddRange(userRoles);
            context.SaveChanges();

            context.Users.Add(new User()
            {
                FirstName = "izzettin",
                LastName = "eroğlu",
                EMail = "erogluuizzettin@gmail.com",
                Password = "Eroglu123",
                UserRole = userRoles[0]
            });
            context.SaveChanges();

            List<Status> statuses = new List<Status>();
            statuses.Add(new Status { Name = "doing" });
            statuses.Add(new Status { Name = "done" });
            context.Statuses.AddRange(statuses);
            context.SaveChanges();
        }
    }
}
