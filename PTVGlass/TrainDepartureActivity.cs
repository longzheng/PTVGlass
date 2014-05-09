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
using GlassProgressBar;
using Android.Glass.App;

namespace PTVGlass
{
	[Activity]
	public class TrainDepartureActivity : Activity
	{

		// The project requires the Google Glass Component from
		// https://components.xamarin.com/view/googleglass
		// so make sure you add that in to compile succesfully.
		protected override async void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Show loading screen
 			SetContentView (Resource.Layout.LoadingScreen);
 			var loadingText = FindViewById<TextView> (Resource.Id.loading_text);
 			loadingText.SetText(Resource.String.getting_departures); // set loading text
 			var progressBar = FindViewById<SliderView> (Resource.Id.indeterm_slider);
 			progressBar.StartIndeterminate (); // start indeterminate progress bar
 

			// get station ID from intent
			int intentStationId = int.Parse(Intent.GetStringExtra ("stationId"));

			// try to call PTV API to get station departures
			List<Departure> stationDepartures;
			try{
				var ptvApi = new PtvApi();
				stationDepartures = await ptvApi.StationDepartures(intentStationId, TransportType.Train, 1);
			}catch(Exception e){
				// show error card
				var errorCard = new Card(this);
				errorCard.SetText (e.ToString());
				SetContentView (errorCard.View);

				return;
			}

			// if there are no departures, show no departure message
			if (stationDepartures.Count == 0) {
				// Show error screen
				SetContentView (Resource.Layout.ErrorScreen);
				var errorText = FindViewById<TextView> (Resource.Id.error_text);
				errorText.SetText(Resource.String.no_upcoming_departures); // set error text

				return;
			}

			// show departures list screen
			ListView listView;
			SetContentView (Resource.Layout.DepartureScreen);
			listView = FindViewById<ListView> (Resource.Id.listview);
			listView.Adapter = new TrainsScreenAdapter(this, stationDepartures); // bind list of station departures to listView
			listView.RequestFocus (); // set focus on the listView so scrolling works on the list
		}
	}

	public class TrainsScreenAdapter : BaseAdapter<Departure> {
		List<Departure> departures;
		Activity context;
		public TrainsScreenAdapter(Activity context, List<Departure> departures)
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
				view = context.LayoutInflater.Inflate(Resource.Layout.TrainDepartureRow, null);
			view.FindViewById<TextView>(Resource.Id.TextLine).Text = item.Platform.Direction.DirectionName;
			view.FindViewById<TextView>(Resource.Id.TextTime).Text = TimeHelper.GetRelativeTime(item.TimeTimetableUTC);
			return view;
		}
	}
}

