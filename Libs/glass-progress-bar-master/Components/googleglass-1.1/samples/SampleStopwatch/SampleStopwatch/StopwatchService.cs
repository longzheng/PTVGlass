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
using Android.Util;

using Android.Glass.Timeline;

namespace SampleStopwatch
{
	[Service(Icon = "@drawable/ic_lap", Label = "@string/app_name", Enabled = true, Exported = true)]
	[IntentFilter(new String[]{"com.google.android.glass.action.VOICE_TRIGGER"})]
	[MetaData("com.google.android.glass.VoiceTrigger", Resource = "@xml/voice_trigger_start")]
	public class StopwatchService : Service
	{
		const String TAG = "StopwatchService";
		const String LIVE_CARD_ID = "stopwatch";

		private ChronometerDrawer callback;
		private TimelineManager timelineManager;
		private LiveCard liveCard;

		public override void OnCreate ()
		{
			base.OnCreate ();
			timelineManager = TimelineManager.From (this);
		}

		public override IBinder OnBind (Intent intent)
		{
			return null;
		}

		public override StartCommandResult OnStartCommand (Intent intent, StartCommandFlags flags, int startId)
		{
			if (liveCard == null) {
				Log.Debug(TAG, "Publishing LiveCard");
				liveCard = timelineManager.CreateLiveCard(LIVE_CARD_ID);

				// Keep track of the callback to remove it before unpublishing.
				callback = new ChronometerDrawer(this);
				liveCard.SetDirectRenderingEnabled (true).SurfaceHolder.AddCallback (callback);

				Intent menuIntent = new Intent(this, typeof(MenuActivity));
				liveCard.SetAction(PendingIntent.GetActivity(this, 0, menuIntent, 0));

				liveCard.Publish(LiveCard.PublishMode.Reveal);
				Log.Debug(TAG, "Done publishing LiveCard");
			} else {
				// TODO(alainv): Jump to the LiveCard when API is available.
			}

			return StartCommandResult.Sticky;
		}

		public override void OnDestroy ()
		{
			Log.Debug (TAG, "OnDestroy Xamarin Stopwatch");
			if (liveCard != null && liveCard.IsPublished) {
				Log.Debug(TAG, "Unpublishing LiveCard");
				if (callback != null) {
					liveCard.SurfaceHolder.RemoveCallback(callback);
				}
				liveCard.Unpublish();
				liveCard = null;
			}

			base.OnDestroy ();
		}
	}
}

