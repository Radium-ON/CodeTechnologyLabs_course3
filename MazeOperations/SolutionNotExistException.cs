using System;
using System.Collections.Generic;
using System.Text;

namespace MazeOperations
{
    public class SolutionNotExistException : Exception
    {
        public SolutionNotExistException()
        {
        }

        public SolutionNotExistException(string message) : base(message)
        {
        }

        public SolutionNotExistException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
