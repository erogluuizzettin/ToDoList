using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoList.CustomException
{
    public class EmptyListException : Exception
    {
        string _fieldName;

        public EmptyListException(string fieldName)
        {
            _fieldName = fieldName;
        }

        public override string Message
        {
            get
            {
                return $"No registered {_fieldName} found";
            }
        }
    }
}
