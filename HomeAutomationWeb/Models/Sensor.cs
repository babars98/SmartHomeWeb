namespace HomeAutomationWeb.Models
{
    public class Sensor
    {
        public int SensorId { get; set; }
        public string SensorName { get; set; } = string.Empty;
        public string SensorGUID { get; set; } = string.Empty;
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public bool IsActive { get; set; }
    }
}
