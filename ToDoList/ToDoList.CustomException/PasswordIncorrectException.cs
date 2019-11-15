using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoList.CustomException
{
    public class PasswordIncorrectException : Exception
    {
        public override string Message
        {
            get
            {
                return "Password is incorrect";
            }
        }
    }
}
