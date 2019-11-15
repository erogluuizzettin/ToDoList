using System;
using System.Collections.Generic;
using System.Text;
using ToDoList.Core.Entity;

namespace ToDoList.Model
{
    public class Status : BaseEntity
    {
        public Status()
        {
            Tasks = new HashSet<Task>();
        }

        public string Name { get; set; }

        //Foreign Keys

        //Nav Props
        public virtual ICollection<Task> Tasks { get; set; }
    }
}
