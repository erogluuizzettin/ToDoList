using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoList.Model;

namespace ToDoList.DAL.Concrete.EntityFramework.Mappings
{
    public class UserMapping : EntityTypeConfiguration<User>
    {
        public UserMapping()
        {
            Property(a => a.FirstName).HasMaxLength(30);
            Property(a => a.LastName).HasMaxLength(20);
            Property(a => a.EMail).HasMaxLength(255);
            Property(a => a.Password).HasMaxLength(20);
        }
    }
}
