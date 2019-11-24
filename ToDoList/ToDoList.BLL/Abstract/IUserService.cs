using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoList.Model;

namespace ToDoList.BLL.Abstract
{
    public interface IUserService : IBaseService<User>
    {
        User GetUserByLogin(string email, string password);
        User GetUserByActivationCode(string activationCode);
        bool GetUserByIsActive(User user);
        void DeleteUsersInActive();
    }
}
