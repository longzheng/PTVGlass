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
	public class CountDownView : FrameLayout
	{
		const String TAG = "CountDownView";

		public event Action<long> OnTick;
		public event Action OnFinish;

		/* Time delimiter specifying when the second component is fully shown. */
		public const float ANIMATION_DURATION_IN_MILLIS = 850.0f;

		// About 24 FPS.
		const long DELAY_MILLIS = 41;
		const int MAX_TRANSLATION_Y = 30;
		const float ALPHA_DELIMITER = 0.95f;
		long SEC_TO_MILLIS = TimeUnit.Seconds.ToMillis(1);

		private TextView secondsView;
		private Handler handler = new Handler();

		private long stopTimeInFuture;
		private bool started;

		private long timeSeconds;
		public long Countdown {
			get {
				return timeSeconds;
			}
			set {
				timeSeconds = value;
			}
		}

		public CountDownView(Context context) : this(context, null, 0) 
		{
		}

		public CountDownView(Context context, IAttributeSet attrs) : this(context, attrs, 0) 
		{
		}

		public CountDownView(Context context, IAttributeSet attrs, int style) : base (context, attrs, style) 
		{
			LayoutInflater.From(context).Inflate(Resource.Layout.card_countdown, this);

			secondsView =  (TextView) FindViewById(Resource.Id.seconds_view);
		}

		public void Start()
		{
			if (!started) {
				stopTimeInFuture =
					TimeUnit.Seconds.ToMillis(timeSeconds) + SystemClock.ElapsedRealtime();
				started = true;
				handler.PostDelayed(UpdateViewRunnable, DELAY_MILLIS);
			}
		}

		void UpdateView (long millisUntilFinish)
		{
			long currentTimeSeconds = TimeUnit.Milliseconds.ToSeconds(millisUntilFinish) + 1;
			long frame = SEC_TO_MILLIS - (millisUntilFinish % SEC_TO_MILLIS);

			secondsView.Text = currentTimeSeconds.ToString();
			if (frame <= ANIMATION_DURATION_IN_MILLIS) {
				float factor = frame / ANIMATION_DURATION_IN_MILLIS;
				secondsView.Alpha = factor * ALPHA_DELIMITER;
				secondsView.TranslationY = (MAX_TRANSLATION_Y * (1 - factor));
			} else {
				float factor = (frame - ANIMATION_DURATION_IN_MILLIS) / ANIMATION_DURATION_IN_MILLIS;
				secondsView.Alpha = (ALPHA_DELIMITER + factor * (1 - ALPHA_DELIMITER));
			}
		}

		void UpdateViewRunnable() 
		{
			long millisLeft = stopTimeInFuture - SystemClock.ElapsedRealtime();

			// Count down is done.
			if (millisLeft <= 0) {
				started = false;
				OnFinish ();
			} else {
				UpdateView(millisLeft);
				OnTick(millisLeft);
				handler.PostDelayed(UpdateViewRunnable, DELAY_MILLIS);
			}
		}
	}
}

