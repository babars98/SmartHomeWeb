using HomeAutomationWeb.DAL;

namespace HomeAutomationWeb.API
{
    public static class SensorAPI
    {
        public static void MapSensorAPIEndPoint(this WebApplication app)
        {
            app.MapGet("/api/CheckSchedule/{sensorId}/{time}/{data}",
                (string sensorId, string time, double data) =>
                {
                    var result = new DataAccess().GetSensorSchedule(sensorId, time);
                    bool res = IsTimeBetweenStartandEnd(time, result.Item1, result.Item2);
                    Task.Run(() => SaveSensorData(sensorId, data, time));
                    return Results.Ok(res);
                });
        }

        private static async Task SaveSensorData(string sensorId, double data, string time)
        {
            var result = new DataAccess().SaveSensorData(sensorId, data, time);
        }

        private static bool IsTimeBetweenStartandEnd(string currentTime, string startTime, string endTime)
        {
            try
            {
                TimeSpan start = TimeSpan.Parse(startTime);
                TimeSpan end = TimeSpan.Parse(endTime);
                TimeSpan now = TimeSpan.Parse(currentTime);

                if (start <= end)
                {
                    // start and stop times are in the same day
                    if (now >= start && now <= end)
                    {
                        return true;
                    }
                }
                else
                {
                    // start and stop times are in different days
                    if (now >= start || now <= end)
                    {
                        return true;
                    }
                }
            }
            catch(Exception ex) { }

            return false;
        }
    }
}
