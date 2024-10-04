using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OSK.Utils
{
    public static class TimeUtils
    {
        private static float secPerYear;
        public delegate void Task();

        public static void DoDelay(this MonoBehaviour _behaviour, float delay, Task task, bool unscaleTime = false)
        {
            _behaviour.StartCoroutine(unscaleTime ? DoTaskUnscale(task, delay) : DoTask(task, delay));
        }

        private static IEnumerator DoTask(Task task, float delay)
        {
            yield return new WaitForSeconds(delay);
            task();
        }

        private static IEnumerator DoTaskUnscale(Task task, float delay)
        {
            yield return new WaitForSecondsRealtime(delay);
            task();
        }

        public static double SystemTimeInMilliseconds
        {
            get => (System.DateTime.UtcNow - new System.DateTime(1970, 1, 1)).TotalMilliseconds;
        }

        public static void SetSecPerYear(float value)
        {
            secPerYear = value;
        }

        public static float YearsToSec(float years)
        {
            return years * secPerYear;
        }
        public static float SecToYears(float value)
        {
            return value / secPerYear;
        }

        public static float SecToMos(float value)
        {
            float _years = value / secPerYear;
            float _mos = _years * 12;
            return _mos;
        }
        
        public static string MinutesToHours(float value)
        {
            int days = (int)value / 86400;
            int hours = (int)(value % 86400) / 3600;
            int minutes = (int)(value % 3600) / 60;
            int seconds = (int)value % 60;
            string formattedTime;

            if (days > 0)
                formattedTime = string.Format("{0:D1}d {1:D1}h", days, hours);
            else if (hours > 0)
                formattedTime = string.Format("{0:D1}h {1:D1}m", hours, minutes);
            else
                formattedTime = string.Format("{0:D1}m {1:D1}s", minutes, seconds);
            return formattedTime;
        }


        public static string SecondsToHours(float value)
        {
            TimeSpan timeSpan = TimeSpan.FromSeconds(value);
            string timerFormatted;
            if (timeSpan.Days == 0)
            {
                timerFormatted = string.Format("{0:D1}h {1:D1}m {2:D1}s", timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
            }
            else timerFormatted = string.Format("{0:D1}d {1:D1}h {2:D1}m {3:D1}s", timeSpan.Days, timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
            return timerFormatted;
        }
         
        public static double GetCurrentTime()
        {
            TimeSpan span = DateTime.Now.Subtract(new DateTime(1970, 1, 1, 0, 0, 0));
            return span.TotalSeconds;
        }

        public static double GetCurrentTimeInDays()
        {
            TimeSpan span = DateTime.Now.Subtract(new DateTime(1970, 1, 1, 0, 0, 0));
            return span.TotalDays;
        }

        public static double GetCurrentTimeInMills()
        {
            TimeSpan span = DateTime.Now.Subtract(new DateTime(1970, 1, 1, 0, 0, 0));
            return span.TotalMilliseconds;
        }

        public static int GetSecondElapsed(DateTime prevDate)
        {
            DateTime now = DateTime.Now;
            TimeSpan timeDifference = now - prevDate;
            return timeDifference.Seconds;
        }
 

        public static float HoursToSeconds(float value)
        {
            return value * 3600;
        }

        public static string FormatSeconds(float elapsed)
        {
            int d = (int)(elapsed * 100.0f);
            int minutes = d / (60 * 100);
            int seconds = (d % (60 * 100)) / 100;
            int hundredths = d % 100;
            return string.Format("{0:00}:{1:00}.{2:00}", minutes, seconds, hundredths);
        }

        public static string ConvertIntToTimeHH_MM_SS(int duration)
        {
            string time = "";
            for (int i = 2; i >= 0; i--)
            {
                if (i < 2) time += ":";
                int detailTime = (int)Mathf.Pow(60, i);
                int t = duration / detailTime;
                duration = duration % detailTime;
                if (t > 9)
                {
                    time += t.ToString();
                    continue;
                }
                if (t > 0)
                {
                    time += "0" + t.ToString();
                    continue;
                }
                time += "00";
            }
            return time;
        }

    }
}
