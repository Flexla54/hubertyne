namespace MqttService.Models
{
    public class Announcement
    {
        public Guid Id { get; set; }

        public string Model { get; set; }

        public string Mac { get; set; }

        public string Ip { get; set; }
    }
}
