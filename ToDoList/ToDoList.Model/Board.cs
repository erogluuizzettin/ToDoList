using System;
using System.Collections.Generic;
using System.Text;
using ToDoList.Core.Entity;

namespace ToDoList.Model
{
    public class Board : BaseEntity
    {
        public Board()
        {
            Tasks = new HashSet<Task>();
        }

        public string Name { get; set; }

        //Foreign Keys
        public int UserID { get; set; }

        //Nav Props
        public virtual User User { get; set; }
        public virtual ICollection<Task> Tasks { get; set; }
    }
}
