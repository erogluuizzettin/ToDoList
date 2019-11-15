using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ToDoList.Model;

namespace ToDoList.BLL.Abstract
{
    public interface ITaskService : IBaseService<Task>
    {
        void Delete(List<Task> tasks);
        List<Task> GetTasksByBoardID(int boardID);
        void TaskFinish(Task task);
    }
}
