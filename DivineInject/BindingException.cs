using System;

namespace DivineInject
{
    public class BindingException : Exception
    {
        public BindingException(string msg)
            : base(msg)
        { }
    }
}
