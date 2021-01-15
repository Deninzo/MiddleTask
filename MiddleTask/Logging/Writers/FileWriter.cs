using System.IO;

namespace MiddleTask.Logging.Writers
{
    public class FileWriter : IWriter
    {
        public FileWriter()
        {
            var filePath = File.ReadAllLines("config.txt")[1].Split('=')[1];
            _writer = new StreamWriter(filePath, true);
        }

        private readonly StreamWriter _writer;

        public void WriteLine(string message)
        {
            _writer.WriteLine(message);
        }
    }
}
