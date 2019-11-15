using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoList.CustomException
{
    public class ModelAvailableException : Exception
    {
        string _modelName;

        public ModelAvailableException(string modelName)
        {
            _modelName = modelName;
        }

        public override string Message
        {
            get
            {
                return $"{_modelName} not found";
            }
        }
    }
}
