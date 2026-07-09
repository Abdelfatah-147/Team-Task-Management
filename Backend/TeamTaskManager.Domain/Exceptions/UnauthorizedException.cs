using System;
using System.Collections.Generic;
using System.Text;

namespace TeamTaskManager.Domain.Exceptions
{
    public class UnauthorizedException : Exception
    {
        public UnauthorizedException() : base("You are not authorized to perform this action.")
        {
        }
    }
}
