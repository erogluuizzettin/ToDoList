using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Text;
using ToDoList.DAL.Concrete.EntityFramework.Mappings;
using ToDoList.Model;

namespace ToDoList.DAL.Concrete.EntityFramework
{
    public class ToDoListDbContext : DbContext
    {
        public ToDoListDbContext()
            :base("Server=.;Database=ToDoListDB;Trusted_Connection=True")
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();

            modelBuilder.Properties()
                .Where(a => a.PropertyType == typeof(string))
                .Configure(a => a.IsRequired());

            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.Entity<Board>()
                    .HasMany(a => a.Tasks)
                    .WithRequired(b => b.Board)
                    .WillCascadeOnDelete(false);

            modelBuilder.Configurations.Add(new UserMapping());
            modelBuilder.Configurations.Add(new UserRoleMapping());
            modelBuilder.Configurations.Add(new TaskMapping());
            modelBuilder.Configurations.Add(new StatusMapping());
            modelBuilder.Configurations.Add(new BoardMapping());
        }

        public DbSet<User> Users { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Task> Tasks { get; set; }
        public DbSet<Status> Statuses { get; set; }
        public DbSet<Board> Boards { get; set; }
    }
}
