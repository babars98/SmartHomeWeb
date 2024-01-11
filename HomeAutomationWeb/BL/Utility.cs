namespace HomeAutomationWeb.BL
{
    public static class Utility
    {
        public static string Convert12Hrto24HrFormat(TimeOnly time)
        {
            return DateTime.Parse(time.ToString()).ToString("HH:mm");
        }

        public static TimeOnly Convert24Hrto12HrFormat(TimeSpan time)
        {
            return new TimeOnly(time.Hours, time.Minutes);
        }

        public static TimeOnly Convert24Hrto12HrFormat(DateTime time)
        {
            return new TimeOnly(time.Hour, time.Minute);
        }
        public static string Convert12Hrto24HrFormat(DateTime time)
        {
            return DateTime.Parse(time.ToString()).ToString("HH:mm");
        }

    }
}
