﻿namespace Oid85.FinMarket.Common.Helpers;

public static class DateHelper
{
    public static List<DateOnly> GetDates(DateOnly from, DateOnly to)
    {
        var curDate = from;
        var dates = new List<DateOnly>();

        var daysOfWeek = new List<DayOfWeek>()
        {
            DayOfWeek.Monday,
            DayOfWeek.Tuesday,
            DayOfWeek.Wednesday,
            DayOfWeek.Thursday,
            DayOfWeek.Friday
        };
        
        while (curDate <= to)
        {
            if (daysOfWeek.Contains(curDate.DayOfWeek))
                dates.Add(curDate);
            
            curDate = curDate.AddDays(1);
        }

        return dates;
    }

    public static List<DateTime> GetFiveMinutesDateTimes(DateTime from, DateTime to)
    {
        var curDateTime = from;
        var dates = new List<DateTime>();

        while (curDateTime <= to)
        {
            dates.Add(curDateTime);
            curDateTime = curDateTime.AddMinutes(5);
        }

        return dates;
    }
}