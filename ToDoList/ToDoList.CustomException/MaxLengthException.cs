using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoList.CustomException
{
    public class MaxLengthException : Exception
    {
        string _fieldName;
        int _maxLength;

        public MaxLengthException(string fieldName, int maxLength)
        {
            _fieldName = fieldName;
            _maxLength = maxLength;
        }

        public override string Message
        {
            get
            {
                return $"The {_fieldName} should be the most {_maxLength} characters";
            }
        }
    }
}
