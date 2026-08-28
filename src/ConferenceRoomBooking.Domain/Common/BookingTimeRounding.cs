public static class BookingTimeRounding
{
    public static DateTime RoundStartDown(DateTime time)
    {
        var roundedMinutes = time.Minute < 30 ? 0 : 30;
        return new DateTime(time.Year, time.Month, time.Day, time.Hour, roundedMinutes, 0);
    }

    public static DateTime RoundEndUp(DateTime time)
    {
        if (time.Minute == 0 || time.Minute == 30)
            return time;

        var roundedMinutes = time.Minute < 30 ? 30 : 0;
        var result = new DateTime(time.Year, time.Month, time.Day, time.Hour, roundedMinutes, 0);
        return roundedMinutes == 0 ? result.AddHours(1) : result;
    }
}