namespace MicroservicesChassis.Logging
{
    public class SerilogOptions
    {
        public LoggingTarget Console { get; set; }
        public LoggingTarget File { get; set; }
    }

    public class LoggingTarget
    {
        public bool Enabled { get; set; }
        public string Level { get; set; }
    }
}
