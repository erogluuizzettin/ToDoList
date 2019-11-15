using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using ToDoList.Model;

namespace ToDoList.DAL.Concrete.EntityFramework.Mappings
{
    public class TaskMapping : EntityTypeConfiguration<Task>
    {
        public TaskMapping()
        {
            Property(a => a.Name).HasMaxLength(50);
            Property(a => a.Description).HasMaxLength(500);
        }
    }
}
