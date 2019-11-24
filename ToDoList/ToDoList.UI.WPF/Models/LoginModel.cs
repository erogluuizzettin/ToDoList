using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoList.UI.WPF.Models
{
    [Serializable]
    public class LoginModel
    {
        public string EMail { get; set; }
        public string Password { get; set; }
        public string Activationcode { get; set; }
    }
}
