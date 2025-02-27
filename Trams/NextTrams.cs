using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Trams.Models;
using System.Collections.Generic;
using System.Text;
using Alexa.NET.Request;
using Alexa.NET.Response;
using Alexa.NET;

namespace Trams
{
    public static class NextTrams
    {
        [FunctionName("NextTrams")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            var json = await req.ReadAsStringAsync();
            var skillRequest = JsonConvert.DeserializeObject<SkillRequest>(json);
            SkillResponse skillResponse = null;

            string[] stopIds = { "1220430", "1220431", "1122414" };

            List<Stop> stops = new List<Stop>();
            //string input = req.Query["count"];
            // int count;
            //int.TryParse(input, out count);

            foreach (var stop in stopIds)
            {
                var data = await Utils.Data.FetchStopDataAsync(stop);
                stops.Add(data);
            }

            var sb = new StringBuilder();
            foreach (var stop in stops)
            {
                for (int i = 0; i < stop.StoptimesWithoutPatterns.Count; i++)
                {
                    var stopTime = stop.StoptimesWithoutPatterns[i];
                    if (i == 0)
                    {
                        sb.Append("The next tram number " + stopTime.Trip.RouteShortName + " from " + stop.Name + " to " + stopTime.Trip.TripHeadsign);
                    }
                    else
                    {
                        sb.Append("and the one after that");
                    }
                    if (stopTime.Realtime)
                    {
                        sb.Append(" leaves ");
                    }
                    else
                    {
                        sb.Append(" is scheduled to leave ");
                    }
                    var minutes = stopTime.Minutes();
                    sb.Append("in " + minutes + (minutes != 1 ? " minutes" : " minute") + " at ");
                    if (stopTime.Realtime)
                    {
                        sb.Append(stopTime.RealtimeDepartureTime().Value.ToShortTimeString().Replace(".",":"));
                    }
                    else
                    {
                        sb.Append(stopTime.ScheduledDepartureTime().ToShortTimeString().Replace(".", ":"));
                    }
                    if (i + 1 >= stop.StoptimesWithoutPatterns.Count)
                    {
                        sb.Append(".");
                    }
                    else
                    {
                        sb.Append(" ");
                    }
                }
                sb.Append(Environment.NewLine);
            }

            skillResponse = ResponseBuilder.Tell(sb.ToString());
            skillResponse.Response.ShouldEndSession = true;

            return new OkObjectResult(skillResponse);

            //return name != null
            //    ? (ActionResult)new OkObjectResult($"Hello, {name}")
            //    : new BadRequestObjectResult("Please pass a name on the query string or in the request body");
        }
    }
}
