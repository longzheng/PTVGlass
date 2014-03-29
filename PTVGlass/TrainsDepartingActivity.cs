using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Glass.App;
using System.Security.Cryptography;
using System.Net;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using Android.Text.Format;

namespace PTVGlass
{
	// We need multiple activities because Google Glass's launcher uses activities to display menu options
	// We can simulate a voice sub-menu by defining multiple activities with the same voice trigger but different labels
	// Adding too many activites however seem to crash Glass's Home launcher

	[Activity (Label = "Southern Cross", Icon = "@drawable/TrainIcon", MainLauncher = true, Enabled = true)]
	[IntentFilter (new String[]{ "com.google.android.glass.action.VOICE_TRIGGER" })]
	[MetaData ("com.google.android.glass.VoiceTrigger", Resource = "@xml/voice_trainsdepartingtrigger")]
	public class SouthernCrossActivity : Activity
	{
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			Intent departureActivity = new Intent (this, typeof(TrainDepartureActivity));
			departureActivity.PutExtra ("stationId", "1181");
			StartActivity (departureActivity);
			Finish ();
		}
	}

	[Activity (Label = "Flinders Street", Icon = "@drawable/TrainIcon", MainLauncher = true, Enabled = true)]
	[IntentFilter (new String[]{ "com.google.android.glass.action.VOICE_TRIGGER" })]
	[MetaData ("com.google.android.glass.VoiceTrigger", Resource = "@xml/voice_trainsdepartingtrigger")]
	public class FlindersStreetActivity : Activity
	{
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			Intent departureActivity = new Intent (this, typeof(TrainDepartureActivity));
			departureActivity.PutExtra ("stationId", "1071");
			StartActivity (departureActivity);
			Finish ();
		}
	}

	[Activity (Label = "Springvale", Icon = "@drawable/TrainIcon", MainLauncher = true, Enabled = true)]
	[IntentFilter (new String[]{ "com.google.android.glass.action.VOICE_TRIGGER" })]
	[MetaData ("com.google.android.glass.VoiceTrigger", Resource = "@xml/voice_trainsdepartingtrigger")]
	public class SpringvaleActivity : Activity
	{
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			Intent departureActivity = new Intent (this, typeof(TrainDepartureActivity));
			departureActivity.PutExtra ("stationId", "1183");
			StartActivity (departureActivity);
			Finish ();
		}
	}

	[Activity (Label = "Flagstaff", Icon = "@drawable/TrainIcon", MainLauncher = true, Enabled = true)]
	[IntentFilter (new String[]{ "com.google.android.glass.action.VOICE_TRIGGER" })]
	[MetaData ("com.google.android.glass.VoiceTrigger", Resource = "@xml/voice_trainsdepartingtrigger")]
	public class FlagstaffActivity : Activity
	{
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			Intent departureActivity = new Intent (this, typeof(TrainDepartureActivity));
			departureActivity.PutExtra ("stationId", "1068");
			StartActivity (departureActivity);
			Finish ();
		}
	}

	[Activity (Label = "Melbourne Central", Icon = "@drawable/TrainIcon", MainLauncher = true, Enabled = true)]
	[IntentFilter (new String[]{ "com.google.android.glass.action.VOICE_TRIGGER" })]
	[MetaData ("com.google.android.glass.VoiceTrigger", Resource = "@xml/voice_trainsdepartingtrigger")]
	public class TrainStation1120Activity : Activity
	{
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			Intent departureActivity = new Intent (this, typeof(TrainDepartureActivity));
			departureActivity.PutExtra ("stationId", "1120");
			StartActivity (departureActivity);
			Finish ();
		}
	}

	[Activity (Label = "Parliament", Icon = "@drawable/TrainIcon", MainLauncher = true, Enabled = true)]
	[IntentFilter (new String[]{ "com.google.android.glass.action.VOICE_TRIGGER" })]
	[MetaData ("com.google.android.glass.VoiceTrigger", Resource = "@xml/voice_trainsdepartingtrigger")]
	public class TrainStation1155Activity : Activity
	{
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			Intent departureActivity = new Intent (this, typeof(TrainDepartureActivity));
			departureActivity.PutExtra ("stationId", "1155");
			StartActivity (departureActivity);
			Finish ();
		}
	}

	[Activity (Label = "Richmond", Icon = "@drawable/TrainIcon", MainLauncher = true, Enabled = true)]
	[IntentFilter (new String[]{ "com.google.android.glass.action.VOICE_TRIGGER" })]
	[MetaData ("com.google.android.glass.VoiceTrigger", Resource = "@xml/voice_trainsdepartingtrigger")]
	public class TrainStation1162Activity : Activity
	{
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			Intent departureActivity = new Intent (this, typeof(TrainDepartureActivity));
			departureActivity.PutExtra ("stationId", "1162");
			StartActivity (departureActivity);
			Finish ();
		}
	}
}


