using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Trams.Models
{
    public class Pattern
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("headsign")]
        public string Headsign { get; set; }

        // Replaces only one instance
        public override string ToString()
        {
            return Regex.Replace(Name, "\\(HSL:[0-9]+\\)\\s", "");
        }
    }
    public class Trip
    {

        [JsonProperty("routeShortName")]
        public string RouteShortName { get; set; }

        [JsonProperty("tripHeadsign")]
        public string TripHeadsign { get; set; }

        [JsonProperty("pattern")]
        public Pattern Pattern { get; set; }

        public override string ToString()
        {
            return $"tram {RouteShortName} to {TripHeadsign}";
        }
    }

    public class StoptimesWithoutPattern
    {

        [JsonProperty("scheduledArrival")]
        public int ScheduledArrival { get; set; }

        [JsonProperty("realtimeArrival")]
        public int RealtimeArrival { get; set; }

        [JsonProperty("arrivalDelay")]
        public int ArrivalDelay { get; set; }

        [JsonProperty("scheduledDeparture")]
        public int ScheduledDeparture { get; set; }

        [JsonProperty("realtimeDeparture")]
        public int RealtimeDeparture { get; set; }

        [JsonProperty("departureDelay")]
        public int DepartureDelay { get; set; }

        [JsonProperty("realtime")]
        public bool Realtime { get; set; }

        [JsonProperty("realtimeState")]
        public string RealtimeState { get; set; }

        [JsonProperty("serviceDay")]
        public int ServiceDay { get; set; }

        [JsonProperty("trip")]
        public Trip Trip { get; set; }

        public DateTime ScheduledDepartureTime()
        {
            return ToLocalTime(ServiceDay, ScheduledDeparture);
        }

        public DateTime? RealtimeDepartureTime()
        {
            if (RealtimeState == "UPDATED")
            {
                return ToLocalTime(ServiceDay, RealtimeDeparture);
            }
            else
            {
                return null;
            }
        }

        static TimeZoneInfo tz = TimeZoneInfo.FindSystemTimeZoneById("FLE Standard Time");

        public int Minutes()
        {
            if (this.Realtime)
            {
                return (RealtimeDepartureTime().Value - TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, tz)).Minutes;
            }
            else
            {
                return (ScheduledDepartureTime() - TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, tz)).Minutes;
            }
        }
        DateTime ToLocalTime(int day, int time)
        {
            var utc = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc).AddSeconds(day + time);
            return TimeZoneInfo.ConvertTimeFromUtc(utc, tz);
        }

        public bool? IsLate()
        {
            if (RealtimeState == "UPDATED")
            {
                return DepartureDelay > 60;
            }
            else
            {
                return null;
            }
        }

        
    }

    public class Stop
    {

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("stoptimesWithoutPatterns")]
        public List<StoptimesWithoutPattern> StoptimesWithoutPatterns { get; set; }
    }
}
