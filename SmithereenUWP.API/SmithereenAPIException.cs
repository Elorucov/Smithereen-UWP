using System;

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
