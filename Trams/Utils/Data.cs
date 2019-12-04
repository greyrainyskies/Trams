using GraphQL.Client;
using GraphQL.Common.Request;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Trams.Models;

namespace Trams.Utils
{
    class Data
    {
        const string endpoint = "https://api.digitransit.fi/routing/v1/routers/hsl/index/graphql";
        public static async Task<Stop> FetchStopDataAsync(string stopID, int numberOfDepartures = 1)
        {
            var request = new GraphQLRequest()
            {
                Query = @"query ($stop: String!, $number: Int){ stop(id: $stop) { name stoptimesWithoutPatterns(numberOfDepartures: $number) { scheduledArrival realtimeArrival arrivalDelay scheduledDeparture realtimeDeparture departureDelay realtime realtimeState serviceDay trip { routeShortName tripHeadsign pattern { name headsign } } } } }",
                Variables = new { stop = "HSL:" + stopID, number = numberOfDepartures > 0 ? numberOfDepartures : 1 }
            };

            var client = new GraphQLClient(endpoint);

            var response = await client.PostAsync(request);

            var data = response.GetDataFieldAs<Stop>("stop");

            return data;
        }
    }
}
