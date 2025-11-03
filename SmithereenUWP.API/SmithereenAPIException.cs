using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmithereenUWP.API
{
    public sealed class SmithereenAPIException : Exception
    {
        public int Code { get; private set; }

        internal SmithereenAPIException(int code, string message) : base(message)
        {
            Code = code;
        }
    }
}
