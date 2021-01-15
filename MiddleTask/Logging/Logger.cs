using System;
using System.Collections.Generic;
using System.Text;
using MiddleTask.Logging.Writers;

namespace MiddleTask.Logging
{
    public class Logger
    {
        public Logger(IWriter[] writers)
        {
            _writers = writers;
        }

        private readonly IList<IWriter> _writers;

        public void Log(string message)
        {
            foreach (var writer in _writers)
            {
                writer.WriteLine(message);
            }
        }
    }
}
