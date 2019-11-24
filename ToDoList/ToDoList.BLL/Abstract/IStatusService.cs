using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoList.Model;

namespace ToDoList.BLL.Abstract
{
    public interface IStatusService : IBaseService<Status>
    {
        Status GetStatusByName(string statusName);
    }
}
