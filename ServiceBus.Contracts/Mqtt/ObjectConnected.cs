namespace Hubertyne.ServiceBus.Contracts.Mqtt
{
    public record ObjectConnected
    {
        public string Type { get; set; }
        
        public string Id { get; set; }

        public DateTime ConnectedDate { get; set; }
    }
}