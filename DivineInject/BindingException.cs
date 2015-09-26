using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DivineInject
{
    public class BindingException : Exception
    {
        public BindingException(string msg)
            : base(msg)
        { }
    }
}
