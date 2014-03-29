using System;
using System.IO;
using System.Collections.Generic;

namespace GenerateTrainStationActivities
{
	public class GenerateTrainStationActivities
	{
		static void Main(){

			List<TrainStation> stations = new List<TrainStation>();

			var reader = new StreamReader (File.OpenRead (@"trains.csv"));
			reader.ReadLine (); // skip first line heading

			while (!reader.EndOfStream) {
				var csvLine = reader.ReadLine ();
				var csvValues = csvLine.Split (',');

				stations.Add (new TrainStation () {
					ID = csvValues[0],
					Name = csvValues[1]
				});
			}

			foreach (var station in stations) {
				Console.WriteLine ("[Activity (Label = \""+station.Name+"\", Icon = \"@drawable/TrainIcon\", MainLauncher = true, Enabled = true)]");
				Console.WriteLine ("[IntentFilter (new String[]{ \"com.google.android.glass.action.VOICE_TRIGGER\" })]");
				Console.WriteLine ("[MetaData (\"com.google.android.glass.VoiceTrigger\", Resource = \"@xml/voice_trainsdepartingtrigger\")]");
				Console.WriteLine ("public class TrainStation"+station.ID+"Activity : Activity");
				Console.WriteLine ("{");
				Console.WriteLine ("protected override void OnCreate (Bundle bundle)");
				Console.WriteLine ("{");
				Console.WriteLine ("base.OnCreate (bundle);");
				Console.WriteLine ("");
				Console.WriteLine ("Intent departureActivity = new Intent (this, typeof(DepartureActivity));");
				Console.WriteLine ("departureActivity.PutExtra (\"stationId\", \""+station.ID+"\");");
				Console.WriteLine ("StartActivity (departureActivity);");
				Console.WriteLine ("Finish ();");
				Console.WriteLine ("}");
				Console.WriteLine ("}");
			}

		}
	}

	public class TrainStation {
		public string Name { get;set; }
		public string ID { get;set;}
	}
}

