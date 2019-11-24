using Ninject;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using ToDoList.BLL.Abstract;
using ToDoList.BLL.Concrete;

namespace ToDoList.UI.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private IKernel container;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            ConfigureContainer();
            ComposeObjects();
            Current.MainWindow.Show();
        }

        private void ConfigureContainer()
        {
            this.container = new StandardKernel();
            container.Bind<IUserService>().To<UserService>().InTransientScope();
            container.Bind<IUserRoleService>().To<UserRoleService>().InTransientScope();
            container.Bind<ITaskService>().To<TaskService>().InTransientScope();
            container.Bind<IBoardService>().To<BoardService>().InTransientScope();
            container.Bind<IStatusService>().To<StatusService>().InTransientScope();
        }

        private void ComposeObjects()
        {
            Current.MainWindow = this.container.Get<MainWindow>();
        }
    }
}
