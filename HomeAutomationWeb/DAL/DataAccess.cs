using HomeAutomationWeb.BL;
using HomeAutomationWeb.Models;
using System.Data;
using System.Dynamic;

namespace HomeAutomationWeb.DAL
{
    public class DataAccess
    {
        private readonly DBConnect _dbConnect;

        public DataAccess()
        {
            _dbConnect = new DBConnect();
        }

        public List<Sensor> GetAllSensors()
        {
            string query = "SELECT SensorId,Name,SensorGUID,StartTime,EndTime,IsActive FROM dbo.[Sensor] WHERE IsActive = 1";

            var datatable = _dbConnect.ExecuteReadQuery(query);

            var list = new List<Sensor>();

            if (datatable.Rows.Count < 1)
                return list;

            foreach (DataRow row in datatable.Rows)
            {
                list.Add(new Sensor()
                {
                    SensorId = Convert.ToInt32(row["SensorId"]),
                    SensorName = row["Name"].ToString(),
                    SensorGUID = row["SensorGUID"].ToString(),
                    StartTime = Utility.Convert24Hrto12HrFormat((TimeSpan)row["StartTime"]),
                    EndTime = Utility.Convert24Hrto12HrFormat((TimeSpan)row["EndTime"]),
                    IsActive = Convert.ToBoolean(row["IsActive"])
                });
            }

            return list;
        }

        public Sensor GetSensorInfo(string SensorGuid)
        {
            string query = string.Format("SELECT SensorId,Name,SensorGUID,StartTime,EndTime,IsActive FROM dbo.[Sensor] WHERE SensorGUID = '{0}' AND IsActive = 1", SensorGuid);

            var datatable = _dbConnect.ExecuteReadQuery(query);

            if (datatable.Rows.Count < 1)
                return new Sensor();

            DataRow row = datatable.Rows[0];

            var sensor = new Sensor
            {
                SensorId = Convert.ToInt32(row["SensorId"]),
                SensorName = row["Name"].ToString(),
                SensorGUID = row["SensorGUID"].ToString(),
                StartTime = Utility.Convert24Hrto12HrFormat((TimeSpan)row["StartTime"]),
                EndTime = Utility.Convert24Hrto12HrFormat((TimeSpan)row["EndTime"]),
                IsActive = Convert.ToBoolean(row["IsActive"])
            };

            return sensor;
        }

        public bool AddSensor(Sensor sensor)
        {
            string query = "INSERT INTO dbo.[Sensor] (Name,SensorGUID,StartTime,EndTime,DateCreated,IsActive) VALUES (@name,@guid,@starttime,@endtime,@datecreated,@isactive)";

            var param = new Dictionary<string, object>()
            {
                { "@name", sensor.SensorName},
                { "@guid", Guid.NewGuid()},
                { "@starttime", Utility.Convert12Hrto24HrFormat(sensor.StartTime)},
                { "@endtime", Utility.Convert12Hrto24HrFormat(sensor.EndTime)},
                { "@isactive", true },
                { "@datecreated", DateTime.Now }
            };

            return _dbConnect.ExecuteWriteQuery(query, param);
        }

        public bool UpdateSensor(Sensor sensor)
        {
            string query = string.Format("UPDATE dbo.[Sensor] SET Name = @name, StartTime = @starttime, EndTime = @endtime WHERE SensorGUID = '{0}'", sensor.SensorGUID);

            var param = new Dictionary<string, object>()
            {
                { "@name", sensor.SensorName},
                { "@guid", Guid.NewGuid()},
                { "@starttime", Utility.Convert12Hrto24HrFormat(sensor.StartTime)},
                { "@endtime", Utility.Convert12Hrto24HrFormat(sensor.EndTime)},
                { "@isactive", true },
                { "@datecreated", DateTime.Now }
            };

            return _dbConnect.ExecuteWriteQuery(query, param);
        }

        public (string, string) GetSensorSchedule(string sensorId, string time)
        {
            try
            {
                var parsedtime = Utility.Convert12Hrto24HrFormat(TimeOnly.Parse(time));
                string query = string.Format("SELECT StartTime, EndTime FROM dbo.Sensor WHERE SensorGUID = '{0}'", sensorId);

                var datatable = _dbConnect.ExecuteReadQuery(query);

                DataRow row = datatable.Rows[0];
                var startTime = row["StartTime"].ToString();
                var endTime = row["EndTime"].ToString();

                return (startTime, endTime);
            }
            catch (Exception ex) { }

            return (TimeOnly.MinValue.ToString(), TimeOnly.MinValue.ToString());
        }
    }
}
