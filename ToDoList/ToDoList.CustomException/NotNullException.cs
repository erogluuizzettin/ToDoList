using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoList.CustomException
{
    public class NotNullException : Exception
    {
        string _fieldName;

        public NotNullException(string fieldName)
        {
            _fieldName = fieldName;
        }

        public override string Message
        {
            get
            {
                return $"{_fieldName} space can not be passed empty";
            }
        }
    }
}
