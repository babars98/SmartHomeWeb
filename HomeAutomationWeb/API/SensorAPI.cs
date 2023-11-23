using HomeAutomationWeb.DAL;

namespace HomeAutomationWeb.API
{
    public static class SensorAPI
    {
        public static void MapSensorAPIEndPoint(this WebApplication app)
        {
            app.MapGet("/api/CheckSchedule/{sensorId}/{time}",
                (string sensorId, string time) =>
                {
                    var result = new DataAccess().CheckSensorSchedule(sensorId, time);
                    return Results.Ok(result);
                });
        }
    }
}
