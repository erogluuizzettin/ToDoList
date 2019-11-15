using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ToDoList.BLL.Concrete;
using ToDoList.Model;

namespace ToDoList.UI.WPF
{
    /// <summary>
    /// Interaction logic for AddTaskWindow.xaml
    /// </summary>
    public partial class AddTaskWindow : Window
    {
        TaskService _taskService;
        BoardService _boardService;
        Board _currentBoard;
        User _currentUser;

        public AddTaskWindow(User user)
        {
            InitializeComponent();
            _taskService = new TaskService();
            _boardService = new BoardService();
            _currentUser = user;
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (cmbTaskList.SelectedIndex == -1)
                {
                    MessageBox.Show("Please select a Task List");
                    return;
                }

                Task currentTask = new Task();
                currentTask.Name = txtTitle.Text;
                currentTask.Description = txtDescription.Text;
                currentTask.StartDate = dtpStartTime.SelectedDate.Value;
                currentTask.Deadline = dtpDeadLine.SelectedDate.Value;
                currentTask.StatusID = 1;
                currentTask.BoardID = _currentBoard.ID;

                _taskService.Insert(currentTask);
                MessageBox.Show("The task was successfully added");
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            BindCombobox();
        }

        public List<Board> Brd { get; set; }
        void BindCombobox()
        {
            var item = _boardService.GetBoardsByUser(_currentUser);
            Brd = item;
            DataContext = Brd;
        }

        private void CmbTaskList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _currentBoard = cmbTaskList.SelectedItem as Board;
        }
    }
}
