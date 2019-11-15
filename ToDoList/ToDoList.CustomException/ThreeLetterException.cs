using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoList.CustomException
{
    public class ThreeLetterException : Exception
    {
        public override string Message
        {
            get
            {
                return "The password must contain at least 3 letters";
            }
        }
    }
}
