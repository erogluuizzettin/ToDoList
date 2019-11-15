using System;
using System.Collections.Generic;
using System.Text;
using ToDoList.Core.DAL;
using ToDoList.Model;

namespace ToDoList.DAL.Abstract
{
    public interface ITaskDAL : IRepository<Task>
    {

    }
}
