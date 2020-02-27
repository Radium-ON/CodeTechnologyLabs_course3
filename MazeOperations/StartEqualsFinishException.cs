using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace MazeOperations
{
    public class StartEqualsFinishException : Exception
    {
        public StartEqualsFinishException(){ }

        public StartEqualsFinishException(string message) : base(message)
        {
        }

        public StartEqualsFinishException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
