using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Glass.App;
using GlassProgressBar;

namespace GlassProgressTest
{
	[Activity (Label = "GlassProgressTest", Icon = "@drawable/Icon", MainLauncher = true, Enabled = true)]
	[IntentFilter (new String[]{ "com.google.android.glass.action.VOICE_TRIGGER" })]
	[MetaData ("com.google.android.glass.VoiceTrigger", Resource = "@xml/voicetriggerstart")]
	public class MainActivity : Activity
	{
		private SliderView mProgress;
		private SliderView mIndeterm;

		// The project requires the Google Glass Component from
		// https://components.xamarin.com/view/googleglass
		// so make sure you add that in to compile succesfully.
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			SetContentView(Resource.Layout.activity_main);
			mProgress = (SliderView) FindViewById(Resource.Id.progress_slider);
			mIndeterm = (SliderView) FindViewById(Resource.Id.indeterm_slider);

			mIndeterm.StartIndeterminate();
			mProgress.StartProgress(10 * 1000);
		}
	}
}


