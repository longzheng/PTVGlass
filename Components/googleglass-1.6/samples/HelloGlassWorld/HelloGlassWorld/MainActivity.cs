using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Glass.App;

namespace HelloGlassWorld
{
	[Activity (Label = "Hello Xamarin", Icon = "@drawable/Icon", MainLauncher = true, Enabled = true)]
	[IntentFilter (new String[]{ "com.google.android.glass.action.VOICE_TRIGGER" })]
	[MetaData ("com.google.android.glass.VoiceTrigger", Resource = "@xml/voicetriggerstart")]
	public class MainActivity : Activity
	{
		// The project requires the Google Glass Component from
		// https://components.xamarin.com/view/googleglass
		// so make sure you add that in to compile succesfully.
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Set our view from the GDK Card API
			var card = new Card (this);
			card.SetText ("Welcome to Xamarin Google Glass Development");
			card.SetFootnote ("Let's get hacking!");
			SetContentView (card.View);
		}
	}
}


