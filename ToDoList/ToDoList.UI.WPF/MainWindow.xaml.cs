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
using ToDoList.BLL.Concrete;
using ToDoList.Model;
using ToDoList.UI.WPF.Models;

namespace ToDoList.UI.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        UserService _userService;
        LoginModel loginModel;

        public MainWindow()
        {
            InitializeComponent();
            _userService = new UserService();
        }

        private void BtnRegister_Click(object sender, RoutedEventArgs e)
        {
            string password = pwdRegisterPassword.Password.ToString();

            User currentUser = new User();
            currentUser.FirstName = txtRegisterFirstName.Text;
            currentUser.LastName = txtRegisterLastName.Text;
            currentUser.EMail = txtRegisterEMail.Text;
            currentUser.Password = password;

            try
            {
                _userService.Insert(currentUser);
                ShowGrid(gLogin);
                txtLoginEMail.Text = currentUser.EMail;
                pwdLoginPassword.Password = currentUser.Password;
                MessageBox.Show("Registration Successful. You are redirected to the login page");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void BtnGoToLogin_Click(object sender, RoutedEventArgs e)
        {
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
                ToDoListWindow window = new ToDoListWindow(currentUser);
                window.ShowDialog();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            RememberMe();
        }

        private void BtnGoToRegister_Click(object sender, RoutedEventArgs e)
        {
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
    }
}
