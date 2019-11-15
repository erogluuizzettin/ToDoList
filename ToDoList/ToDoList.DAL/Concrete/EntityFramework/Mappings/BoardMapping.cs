using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoList.Model;

namespace ToDoList.DAL.Concrete.EntityFramework.Mappings
{
    public class BoardMapping : EntityTypeConfiguration<Board>
    {
        public BoardMapping()
        {
            Property(a => a.Name).HasMaxLength(75);
        }
    }
}
