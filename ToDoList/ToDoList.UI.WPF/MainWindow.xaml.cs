using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ToDoList.BLL.Abstract;
using ToDoList.BLL.Concrete;
using ToDoList.Model;
using ToDoList.UI.WPF.Helpers;
using ToDoList.UI.WPF.Models;
using ToDoList.UI.WPF.Models.Ninject;

namespace ToDoList.UI.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        LoginModel loginModel;
        IUserService _userService;
        IUserRoleService _userRoleService;
        ITaskService _taskService;
        IBoardService _boardService;
        IStatusService _statusService;
        BoardTaskStatusNinject _currentNinjects;


        public MainWindow(IUserService userService, IUserRoleService userRoleService, ITaskService taskService, IBoardService boardService, IStatusService statusService)
        {
            InitializeComponent();
            _currentNinjects = new BoardTaskStatusNinject();
            _currentNinjects.BoardService = boardService;
            _currentNinjects.TaskService = taskService;
            _currentNinjects.StatusService = statusService;
            _userService = userService;
            _userRoleService = userRoleService;
            _taskService = taskService;
            _boardService = boardService;
            _statusService = statusService;
        }
        
        private void BtnRegister_Click(object sender, RoutedEventArgs e)
        {
            string password = pwdRegisterPassword.Password.ToString();

            Random random = new Random();
            User currentUser = new User();
            currentUser.FirstName = txtRegisterFirstName.Text;
            currentUser.LastName = txtRegisterLastName.Text;
            currentUser.EMail = txtRegisterEMail.Text;
            currentUser.Password = password;
            currentUser.ActivationCode = random.Next(1001, 9999).ToString();
            currentUser.IsActive = false;

            try
            {
                bool result = _userService.Insert(currentUser);
                if (result)
                {
                    result = EMailHelper.SendEMail(currentUser.EMail, currentUser.ActivationCode, currentUser.FirstName, currentUser.LastName);
                    if (result)
                    {
                        MessageBox.Show(string.Format(currentUser.FirstName + " " + currentUser.LastName + "\nCheck your mail inbox for activation code"));
                    }
                    else
                    {
                        MessageBox.Show("Could not send activation mail");
                        _userService.Delete(currentUser);
                        return;
                    }
                    lblActivation.Visibility = Visibility.Visible;
                    txtActivationCode.Visibility = Visibility.Visible;
                }

                ShowGrid(gLogin);
                txtLoginEMail.Text = currentUser.EMail;
                pwdLoginPassword.Password = currentUser.Password;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void BtnGoToLogin_Click(object sender, RoutedEventArgs e)
        {
            this.Title = "To-Do List Login Page";
            if (txtActivationCode.Visibility == Visibility.Visible)
            {
                lblActivation.Visibility = Visibility.Hidden;
                txtActivationCode.Visibility = Visibility.Hidden;
                txtLoginEMail.Text = "";
                pwdLoginPassword.Password = "";
                chkRemember.IsChecked = false;
            }
            ShowGrid(gLogin);
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            string password = pwdLoginPassword.Password.ToString();
            loginModel = new LoginModel();
            loginModel.EMail = txtLoginEMail.Text;
            loginModel.Password = password;

            try
            {
                User currentUser = _userService.GetUserByLogin(loginModel.EMail, loginModel.Password);
                if (currentUser == null)
                {
                    _userService.Delete(currentUser);
                    return;
                }
                else
                {
                    if (txtActivationCode.Visibility == Visibility.Visible)
                    {
                        if (string.IsNullOrWhiteSpace(txtActivationCode.Text))
                        {
                            MessageBox.Show("You did not enter the activation code");
                            return;
                        }
                        User activeUser = _userService.GetUserByActivationCode(txtActivationCode.Text);
                        if (activeUser == null)
                        {
                            MessageBox.Show("Make sure you enter the Activation Code correctly");
                            return;
                        }
                        if (currentUser == activeUser && !currentUser.IsActive)
                        {
                            MessageBox.Show("Your membership has been activated");
                            currentUser.IsActive = true;
                            _userService.Update(currentUser);
                            OpenToDoListPage(currentUser);
                        }
                        else
                        {

                            MessageBox.Show("User not found, please register");
                            ShowGrid(gRegistered);
                            return;
                        }
                    }
                    DeleteUsersInActive();
                    OpenToDoListPage(currentUser);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            RememberMe();
        }

        void OpenToDoListPage(User currentUser)
        {
            ToDoListWindow window = new ToDoListWindow(currentUser, _currentNinjects);
            window.ShowDialog();
            this.Close();
        }

        private void BtnGoToRegister_Click(object sender, RoutedEventArgs e)
        {
            txtRegisterEMail.Text = "";
            txtRegisterFirstName.Text = "";
            txtRegisterLastName.Text = "";
            pwdRegisterPassword.Password = "";
            this.Title = "To-Do List Register Page";
            DeleteUsersInActive();
            ShowGrid(gRegistered);
        }

        void ShowGrid(Grid grid)
        {
            gRegistered.Visibility = Visibility.Hidden;
            gLogin.Visibility = Visibility.Hidden;
            grid.Visibility = Visibility.Visible;
        }

        private void GLogin_Loaded(object sender, RoutedEventArgs e)
        {
            if (File.Exists("ToDoList.psg"))
            {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream fs = new FileStream("ToDoList.psg", FileMode.Open, FileAccess.Read);
                loginModel = bf.Deserialize(fs) as LoginModel;
                fs.Close();

                txtLoginEMail.Text = loginModel.EMail;
                pwdLoginPassword.Password = loginModel.Password;
                chkRemember.IsChecked = true;
            }
        }


        void RememberMe()
        {
            if ((bool)chkRemember.IsChecked)
            {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream fs = new FileStream("ToDoList.psg", FileMode.Create, FileAccess.Write);
                bf.Serialize(fs, loginModel);
                fs.Close();
            }
            else
            {
                if (File.Exists("ToDoList.psg"))
                {
                    File.Delete("ToDoList.psg");
                }
            }
        }

        private void GRegistered_Loaded(object sender, RoutedEventArgs e)
        {
            DeleteUsersInActive();
        }

        void DeleteUsersInActive()
        {
            try
            {
                _userService.DeleteUsersInActive();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DeleteUsersInActive();
        }
    }
}
