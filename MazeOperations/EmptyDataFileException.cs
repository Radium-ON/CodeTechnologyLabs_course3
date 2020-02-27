using System;
using System.Collections.Generic;
using System.Text;

namespace MazeOperations
{
    public class EmptyDataFileException : Exception
    {
        public EmptyDataFileException() { }

        public EmptyDataFileException(string message) : base(message) { }

        public EmptyDataFileException(string message, Exception inner) : base(message, inner) { }
    }
}
