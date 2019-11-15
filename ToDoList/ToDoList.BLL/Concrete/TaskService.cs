using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ToDoList.BLL.Abstract;
using ToDoList.CustomException;
using ToDoList.DAL.Abstract;
using ToDoList.DAL.Concrete.EntityFramework.DAL;
using ToDoList.Model;

namespace ToDoList.BLL.Concrete
{
    public class TaskService : ITaskService
    {
        EFTaskDAL _taskDAL;

        public TaskService()
        {
            _taskDAL = new EFTaskDAL();
        }

        public bool Delete(Task model)
        {
            CheckAvailableTask(model.ID);
            return _taskDAL.Delete(model) > 0;
        }

        public bool Delete(int entityID)
        {
            return _taskDAL.Delete(Get(entityID)) > 0;
        }

        public void Delete(List<Task> tasks)
        {
            try
            {
                foreach (Task item in tasks)
                {
                    Delete(item);
                }
            }
            catch (Exception)
            {
                throw new Exception();
            }
        }

        public Task Get(int entityID)
        {
            try
            {
                return _taskDAL.Get(a => a.ID == entityID);
            }
            catch
            {
                throw new ModelAvailableException("Task");
            }
        }

        public List<Task> GetAll()
        {
            try
            {
                return _taskDAL.GetAll().ToList();
            }
            catch
            {
                throw new EmptyListException("Task");
            }
        }

        public bool Insert(Task model)
        {
            CheckNullField(model);
            CheckMaxLengthName(model.Name);
            CheckMaxLengthDescription(model.Description);
            return _taskDAL.Add(model) > 0;
        }

        public bool Update(Task model)
        {
            CheckNullField(model);
            CheckAvailableTask(model.ID);
            CheckMaxLengthName(model.Name);
            CheckMaxLengthDescription(model.Description);
            return _taskDAL.Update(model) > 0;
        }


        void CheckNullField(Task task)
        {
            if (string.IsNullOrWhiteSpace(task.Name))
            {
                throw new NotNullException("Name");
            }
            if (string.IsNullOrWhiteSpace(task.Description))
            {
                throw new NotNullException("Description");
            }
            if (task.StartDate == null)
            {
                throw new NotNullException("Start Time");
            }
            if (task.Deadline == null)
            {
                throw new NotNullException("Finish Time");
            }
        }
        
        void CheckAvailableTask(int taskID)
        {
            bool flag = false;

            foreach (Task item in GetAll())
            {
                if (item.ID == taskID)
                {
                    flag = true;
                    break;
                }
            }

            if (!flag)
            {
                throw new ModelAvailableException("Task");
            }
        }

        void CheckMaxLengthName(string name)
        {
            if (name.Length > 50)
            {
                throw new MaxLengthException("Name", 50);
            }
        }

        void CheckMaxLengthDescription(string description)
        {
            if (description.Length > 500)
            {
                throw new MaxLengthException("Description", 500);
            }
        }

        public List<Task> GetTasksByBoardID(int boardID)
        {
            try
            {
                return _taskDAL.GetAll(a => a.BoardID == boardID).ToList();
            }
            catch
            {
                throw new EmptyListException("Board");
            }
        }

        public void TaskFinish(Task task)
        {
            task.StatusID = 2;
            _taskDAL.Update(task);
        }
    }
}
