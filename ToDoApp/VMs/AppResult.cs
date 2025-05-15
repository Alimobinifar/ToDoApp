using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoApp.VMs
{
    public class AppResult
    {
        public int Result { get; set; }
        public int ErrCode { get; set; }
        public string Msg { get; set; }
        public bool Error { get; set; }
        public IEnumerable List { get; set; }
        public Object Object { get; set; }
        
    }
}
