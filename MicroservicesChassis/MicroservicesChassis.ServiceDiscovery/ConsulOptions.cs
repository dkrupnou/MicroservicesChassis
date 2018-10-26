namespace MicroservicesChassis.ServiceDiscovery
{
    public class ConsulOptions
    {
        public bool Enabled { get; set; }
        public string Url { get; set; }
        public string ServiceName { get; set; }
        public bool PingEnabled { get; set; }
        public string PingEndpoint { get; set; }
        public int PingInterval { get; set; }
        public int RemoveAfterInterval { get; set; }
        public int RequestRetries { get; set; }
    }
}
