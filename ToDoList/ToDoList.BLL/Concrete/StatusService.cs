using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoList.BLL.Abstract;
using ToDoList.CustomException;
using ToDoList.DAL.Abstract;
using ToDoList.DAL.Concrete.EntityFramework.DAL;
using ToDoList.Model;

namespace ToDoList.BLL.Concrete
{
    public class StatusService : IStatusService
    {
        EFStatusDAL _statusDAL;

        public StatusService()
        {
            _statusDAL = new EFStatusDAL();
        }

        public bool Delete(Status model)
        {
            CheckAvailableStatus(model.ID);
            return _statusDAL.Delete(model) > 0;
        }

        public bool Delete(int entityID)
        {
            return _statusDAL.Delete(Get(entityID)) > 0;
        }

        public Status Get(int entityID)
        {
            try
            {
                return _statusDAL.Get(a => a.ID == entityID);
            }
            catch
            {
                throw new ModelAvailableException("Status");
            }
        }

        public List<Status> GetAll()
        {
            try
            {
                return _statusDAL.GetAll().ToList();
            }
            catch
            {
                throw new EmptyListException("Status");
            }
        }

        public bool Insert(Status model)
        {
            CheckMaxLengthName(model.Name);
            return _statusDAL.Add(model) > 0;
        }

        public bool Update(Status model)
        {
            CheckAvailableStatus(model.ID);
            CheckMaxLengthName(model.Name);
            return _statusDAL.Update(model) > 0;
        }


        void CheckAvailableStatus(int statusID)
        {
            bool flag = false;

            foreach (Status item in GetAll())
            {
                if (item.ID == statusID)
                {
                    flag = true;
                    break;
                }
            }

            if (!flag)
            {
                throw new ModelAvailableException("Status");
            }
        }

        void CheckMaxLengthName(string name)
        {
            if (name.Length > 8)
            {
                throw new MaxLengthException("Name", 8);
            }
        }
    }
}
