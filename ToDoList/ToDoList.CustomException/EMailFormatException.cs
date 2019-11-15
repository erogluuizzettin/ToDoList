using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoList.CustomException
{
    public class EMailFormatException : Exception
    {
        public override string Message
        {
            get
            {
                return "EMail address not entered in proper format";
            }
        }
    }
}
