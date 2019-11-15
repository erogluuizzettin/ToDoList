using System;
using System.Collections.Generic;
using System.Text;
using ToDoList.Core.Entity;

namespace ToDoList.Model
{
    public class User : BaseEntity
    {
        public User()
        {
            Boards = new HashSet<Board>();
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EMail { get; set; }
        public string Password { get; set; }

        //Foreign Keys
        public int UserRoleID { get; set; }

        //Nav Props
        public virtual UserRole UserRole { get; set; }
        public virtual ICollection<Board> Boards { get; set; }
    }
}
