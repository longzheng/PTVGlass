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
	[Activity (Icon = "@drawable/BusIcon", MainLauncher = true, Enabled = true)]
	[IntentFilter (new String[]{ "com.google.android.glass.action.VOICE_TRIGGER" })]
	[MetaData ("com.google.android.glass.VoiceTrigger", Resource = "@xml/voice_bussesnearmetrigger")]
	public class BussesNearMeActivity : Activity
	{
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			Intent nearMeActivity = new Intent (this, typeof(NearMeActivity));
			nearMeActivity.PutExtra ("type", "bus");
			StartActivity (nearMeActivity);
			Finish ();
		}
	}

	[Activity (Icon = "@drawable/TrainIcon", MainLauncher = true, Enabled = true)]
	[IntentFilter (new String[]{ "com.google.android.glass.action.VOICE_TRIGGER" })]
	[MetaData ("com.google.android.glass.VoiceTrigger", Resource = "@xml/voice_trainsnearmetrigger")]
	public class TrainsNearMeActivity : Activity
	{
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			Intent nearMeActivity = new Intent (this, typeof(NearMeActivity));
			nearMeActivity.PutExtra ("type", "train");
			StartActivity (nearMeActivity);
			Finish ();
		}
	}

	[Activity (Icon = "@drawable/TramIcon", MainLauncher = true, Enabled = true)]
	[IntentFilter (new String[]{ "com.google.android.glass.action.VOICE_TRIGGER" })]
	[MetaData ("com.google.android.glass.VoiceTrigger", Resource = "@xml/voice_tramsnearmetrigger")]
	public class TramsNearMeActivity : Activity
	{
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			Intent nearMeActivity = new Intent (this, typeof(NearMeActivity));
			nearMeActivity.PutExtra ("type", "tram");
			StartActivity (nearMeActivity);
			Finish ();
		}
	}
}

