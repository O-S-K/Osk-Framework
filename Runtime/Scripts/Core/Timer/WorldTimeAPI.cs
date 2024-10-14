using UnityEngine;
using System;
using System.Collections;
using System.Text.RegularExpressions;
using UnityEngine.Networking;

namespace OSK
{
    public class WorldTimeAPI
    {
        struct TimeData
        {
            //public string client_ip;
            //...
            public string datetime;

            public string timezone;
            //..
        }

        const string API_URL = "https://worldtimeapi.org/api/ip";
        private DateTime _currentDateTime = DateTime.Now;

        public DateTime GetCurrentDateTime()
        {
            //here we don't need to get the datetime from the server again
            // just add elapsed time since the game start to _currentDateTime
            return _currentDateTime.AddSeconds(Time.realtimeSinceStartup);
        }

        public DateTime GetRealDateTime()
        {
            return _currentDateTime;
        }


        public IEnumerator GetRealDateTimeFromAPI()
        {
            using UnityWebRequest webRequest = UnityWebRequest.Get(API_URL);
            Logg.Log("Getting real datetime...");
            yield return webRequest.SendWebRequest();

            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                Logg.LogError(webRequest.error);
                //_currentDateTime = DateTime.Now;
            }
            else
            {
                // Success
                TimeData timeData = JsonUtility.FromJson<TimeData>(webRequest.downloadHandler.text);
                // timeData.datetime value is : 2020-08-14T15:54:04+01:00
                _currentDateTime = ParseDateTime(timeData.datetime);
                Logg.Log("Get time success: " + _currentDateTime, ColorCustom.Green);
            }
        }

        //datetime format => 2020-08-14T15:54:04+01:00
        private DateTime ParseDateTime(string datetime)
        {
            //match 0000-00-00
            string date = Regex.Match(datetime, @"^\d{4}-\d{2}-\d{2}").Value;
            //match 00:00:00
            string time = Regex.Match(datetime, @"\d{2}:\d{2}:\d{2}").Value;
            return DateTime.Parse(string.Format("{0} {1}", date, time));
        }
    }
}


/* API (json)
{
    "abbreviation" : "+01",
    "client_ip"    : "190.107.125.48",
    "datetime"     : "2020-08-14T15:544:04+01:00",
    "dst"          : false,
    "dst_from"     : null,
    "dst_offset"   : 0,
    "dst_until"    : null,
    "raw_offset"   : 3600,
    "timezone"     : "Asia/Brunei",
    "unixtime"     : 1595601262,
    "utc_datetime" : "2020-08-14T15:54:04+00:00",
    "utc_offset"   : "+01:00"
}

We only need "datetime" property.
*/