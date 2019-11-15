using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ToDoList.UI.WPF.Models;

namespace ToDoList.UI.WPF
{
    /// <summary>
    /// Interaction logic for SearchWindow.xaml
    /// </summary>
    public partial class SearchWindow : Window
    {
        List<TaskModel> tasks;
        public SearchWindow(out List<TaskModel> taskModels)
        {
            InitializeComponent();
            taskModels = tasks;
        }
    }
}
