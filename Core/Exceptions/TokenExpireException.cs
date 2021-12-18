using System;
namespace Core.Exceptions
{
    public class TokenExpireException : Exception
    {
        public TokenExpireException(string message) : base(message)
        {
        }
    }
}
