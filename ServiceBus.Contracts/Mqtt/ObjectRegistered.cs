namespace Hubertyne.ServiceBus.Contracts.Mqtt
{
    public record ObjectRegistered
    {
        public string Model { get; set; }
        
        public Guid Id { get; set; }

        public DateTime ConnectedDate { get; set; }
    }
}