using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace SampleStopwatch
{
	[Activity (Label = "@string/app_name", MainLauncher = true, Theme = "@style/MenuTheme", Enabled = true)]
	public class MenuActivity : Activity
	{
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
		}

		protected override void OnResume ()
		{
			base.OnResume ();

			OpenOptionsMenu ();
		}

		public override bool OnCreateOptionsMenu (IMenu menu)
		{
			MenuInflater.Inflate(Resource.Menu.stopwatch, menu);
			return true;
		}

		public override bool OnOptionsItemSelected (IMenuItem item)
		{
			// Handle item selection.
			switch (item.ItemId) {
			case Resource.Id.stop:
				StopService(new Intent(this, typeof(StopwatchService)));
				return true;
			default:
				return base.OnOptionsItemSelected (item);
			}
		}

		public override void OnOptionsMenuClosed (IMenu menu)
		{
			Finish ();
		}
	}
}


