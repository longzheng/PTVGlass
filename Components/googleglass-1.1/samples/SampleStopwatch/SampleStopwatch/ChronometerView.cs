using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Java.Util.Concurrent;

namespace SampleStopwatch
{
	public class ChronometerView : FrameLayout
	{
		public event Action OnChange;

		// About 24 FPS.
		const long DELAY_MILLIS = 41;

		TextView mMinuteView;
		TextView mSecondView;
		TextView mCentiSecondView;

		bool started;

		private Handler handler = new Handler();

		private long baseMillis;
		public long BaseMillis {
			get {
				return baseMillis;
			}
			set {
				baseMillis = value;
				UpdateText ();
			}
		}

		private bool forceStart;
		public bool ForceStart {
			get {
				return forceStart;
			}
			set {
				forceStart = value;
				UpdateRunning ();
			}
		}

		bool visible;
		bool Running;

		public ChronometerView (Context context) : this (context, null, 0)
		{
		}

		public ChronometerView (Context context, IAttributeSet attrs) : this (context, attrs, 0)
		{
		}

		public ChronometerView (Context context, IAttributeSet attrs, int defStyle) : base (context, attrs, defStyle)
		{
			LayoutInflater.From(context).Inflate(Resource.Layout.card_chronometer, this);

			mMinuteView = (TextView) FindViewById(Resource.Id.minute);
			mSecondView = (TextView) FindViewById(Resource.Id.second);
			mCentiSecondView = (TextView) FindViewById(Resource.Id.centi_second);

			baseMillis = SystemClock.ElapsedRealtime ();

		}

		/*
	     * Start the chronometer.
	     */
		public void Start() 
		{
			started = true;
			UpdateRunning();
		}

		/*
	     * Stop the chronometer.
	     */
		public void Stop() 
		{
			started = false;
			UpdateRunning();
		}

		protected override void OnDetachedFromWindow ()
		{
			base.OnDetachedFromWindow ();
			visible = false;
			UpdateRunning();

		}

		protected override void OnWindowVisibilityChanged (ViewStates visibility)
		{
			base.OnWindowVisibilityChanged (visibility);
			visible = (visibility == ViewStates.Visible);
			UpdateRunning();
		}

		void UpdateTextRunnable() 
		{
			if (Running) {
				UpdateText();
				handler.PostDelayed(UpdateTextRunnable, DELAY_MILLIS);
			}
		}

		void UpdateRunning ()
		{
			bool running = (visible || forceStart) && started;
			Console.WriteLine ("Visible = " + visible);
			Console.WriteLine ("Force Start = " + forceStart);
			Console.WriteLine ("Started " + started);
			Console.WriteLine ("Running = " + running);
			Console.WriteLine ("Running class = " + Running);
			if (running != Running) {
				if (running) {
					handler.Post(UpdateTextRunnable);
				} else {
					handler.RemoveCallbacks(UpdateTextRunnable);
				}
				Running = running;
			}
		}

		void UpdateText ()
		{
			long millis = SystemClock.ElapsedRealtime() - BaseMillis;
			// Cap chronometer to one hour.
			millis %= TimeUnit.Hours.ToMillis(1);

			mMinuteView.Text = (String.Format("{0:D2}", TimeUnit.Milliseconds.ToMinutes(millis)));
			millis %= TimeUnit.Minutes.ToMillis(1);
			mSecondView.Text = (String.Format("{0:D2}", TimeUnit.Milliseconds.ToSeconds(millis)));
			millis = (millis % TimeUnit.Seconds.ToMillis(1)) / 10;
			mCentiSecondView.Text = (String.Format("{0:D2}", millis));
			OnChange();
		}
	}
}

