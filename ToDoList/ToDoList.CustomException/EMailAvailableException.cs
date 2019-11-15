﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoList.CustomException
{
    public class EMailAvailableException : Exception
    {
        public override string Message
        {
            get
            {
                return "This email address is available in the system";
            }
        }
    }
}
