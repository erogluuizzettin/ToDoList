using System;
using System.Collections.Generic;
using System.Text;
using ToDoList.Core.Entity;

namespace ToDoList.Model
{
    public class Task : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime Deadline { get; set; }
        public DateTime StartDate { get; set; }

        //Foreign Keys
        public int StatusID { get; set; }
        public int BoardID { get; set; }

        //Nav Props
        public virtual Status Status { get; set; }
        public virtual Board Board { get; set; }
    }
}
