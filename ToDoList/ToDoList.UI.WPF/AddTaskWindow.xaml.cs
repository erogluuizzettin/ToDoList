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
using ToDoList.BLL.Abstract;
using ToDoList.BLL.Concrete;
using ToDoList.Model;
using ToDoList.UI.WPF.Models;
using ToDoList.UI.WPF.Models.Ninject;

namespace ToDoList.UI.WPF
{
    /// <summary>
    /// Interaction logic for AddTaskWindow.xaml
    /// </summary>
    public partial class AddTaskWindow : Window
    {
        ITaskService _taskService;
        IBoardService _boardService;
        IStatusService _statusService;
        Board _currentBoard;
        User _currentUser;
        Task _currentTask;
        bool _taskDetails;
        bool _editTask;

        public AddTaskWindow(User user, bool taskDetails, Task task, bool editTask, BoardTaskStatusNinject ninject)
        {
            InitializeComponent();
            _boardService = ninject.BoardService;
            _statusService = ninject.StatusService;
            _taskService = ninject.TaskService;
            _currentUser = user;
            if (taskDetails)
            {
                btnAdd.Visibility = Visibility.Hidden;
                _taskDetails = taskDetails;
                _currentTask = task;
            }
            if (editTask)
            {
                btnAdd.Content = "Edit Task";
                rdbDoing.Visibility = Visibility.Visible;
                rdbDone.Visibility = Visibility.Visible;
                _editTask = editTask;
                _currentTask = task;
            }
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (!_editTask && !_taskDetails)
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
                    currentTask.Status = _statusService.GetStatusByName("doing");
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
            else
            {
                try
                {
                    if (cmbTaskList.SelectedIndex == -1)
                    {
                        MessageBox.Show("Please select a Task List");
                        return;
                    }
                    
                    _currentTask.Name = txtTitle.Text;
                    _currentTask.Description = txtDescription.Text;
                    _currentTask.StartDate = dtpStartTime.SelectedDate.Value;
                    _currentTask.Deadline = dtpDeadLine.SelectedDate.Value;
                    _currentTask.BoardID = _currentBoard.ID;

                    _taskService.Update(_currentTask);
                    MessageBox.Show("The task was successfully updated");
                    
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (_taskDetails)
            {
                this.Title = "Task Show Details Page";
                EditAndShowTask(false, false);
                cmbTaskList.DisplayMemberPath = _currentTask.Board.Name;
            }
            else if (_editTask)
            {
                this.Title = "Edit Task Page";
                EditAndShowTask(true, true);
                BindCombobox();
            }
            else
            {
                BindCombobox();
            }

            
        }

        void EditAndShowTask(bool edit, bool controlsEnable)
        {
            txtTitle.Text = _currentTask.Name;
            txtDescription.Text = _currentTask.Description;
            dtpStartTime.SelectedDate = _currentTask.StartDate;
            dtpDeadLine.SelectedDate = _currentTask.Deadline;
            if (!edit)
            {
                lblStatus.Content = $"Status : {_currentTask.Status.Name}";
                lblStatus.Visibility = Visibility.Visible;
            }
            else
            {
                if (_currentTask.Status.ID == 1)
                {
                    rdbDoing.IsChecked = true;
                }
                else
                {
                    rdbDone.IsChecked = true;
                }
                rdbDoing.Visibility = Visibility.Visible;
                rdbDone.Visibility = Visibility.Visible;
            }

            cmbTaskList.IsEnabled = controlsEnable;
            txtTitle.IsEnabled = controlsEnable;
            txtDescription.IsEnabled = controlsEnable;
            dtpDeadLine.IsEnabled = controlsEnable;
            dtpStartTime.IsEnabled = controlsEnable;
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

        private void RdbDoing_Checked(object sender, RoutedEventArgs e)
        {
            _currentTask.Status = _statusService.GetStatusByName(rdbDoing.Content.ToString());
        }

        private void RdbDone_Checked(object sender, RoutedEventArgs e)
        {
            _currentTask.Status = _statusService.GetStatusByName(rdbDone.Content.ToString());
        }
    }
}
