using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System.Threading.Tasks;
using System.Net.Http;
using Android.Locations;
using ModernHttpClient;

namespace PTVGlass
{
	public class PtvApi
	{
		string ptvDevId = ""; // PTV Developer ID
		string ptvDevKey = ""; // PTV Developer Secret Key
		string ptvApiBase = "http://timetableapi.ptv.vic.gov.au"; // PTV API Base URL

		public async Task<List<Departure>> StationDepartures(int stationId, TransportType transportType, int limit)
		{
			Newtonsoft.Json.Linq.JObject departuresResult;
			Uri departuresApi = new Uri(ptvApiUrlWithSignaure("/v2/mode/"+(int)transportType+"/stop/"+stationId+"/departures/by-destination/limit/"+limit));
			departuresResult = await callApi (departuresApi);

			var departures = new List<Departure> ();
			foreach(var departure in departuresResult["values"])
			{
				departures.Add(GetDeparture(departure));
			}

			return departures;
		}

		public async Task<List<Stop>> StopsNearby(double latitude, double longitude)
		{
			Newtonsoft.Json.Linq.JArray nearbyResult;
			Uri nearbyApi = new Uri(ptvApiUrlWithSignaure("/v2/nearme/latitude/"+latitude+"/longitude/"+longitude));
			nearbyResult = await callApiArray (nearbyApi);

			var stopsNearby = new List<Stop> ();
			foreach (var stop in nearbyResult) {
				stopsNearby.Add (GetStop (stop ["result"]));
			}

			return stopsNearby;
		}

		public async void Search(string searchString)
		{
			Newtonsoft.Json.Linq.JObject searchResult;
			Uri searchApi = new Uri(ptvApiUrlWithSignaure("/v2/search/"+searchString));
			searchResult = await callApi (searchApi);
		}

		public async Task<Newtonsoft.Json.Linq.JObject> callApi (Uri uri)
		{
			var httpClient = new HttpClient(new OkHttpNetworkHandler());
			string contents = await httpClient.GetStringAsync(uri);
			return Newtonsoft.Json.Linq.JObject.Parse (contents);
		}

		public async Task<Newtonsoft.Json.Linq.JArray> callApiArray (Uri uri)
		{
			var httpClient = new HttpClient(new OkHttpNetworkHandler());
			string contents = await httpClient.GetStringAsync(uri);
			return Newtonsoft.Json.Linq.JArray.Parse (contents);
		}

		public string ptvApiUrlWithSignaure(string endpoint)
		{
			// add developer id
			string url = string.Format("{0}{1}devid={2}",endpoint,endpoint.Contains("?") ? "&" : "?",ptvDevId);
			System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
			// encode key
			byte[] keyBytes = encoding.GetBytes(ptvDevKey);
			// encode url
			byte[] urlBytes = encoding.GetBytes(url);
			byte[] tokenBytes = new System.Security.Cryptography.HMACSHA1(keyBytes).ComputeHash(urlBytes);
			var sb = new System.Text.StringBuilder();
			// convert signature to string
			Array.ForEach<byte>(tokenBytes, x => sb.Append (x.ToString("X2")));
			// add signature and api base to url
			return ptvApiBase + string.Format("{0}&signature={1}",url,sb.ToString());
		}

		public Departure GetDeparture(Newtonsoft.Json.Linq.JToken json)
		{
			return new Departure () {
				Platform = GetPlatform(json["platform"]),
				Run = GetRun(json["run"]),
				TimeTimetableUTC = (DateTime)json ["time_timetable_utc"],
				TimeRealtimeUTC = (DateTime?)json ["time_realtime_utc"],
				Flags = (string)json ["flags"]
			};
		}

		public Platform GetPlatform(Newtonsoft.Json.Linq.JToken json)
		{
			return new Platform () {
				RealtimeID = (int)json["realtime_id"],
				Stop = GetStop(json["stop"]),
				Direction = GetDirection(json["direction"])
			};
		}

		public Run GetRun(Newtonsoft.Json.Linq.JToken json)
		{
			return new Run () {
				TransportType = (TransportType)Enum.Parse (typeof(TransportType), (string)json ["transport_type"], true),
				RunID = (int)json ["run_id"],
				NumSkipped = (int)json ["num_skipped"],
				DestinationId = (int)json ["destination_id"],
				DestinationName = (string)json ["destination_name"]
			};
		}

		public Stop GetStop(Newtonsoft.Json.Linq.JToken json)
		{
			return new Stop () {
				Suburb = (string)json ["suburb"],
				TransportType = (TransportType)Enum.Parse (typeof(TransportType), (string)json ["transport_type"], true),
				StopID = (int)json ["stop_id"],
				LocationName = (string)json ["location_name"],
				Latitude = (float)json ["lat"],
				Longitud = (float)json ["lon"],
				Distance = (float)json ["distance"]
			};
		}

		public Direction GetDirection(Newtonsoft.Json.Linq.JToken json)
		{
			return new Direction () {
				LineDirectionID = (int)json ["linedir_id"],
				DirectionID = (int)json ["direction_id"],
				DirectionName = (string)json ["direction_name"],
				Line = GetLine(json["line"])
			};
		}

		public Line GetLine(Newtonsoft.Json.Linq.JToken json)
		{
			return new Line () {
				TransportType = (TransportType)Enum.Parse (typeof(TransportType), (string)json ["transport_type"], true),
				LineID = (int)json ["line_id"],
				LineName = (string)json ["line_name"],
				LineNumber = (string)json ["line_number"]
			};
		}

	}
}

