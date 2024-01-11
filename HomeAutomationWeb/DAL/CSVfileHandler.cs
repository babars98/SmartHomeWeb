using HomeAutomationWeb.BL;
using HomeAutomationWeb.Models;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Hosting.Server;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.CompilerServices;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace HomeAutomationWeb.DAL
{
    public class CSVfileHandler
    {
        #region private
        private const string SensorFile = "SensorFile.csv";
        private const string SensorDataFile = "SensorDataFile.csv";
        #endregion

        public bool AddSensor(Sensor sensor)
        {
            var data = $"{sensor.SensorName},{Guid.NewGuid()},{Utility.Convert12Hrto24HrFormat(sensor.StartTime)},{Utility.Convert12Hrto24HrFormat(sensor.EndTime)}";
            var file = GetCCVFilePath(SensorFile);
            try
            {
                using StreamWriter writer = new StreamWriter(file, true);
                writer.WriteLine(data.ToString());

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public List<Sensor> GetAllSensors()
        {
            var list = new List<Sensor>();
            StreamReader reader;
            var file = GetCCVFilePath(SensorFile);
            try
            {
                reader = new StreamReader(file);
            }
            catch (Exception ex)
            {
                return list;
            }


            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                var data = line.Split(',');
                list.Add(new Sensor()
                {
                    SensorName = data[0],
                    SensorGUID = data[1],
                    StartTime = Utility.Convert24Hrto12HrFormat(Convert.ToDateTime(data[2])),
                    EndTime = Utility.Convert24Hrto12HrFormat(Convert.ToDateTime(data[3]))
                });
            }
            reader.Dispose();

            return list;
        }

        public Sensor GetSensorInfo(string sensorGuid)
        {

            var sensors = GetAllSensors();

            if (sensors == null || sensors.Count <= 0)
                return new Sensor();


            var sensor = sensors.FirstOrDefault(c => c.SensorGUID == sensorGuid);

            return sensor;
        }

        public bool UpdateSensor(Sensor sensor)
        {

            var sensors = GetAllSensors();
            if (sensors == null || sensors.Count <= 0)
                return false;

            var sb = new StringBuilder();

            for (int i = 0; i < sensors.Count; i++)
            {
                if (sensors[i].SensorGUID == sensor.SensorGUID)
                {
                    var d = string.Format($"{sensor.SensorName},{sensor.SensorGUID},{Utility.Convert12Hrto24HrFormat(sensor.StartTime)},{Utility.Convert12Hrto24HrFormat(sensor.EndTime)}");
                    sb.AppendLine(d);
                }
                else
                {
                    var d = string.Format($"{sensors[i].SensorName},{sensors[i].SensorGUID},{Utility.Convert12Hrto24HrFormat(sensors[i].StartTime)},{Utility.Convert12Hrto24HrFormat(sensors[i].EndTime)}");
                    sb.AppendLine(d);
                }
            }

            var file = GetCCVFilePath(SensorFile);
            using StreamWriter writer = new StreamWriter(file, false);
            writer.Write(sb.ToString());

            return true;
        }

        public (string, string) GetSensorSchedule(string sensorId)
        {
            try
            {
                var sensors = GetAllSensors();
                var sensor = sensors.FirstOrDefault(c => c.SensorGUID == sensorId);

                return (Utility.Convert12Hrto24HrFormat(sensor.StartTime), Utility.Convert12Hrto24HrFormat(sensor.EndTime));
            }
            catch (Exception ex) { }

            return (TimeOnly.MinValue.ToString(), TimeOnly.MinValue.ToString());
        }

        public List<SensorData> GetSensorData(string SensorGuid)
        {
            var list = new List<SensorData>();
            StreamReader reader;
            var file = GetCCVFilePath(SensorDataFile);
            try
            {
                reader = new StreamReader(file);
            }
            catch (Exception ex)
            {
                return list;
            }


            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                var data = line.Split(',');
                list.Add(new SensorData()
                {
                    Data = Convert.ToDouble(data[1]),
                    Time = data[2]
                });
            }
            reader.Dispose();

            return list;
        }

        public bool SaveSensorData(string sensorId, double data, string time)
        {
            var sensor = GetSensorInfo(sensorId);

            if (!sensor.SensorName.Contains("Temperature", StringComparison.OrdinalIgnoreCase))
                return false;

            var line = $"{sensorId},{data},{Utility.Convert12Hrto24HrFormat(Convert.ToDateTime(time))}";
            var file = GetCCVFilePath(SensorDataFile);

            try
            {
                using StreamWriter writer = new StreamWriter(file, true);
                writer.WriteLine(line);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private string GetCCVFilePath(string filename)
        {
            var path = System.Reflection.Assembly.GetExecutingAssembly().Location;
            Console.WriteLine("Paht : " + path);
            path = path.Replace("HomeAutomationWeb.dll", "");

            return Path.Combine(path, filename);
        }
    }
}
