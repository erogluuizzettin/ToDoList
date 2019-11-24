using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoList.BLL.Abstract;

namespace ToDoList.UI.WPF.Models.Ninject
{
    public class BoardTaskStatusNinject
    {
        public ITaskService TaskService { get; set; }
        public IBoardService BoardService { get; set; }
        public IStatusService StatusService { get; set; }
    }
}
