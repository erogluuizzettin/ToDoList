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
    public class UserRoleService : IUserRoleService
    {
        EFUserRoleDAL _userRoleDAL;

        public UserRoleService()
        {
            _userRoleDAL = new EFUserRoleDAL();
        }

        public bool Delete(UserRole model)
        {
            CheckAvailableUserRole(model.ID);
            return _userRoleDAL.Delete(model) > 0;
        }

        public bool Delete(int entityID)
        {
            return _userRoleDAL.Delete(Get(entityID)) > 0;
        }

        public UserRole Get(int entityID)
        {
            try
            {
                return _userRoleDAL.Get(a => a.ID == entityID);
            }
            catch
            {
                throw new ModelAvailableException("UserRole");
            }
        }

        public List<UserRole> GetAll()
        {
            try
            {
                return _userRoleDAL.GetAll().ToList();
            }
            catch
            {
                throw new EmptyListException("UserRole");
            }
        }

        public bool Insert(UserRole model)
        {
            CheckMaxLengthRoleName(model.RoleName);
            return _userRoleDAL.Add(model) > 0;
        }

        public bool Update(UserRole model)
        {
            CheckAvailableUserRole(model.ID);
            CheckMaxLengthRoleName(model.RoleName);
            return _userRoleDAL.Update(model) > 0;
        }


        void CheckAvailableUserRole(int userRoleID)
        {
            bool flag = false;

            foreach (UserRole item in GetAll())
            {
                if (item.ID == userRoleID)
                {
                    flag = true;
                    break;
                }
            }

            if (!flag)
            {
                throw new ModelAvailableException("UserRole");
            }
        }

        void CheckMaxLengthRoleName(string roleName)
        {
            if (roleName.Length > 13)
            {
                throw new MaxLengthException("Role Name", 13);
            }
        }
    }
}
