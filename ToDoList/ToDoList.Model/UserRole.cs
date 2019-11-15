using System;
using System.Collections.Generic;
using System.Text;
using ToDoList.Core.Entity;

namespace ToDoList.Model
{
    public class UserRole : BaseEntity
    {
        public UserRole()
        {
            Users = new HashSet<User>();
        }

        public string RoleName { get; set; }

        //Foreign Keys

        //Nav Props
        public virtual ICollection<User> Users { get; set; }
    }
}
