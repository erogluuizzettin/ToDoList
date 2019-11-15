using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoList.Core.DAL.EntityFramework;
using ToDoList.DAL.Abstract;
using ToDoList.Model;

namespace ToDoList.DAL.Concrete.EntityFramework.DAL
{
    public class EFUserDAL : EFRepositoryBase<User, ToDoListDbContext>, IUserDAL
    {

    }
}
