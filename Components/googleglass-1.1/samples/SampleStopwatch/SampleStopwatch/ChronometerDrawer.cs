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
using Java.Util.Concurrent;
using Android.Media;
using Android.Graphics;
using Android.Util;

namespace SampleStopwatch
{
	public class ChronometerDrawer : Java.Lang.Object, ISurfaceHolderCallback
	{
		const String TAG = "ChronometerDrawer";

		long SEC_TO_MILLIS = TimeUnit.Seconds.ToMillis(1);
		const int SOUND_PRIORITY = 1;
		const int MAX_STREAMS = 1;
		const int COUNT_DOWN_VALUE = 3;

		SoundPool soundPool;
		int startSoundId;
		int countDownSoundId;

		CountDownView countDownView;
		ChronometerView chronometerView;

		long _currentTimeSeconds;
		bool countDownSoundPlayed;

		ISurfaceHolder _holder;
		bool countDownDone;

		public ChronometerDrawer(Context context) 
		{
			countDownView = new CountDownView(context);
			countDownView.Countdown = (COUNT_DOWN_VALUE);
			countDownView.OnTick += (long millisUntilFinish) => {
				MaybePlaySound(millisUntilFinish);
				Draw(countDownView);
			};
			countDownView.OnFinish +=  () => {
				countDownDone = true;
				chronometerView.BaseMillis = (SystemClock.ElapsedRealtime());
				if (_holder != null) {
					chronometerView.Start();
				}
				PlaySound(startSoundId);
			};

			chronometerView = new ChronometerView(context);
			chronometerView.OnChange += () => {
				Draw(chronometerView);
			};
			chronometerView.ForceStart = true;

			soundPool = new SoundPool (MAX_STREAMS, Stream.Music, 0);
			startSoundId = soundPool.Load (context, Resource.Raw.start, SOUND_PRIORITY);
			countDownSoundId = soundPool.Load (context, Resource.Raw.countdown_bip, SOUND_PRIORITY);

		}

		public void SurfaceChanged (ISurfaceHolder holder, Android.Graphics.Format format, int width, int height)
		{
			// Measure and layout the view with the canvas dimensions.
			int measuredWidth = View.MeasureSpec.MakeMeasureSpec(width, MeasureSpecMode.Exactly);
			int measuredHeight = View.MeasureSpec.MakeMeasureSpec(height, MeasureSpecMode.Exactly);

			countDownView.Measure(measuredWidth, measuredHeight);
			countDownView.Layout(
				0, 0, countDownView.MeasuredWidth, countDownView.MeasuredHeight);

			chronometerView.Measure(measuredWidth, measuredHeight);
			chronometerView.Layout(
				0, 0, chronometerView.MeasuredWidth, chronometerView.MeasuredHeight);
		}

		public void SurfaceCreated (ISurfaceHolder holder)
		{
			Log.Debug(TAG, "Surface created");
			_holder = holder;
			if (countDownDone) {
				chronometerView.Start();
			} else {
				countDownView.Start();
			}
		}

		public void SurfaceDestroyed (ISurfaceHolder holder)
		{
			Log.Debug(TAG, "Surface destroyed");
			chronometerView.Stop();
			_holder = null;
		}

		void MaybePlaySound (long millisUntilFinish)
		{
			long currentTimeSeconds = TimeUnit.Milliseconds.ToSeconds(millisUntilFinish);
			long milliSecondsPart = millisUntilFinish % SEC_TO_MILLIS;

			if (currentTimeSeconds != _currentTimeSeconds) {
				countDownSoundPlayed = false;
				_currentTimeSeconds = currentTimeSeconds;
			}
			if (!countDownSoundPlayed && (milliSecondsPart <= CountDownView.ANIMATION_DURATION_IN_MILLIS)) {
				PlaySound(countDownSoundId);
				countDownSoundPlayed = true;
			}
		}

		void Draw (View view)
		{
			Canvas canvas;
			try {
				canvas = _holder.LockCanvas();
			} catch (Exception e) {
				return;
			}
			if (canvas != null) {
				view.Draw(canvas);
				_holder.UnlockCanvasAndPost(canvas);
			}
		}

		void PlaySound (int startSoundId)
		{
			soundPool.Play(startSoundId,
				1 /* leftVolume */,
				1 /* rightVolume */,
				SOUND_PRIORITY,
				0 /* loop */,
				1 /* rate */);
		}
	}
}

