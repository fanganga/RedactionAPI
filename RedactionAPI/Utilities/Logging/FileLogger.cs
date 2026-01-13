namespace RedactionAPI.Utilities.Logging
{
    public class FileLogger : ICustomLogger
    {
        private readonly string _loggingFilePath = "./redact_log.txt";
        public void WriteLogLine(string message)
        {
            File.AppendAllLines(_loggingFilePath, [message]);
        }
    }
}
