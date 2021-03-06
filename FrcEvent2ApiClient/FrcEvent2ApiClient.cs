﻿using FrcEvent2ApiClient.FrcObjects.MatchAPI;
using FrcEvent2ApiClient.FrcObjects.RankingAPI;
using FrcEvent2ApiClient.FrcObjects.ScheduleAPI;
using FrcEvent2ApiClient.FrcObjects.TeamAPI;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using Windows.Web.Http;

namespace FrcEvent2ApiClient
{
    public static class FrcEvent2ApiClient
    {
//        const string BaseUrl = "https://frc-api.firstinspires.org/v2.0/";  // Production URL for APIs
        const string BaseUrl = "https://frc-staging-api.firstinspires.org/v2.0/"; // Staging URL for APIs
        const string Season = "2016";
        const string Token = "";
        const string TeamNumberDefault = "2147";

        public static async Task<TeamData> GetTeamData(string teamNumber)
        {
            int i = 0;
            if (teamNumber.CompareTo("") == 0 || !int.TryParse(teamNumber, out i))
            {
                teamNumber = "2147";
            }

            var response = await GetAsyncFromFirst(new Uri(BaseUrl + Season + "/teams?teamNumber=" + teamNumber));
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(TeamData));
            using (var ms = new MemoryStream(Encoding.Unicode.GetBytes(response)))
            {
                return (TeamData)serializer.ReadObject(ms); // serialize the data into TeamData
            }
        }

        public static async Task<string> GetEventMatchesForTeam(string eventCode, string teamNumber)
        {
            if (eventCode.CompareTo("") == 0)
            {
                eventCode = "WAELL";
            }
            var schedule = await GetAsyncFromFirst(new Uri(BaseUrl + Season + "/schedule/" + eventCode + "?teamNumber=" + teamNumber));
            var results = await GetAsyncFromFirst(new Uri(BaseUrl + Season + "/matches/" + eventCode + "?teamNumber=" + teamNumber));
            return schedule;
        }

        public static async Task<string> GetEventMatches(string eventCode)
        {
            if (eventCode.CompareTo("") == 0)
            {
                eventCode = "WAELL";
            }
            var schedule = await GetAsyncFromFirst(new Uri(BaseUrl + Season + "/schedule/" + eventCode));
            var results = await GetAsyncFromFirst(new Uri(BaseUrl + Season + "/matches/" + eventCode));
            return schedule;
        }

        public static async Task<List<RankingData>> GetEventRankingList(string eventCode)
        {
            if (eventCode.CompareTo("") == 0)
            {
                eventCode = "WAELL";
            }

            var response = await GetAsyncFromFirst(new Uri(BaseUrl + Season + "/ranking/" + eventCode));
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(RankingData));
            using (var ms = new MemoryStream(Encoding.Unicode.GetBytes(response)))
            {
                return (List<RankingData>)serializer.ReadObject(ms); // serialize the data into TeamData
            }
        }

        public static async Task<List<RankingData>> GetEventRankingForTeam(string eventCode, string teamNumber)
        {
            int i = 0;
            if (teamNumber.CompareTo("") == 0 || !int.TryParse(teamNumber, out i))
            {
                teamNumber = "2147";
            }

            if (eventCode.CompareTo("") == 0)
            {
                eventCode = "WAELL";
            }

            var response = await GetAsyncFromFirst(new Uri(BaseUrl + Season + "/ranking/" + eventCode + "?teamNumber=" + teamNumber));
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(RankingData));
            using (var ms = new MemoryStream(Encoding.Unicode.GetBytes(response)))
            {
                return (List<RankingData>)serializer.ReadObject(ms); // serialize the data into TeamData
            }
        }

        private static async Task<string> GetAsyncFromFirst(Uri uri)
        {
            string responseData = "";
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.TryAppendWithoutValidation("accept", "application/json");
                httpClient.DefaultRequestHeaders.TryAppendWithoutValidation("authorization", "Basic " + Token);
                using (var response = await httpClient.GetAsync(uri))
                {
                    responseData = await response.Content.ReadAsStringAsync();
                }
            }
            return responseData;
        }
    }
}
