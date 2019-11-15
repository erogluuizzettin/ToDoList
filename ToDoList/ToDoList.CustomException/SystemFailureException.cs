using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoList.CustomException
{
    public class SystemFailureException : Exception
    {
        public override string Message
        {
            get
            {
                return "There was an error from the system. Please try again";
            }
        }
    }
}
