using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using ToDoList.BLL.Abstract;
using ToDoList.CustomException;
using ToDoList.DAL.Abstract;
using ToDoList.DAL.Concrete.EntityFramework.DAL;
using ToDoList.Model;

namespace ToDoList.BLL.Concrete
{
    public class UserService : IUserService
    {
        EFUserDAL _userDAL;

        public UserService()
        {
            _userDAL = new EFUserDAL();
        }

        public bool Delete(User model)
        {
            CheckAvailableUser(model.ID);
            return _userDAL.Delete(model) > 0;
        }

        public bool Delete(int entityID)
        {
            return _userDAL.Delete(Get(entityID)) > 0;
        }

        public User Get(int entityID)
        {
            try
            {
                return _userDAL.Get(a => a.ID == entityID);
            }
            catch
            {
                throw new ModelAvailableException("User");
            }
        }

        public List<User> GetAll()
        {
            try
            {
                return _userDAL.GetAll().ToList();
            }
            catch
            {

                throw new EmptyListException("User");
            }
        }

        public bool Insert(User model)
        {
            CheckNullFields(model);
            CheckMailAvailable(model.EMail);
            AllControl(model.EMail, model.Password, model.FirstName, model.LastName);
            model.UserRoleID = 2;
            return _userDAL.Add(model) > 0;
        }

        public bool Update(User model)
        {
            CheckNullFields(model);
            CheckAvailableUser(model.ID);
            AllControl(model.EMail, model.Password, model.FirstName, model.LastName);
            return _userDAL.Update(model) > 0;
        }

        public User GetUserByLogin(string email, string password)
        {
            GetEMailAvailable(email, password);
            CheckPasswordMinLength(password);
            CheckPasswordMaxLength(password);
            CheckPasswordThreeLetter(password);
            try
            {
                return _userDAL.Get(a => a.EMail == email && a.Password == password);
            }
            catch (Exception)
            {
                throw new ModelAvailableException("User");
            }
        }


        void GetEMailAvailable(string email, string password)
        {
            bool flag = false;
            foreach (User item in GetAll())
            {
                if (item.EMail == email)
                {
                    flag = true;
                    if (item.Password != password)
                    {
                        throw new PasswordIncorrectException();
                    }
                    break;
                }
            }

            if (!flag)
            {
                throw new ModelAvailableException("User");
            }
        }


        void AllControl(string email, string password, string firstName, string lastName)
        {
            CheckMailFormat(email);
            CheckPasswordMinLength(password);
            CheckPasswordThreeLetter(password);
            CheckFirstNameMaxLength(firstName);
            CheckLastNameMaxLength(lastName);
            CheckEMailMaxLength(email);
            CheckPasswordMaxLength(password);
        }

        void CheckPasswordMaxLength(string password)
        {
            if (password.Length > 20)
            {
                throw new MaxLengthException("Password", 20);
            }
        }

        void CheckEMailMaxLength(string email)
        {
            if (email.Length > 255)
            {
                throw new MaxLengthException("EMail", 255);
            }
        }

        void CheckLastNameMaxLength(string lastName)
        {
            if (lastName.Length > 20)
            {
                throw new MaxLengthException("Last Name", 20);
            }
        }

        void CheckFirstNameMaxLength(string firstName)
        {
            if (firstName.Length > 30)
            {
                throw new MaxLengthException("First Name", 30);
            }
        }

        void CheckMailFormat(string email)
        {
            try
            {
                MailAddress mailAddress = new MailAddress(email);
            }
            catch
            {
                throw new EMailFormatException();
            }
        }

        void CheckMailAvailable(string email)
        {
            foreach (User item in GetAll())
            {
                if (item.EMail == email)
                {
                    throw new EMailAvailableException();
                }
            }
        }

        void CheckPasswordMinLength(string password)
        {
            if (password.Length < 6)
            {
                throw new MinLengthException("password", 6);
            }
        }

        void CheckPasswordThreeLetter(string password)
        {
            int letter = 0;
            foreach (char item in password)
            {
                if (char.IsLetter(item))
                {
                    letter++;
                }
            }

            if (letter < 3)
            {
                throw new ThreeLetterException();
            }
        }

        void CheckNullFields(User user)
        {
            if (string.IsNullOrWhiteSpace(user.FirstName))
            {
                throw new NotNullException("First Name");
            }
            if (string.IsNullOrWhiteSpace(user.LastName))
            {
                throw new NotNullException("Last Name");
            }
            if (string.IsNullOrWhiteSpace(user.EMail))
            {
                throw new NotNullException("EMail");
            }
            if (string.IsNullOrWhiteSpace(user.Password))
            {
                throw new NotNullException("Password");
            }
        }

        void CheckAvailableUser(int userID)
        {
            bool flag = false;
            foreach (User item in GetAll())
            {
                if (item.ID == userID)
                {
                    flag = true;
                    break;
                }
            }

            if (!flag)
            {
                throw new ModelAvailableException("User");
            }
        }

        public User GetUserByActivationCode(string activationCode)
        {
            try
            {
                return _userDAL.Get(a => a.ActivationCode == activationCode);
            }
            catch
            {
                throw new Exception("Activation Code incorrect");
            }
        }

        public bool GetUserByIsActive(User user)
        {
            try
            {
                User currentUSer = _userDAL.Get(a => a.ID == user.ID);
                return currentUSer.IsActive;
            }
            catch
            {
                throw new ModelAvailableException("User");
            }
        }

        public void DeleteUsersInActive()
        {
            foreach (User item in GetAll())
            {
                if (!GetUserByIsActive(item))
                {
                    Delete(item.ID);
                }
            }
        }
    }
}
