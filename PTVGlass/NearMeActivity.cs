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
using Android.Locations;
using GlassProgressBar;
using Android.Glass.App;

namespace PTVGlass
{
	[Activity]
	public class NearMeActivity : Activity, ILocationListener
	{
		LocationManager locationManager;
		TransportType transportType;

		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);

			// get transport type from intent
			transportType = (TransportType)Enum.Parse(typeof(TransportType), Intent.GetStringExtra("type"), true);

			// Show loading screen
			SetContentView(Resource.Layout.LoadingScreen);
			var loadingText = FindViewById<TextView>(Resource.Id.loading_text);
			loadingText.SetText(Resource.String.getting_location); // set loading text
			var progressBar = FindViewById<SliderView>(Resource.Id.indeterm_slider);
			progressBar.StartIndeterminate(); // start indeterminate progress bar

			// set up Android location manager
			locationManager = GetSystemService(Context.LocationService) as LocationManager;
			Criteria locationCriteria = new Criteria()
			{
				Accuracy = Accuracy.Coarse,
				AltitudeRequired = false
			};
			IList<string> providers = locationManager.GetProviders(locationCriteria, true);

			foreach (string provider in providers)
			{
				locationManager.RequestLocationUpdates(provider, 1000, 1, this);
			}
		}

		public async void NearbyDepartures(Location location)
		{
			// Show loading screen
			SetContentView(Resource.Layout.LoadingScreen);
			var loadingText = FindViewById<TextView>(Resource.Id.loading_text);
			loadingText.SetText(Resource.String.getting_stopsnearyou); // set loading text
			var progressBar = FindViewById<SliderView>(Resource.Id.indeterm_slider);
			progressBar.StartIndeterminate(); // start indeterminate progress bar

			var ptvApi = new PtvApi();
			var stopsNearby = await ptvApi.StopsNearby(location.Latitude, location.Longitude);

			int stopLimit = 1;
			switch (transportType)
			{
				case TransportType.Bus:
				case TransportType.Tram:
					stopLimit = 3;
					break;
				case TransportType.Train:
					stopLimit = 1;
					break;
			}

			stopsNearby = stopsNearby.Where(x =>
				x.TransportType == transportType
			).Take(stopLimit).ToList();

			// if there are no stops nearby, show no stops message
			if (stopsNearby.Count == 0)
			{
				var noStopsCard = new Card(this);
				noStopsCard.SetText("No " + transportType.ToString().ToLower() + " stops nearby");
				SetContentView(noStopsCard.ToView());
				return;
			}

			// Update loading text
			loadingText.SetText(Resource.String.getting_departures);

			List<Departure> nearByDepartures = new List<Departure>();
			foreach (Stop stop in stopsNearby)
			{
				nearByDepartures.AddRange(await ptvApi.StationDepartures(stop.StopID, transportType, 1));
			}

			// if there are no departures, show no departure message
			if (nearByDepartures.Count == 0)
			{
				var noDeparturesCard = new Card(this);
				noDeparturesCard.SetText("No upcoming departures scheduled");
				SetContentView(noDeparturesCard.ToView());
				return;
			}

			// show departures list screen
			ListView listView;
			SetContentView(Resource.Layout.DepartureScreen);
			listView = FindViewById<ListView>(Resource.Id.listview);
			// get the right type of screen adapter for the right type of transport
			if (transportType == TransportType.Train)
			{
				// we don't need the train "number" for nearby trains
				listView.Adapter = new NearbyTrainScreenAdapter(this, nearByDepartures); // bind list of station departures to listView
			}
			else
			{
				listView.Adapter = new NearbyBusTramScreenAdapter(this, nearByDepartures); // bind list of station departures to listView
			}
			listView.RequestFocus(); // set focus on the listView so scrolling works on the list
		}

		public void OnProviderEnabled(string provider)
		{
		}

		public void OnProviderDisabled(string provider)
		{
		}

		public void OnStatusChanged(string provider, Availability status, Bundle extras)
		{
		}

		public void OnLocationChanged(Location location)
		{
			if (location.Accuracy < 500)
			{
				locationManager.RemoveUpdates(this); // stop getting updates to save battery

				NearbyDepartures(location);
			}
		}
	}

	public class NearbyBusTramScreenAdapter : BaseAdapter<Departure>
	{
		List<Departure> departures;
		Activity context;
		public NearbyBusTramScreenAdapter(Activity context, List<Departure> departures)
			: base()
		{
			this.context = context;
			this.departures = departures;
		}
		public override long GetItemId(int position)
		{
			return position;
		}
		public override Departure this[int position]
		{
			get { return departures[position]; }
		}
		public override int Count
		{
			get { return departures.Count; }
		}
		public override View GetView(int position, View convertView, ViewGroup parent)
		{
			var item = departures[position];
			View view = convertView;
			if (view == null) // no view to re-use, create new
				view = context.LayoutInflater.Inflate(Resource.Layout.BusTramDepartureWithStationRow, null);
			view.FindViewById<TextView>(Resource.Id.TextLineNum).Text = item.Platform.Direction.Line.LineNumber;
			view.FindViewById<TextView>(Resource.Id.TextLine).Text = item.Platform.Direction.DirectionName;
			view.FindViewById<TextView>(Resource.Id.TextTime).Text = TimeHelper.GetRelativeTime(item.TimeTimetableUTC);
			view.FindViewById<TextView>(Resource.Id.TextStop).Text = item.Platform.Stop.LocationName;
			return view;
		}
	}

	public class NearbyTrainScreenAdapter : BaseAdapter<Departure>
	{
		List<Departure> departures;
		Activity context;
		public NearbyTrainScreenAdapter(Activity context, List<Departure> departures)
			: base()
		{
			this.context = context;
			this.departures = departures;
		}
		public override long GetItemId(int position)
		{
			return position;
		}
		public override Departure this[int position]
		{
			get { return departures[position]; }
		}
		public override int Count
		{
			get { return departures.Count; }
		}
		public override View GetView(int position, View convertView, ViewGroup parent)
		{
			var item = departures[position];
			View view = convertView;
			if (view == null) // no view to re-use, create new
				view = context.LayoutInflater.Inflate(Resource.Layout.TrainDepartureWithStationRow, null);
			view.FindViewById<TextView>(Resource.Id.TextLine).Text = item.Platform.Direction.DirectionName;
			view.FindViewById<TextView>(Resource.Id.TextTime).Text = TimeHelper.GetRelativeTime(item.TimeTimetableUTC);
			view.FindViewById<TextView>(Resource.Id.TextStop).Text = item.Platform.Stop.LocationName;
			return view;
		}
	}
}

