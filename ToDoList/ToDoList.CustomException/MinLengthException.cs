using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoList.CustomException
{
    public class MinLengthException : Exception
    {
        string _fieldName;
        int _minLength;

        public MinLengthException(string fieldName, int minLength)
        {
            _fieldName = fieldName;
            _minLength = minLength;
        }

        public override string Message
        {
            get
            {
                return $"The {_fieldName} must be at least {_minLength} characters";
            }
        }
    }
}
