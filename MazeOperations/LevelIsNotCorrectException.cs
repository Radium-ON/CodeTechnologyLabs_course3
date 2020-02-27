using System;
using System.Collections.Generic;
using System.Text;

namespace MazeOperations
{
    public class LevelIsNotCorrectException : Exception
    {
        public LevelIsNotCorrectException() { }

        public LevelIsNotCorrectException(string message) : base(message) { }

        public LevelIsNotCorrectException(string message, Exception inner) : base(message, inner) { }
    }
}
