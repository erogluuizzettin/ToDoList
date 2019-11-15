using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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
using ToDoList.UI.WPF.Models;

namespace ToDoList.UI.WPF
{
    /// <summary>
    /// Interaction logic for ToDoListWindow.xaml
    /// </summary>
    public partial class ToDoListWindow : Window
    {
        TaskService _taskService;
        BoardService _boardService;
        User _currentUser;
        Board _currentBoardCombobox;
        Board _currentBoardListBox;
        TaskModel _currentTaskModel;
        List<Board> boards;
        List<TaskModel> taskItemSource;

        public ToDoListWindow(User user)
        {
            InitializeComponent();
            _taskService = new TaskService();
            _boardService = new BoardService();
            _currentUser = user;
        }

        private void BtnAddTaskList_Click(object sender, RoutedEventArgs e)
        {
            if (txtBoardName.Text == "--Enter Board Name--" || string.IsNullOrWhiteSpace(txtBoardName.Text))
            {
                MessageBox.Show("Please enter the name of the Task List");
                return;
            }
            else
            {
                try
                {
                    Board currentBoard = new Board();
                    currentBoard.Name = txtBoardName.Text;
                    currentBoard.UserID = _currentUser.ID;
                    _boardService.Insert(currentBoard);
                    MessageBox.Show("Task List added successfully");
                    txtBoardName.Text = "--Enter Board Name--";

                    //refresh
                    boards = lstTaskList.ItemsSource as List<Board>;
                    boards.Add(currentBoard);
                    cmbBoardList.ItemsSource = null;
                    lstTaskList.ItemsSource = null;
                    lstTaskList.ItemsSource = boards;
                    cmbBoardList.ItemsSource = boards;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void BtnAddTask_Click(object sender, RoutedEventArgs e)
        {
            if (cmbBoardList.Items.Count == 0)
            {
                MessageBox.Show("Task cannot be added without Task List");
                return;
            }
            AddTaskWindow window = new AddTaskWindow(_currentUser);
            window.ShowDialog();
            lstTaskList.SelectedIndex = -1;
        }

        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnRemoveTaskList_Click(object sender, RoutedEventArgs e)
        {
            if (cmbBoardList.Items.Count == 0)
            {
                MessageBox.Show("Task List not found");
                return;
            }else if (cmbBoardList.SelectedIndex == -1)
            {
                MessageBox.Show("Please select an item");
                return;
            }

            try
            {
                List<Task> tasks = new List<Task>();
                foreach (Task item in _taskService.GetAll())
                {
                    if (item.BoardID == _currentBoardCombobox.ID)
                    {
                        tasks.Add(item);
                    }
                }
                MessageBoxResult result = MessageBox.Show($"There are {tasks.Count} tasks in the Task List.Are you sure you want to delete", "Worning", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.No)
                {
                    cmbBoardList.SelectedIndex = -1;
                    return;
                }
                boards = lstTaskList.ItemsSource as List<Board>;
                boards.Remove(_currentBoardCombobox);
                _taskService.Delete(tasks);
                _boardService.Delete(_currentBoardCombobox);
                MessageBox.Show("Task List deletion is succesfull");
                cmbBoardList.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
            //refresh
            lstTaskList.ItemsSource = null;
            cmbBoardList.ItemsSource = null;
            lstTaskList.ItemsSource = boards;
            cmbBoardList.ItemsSource = boards;
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

        private void CmbBoardList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _currentBoardCombobox = cmbBoardList.SelectedItem as Board;
        }

        private void LstTaskList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            lstTaskByBoard.ItemsSource = null;
            if (lstTaskList.SelectedIndex == -1)
            {
                return;
            }

            _currentBoardListBox = lstTaskList.SelectedItem as Board;
            if (_currentBoardListBox == null)
            {
                return;
            }
            List<TaskModel> taskModels = new List<TaskModel>();
            foreach (Task item in _taskService.GetTasksByBoardID(_currentBoardListBox.ID))
            {
                TaskModel model = new TaskModel();
                model.ID = item.ID;
                model.Name = item.Name;
                model.Description = item.Description;
                model.StartDate = item.StartDate;
                model.DeadLine = item.Deadline;
                model.Status = item.Status;
                taskModels.Add(model);
            }
            lstTaskByBoard.ItemsSource = taskModels;
        }

        private void BtnTaskDelete_Click(object sender, RoutedEventArgs e)
        {
            Task task = SelectionTask();
            
            if (task == null)
            {
                return;
            }

            try
            {
                _taskService.Delete(task);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            //refresh
            lstTaskByBoard.ItemsSource = null;
            lstTaskByBoard.ItemsSource = taskItemSource;
        }

        private void BtnTaskFinish_Click(object sender, RoutedEventArgs e)
        {
            Task task = SelectionTask();
            if (task == null)
            {
                return;
            }

            if (task.StatusID == 2)
            {
                MessageBox.Show("Task already completed");
                return;
            }

            try
            {
                _taskService.TaskFinish(task);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            TaskModel taskModel = new TaskModel()
            {
                ID=task.ID,
                Name=task.Name,
                Description=task.Description,
                StartDate=task.StartDate,
                DeadLine=task.Deadline,
                Status=task.Status
            };

            //refresh
            taskItemSource.Add(taskModel);
            lstTaskByBoard.ItemsSource = null;
            lstTaskByBoard.ItemsSource = taskItemSource;
        }

        private void BtnTaskEdit_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("This part is not yet designed, sorry");
            return;
        }

        Task SelectionTask()
        {
            if (lstTaskByBoard.SelectedIndex != -1)
            {
                _currentTaskModel = lstTaskByBoard.SelectedItem as TaskModel;
                Task task = _taskService.Get(_currentTaskModel.ID);
                taskItemSource = lstTaskByBoard.ItemsSource as List<TaskModel>;
                taskItemSource.Remove(_currentTaskModel);
                return task;
            }
            else
            {
                MessageBox.Show("You did not select a Task");
                return null;
            }
        }

        private void BtnSearch_Click_1(object sender, RoutedEventArgs e)
        {
            List<TaskModel> tasks;
            SearchWindow window = new SearchWindow(out tasks);
            window.ShowDialog();
            lstTaskList.SelectedIndex = -1;
            lstTaskByBoard.ItemsSource = tasks;
        }
    }
}
