using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoList.DAL.Abstract;
using ToDoList.DAL.Concrete.EntityFramework.DAL;

namespace ToDoList.BLL.IOC.Ninject
{
    public class DALModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IUserDAL>().To<EFUserDAL>();
            Bind<IUserRoleDAL>().To<EFUserRoleDAL>();
            Bind<ITaskDAL>().To<EFTaskDAL>();
            Bind<IBoardDAL>().To<EFBoardDAL>();
            Bind<IStatusDAL>().To<EFStatusDAL>();
        }
    }
}
