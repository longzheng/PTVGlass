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
using Android.Hardware;
using Android.Util;

namespace PTVGlass
{
	/// <summary>
	/// Head-tilt enabled ListView
	/// Adapted from https://github.com/pscholl/glass_snippets/blob/master/imu_listview/src/de/tud/ess/HeadListView.java
	/// </summary>

	public class HeadListView : ListView, ISensorEventListener
	{
		private static float INVALID_X = 10;
		private Sensor mSensor;
		private SensorStatus mLastAccuracy;
		private SensorManager mSensorManager;
		private float mStartX = INVALID_X;
		private static SensorDelay SENSOR_RATE_uS = SensorDelay.Ui;
		private static float VELOCITY = (float)(Math.PI / 180 * 2); // scroll one item per 2°

		public HeadListView(Context context, IAttributeSet attrs, int defStyle)
			: base(context, attrs, defStyle)
		{
			init();
		}

		public HeadListView(Context context, IAttributeSet attrs)
			: base(context, attrs)
		{
			init();
		}

		public HeadListView(Context context)
			: base(context)
		{
			init();
		}

		public void init()
		{
			mSensorManager = (SensorManager)Application.Context.GetSystemService(Context.SensorService);
			mSensor = mSensorManager.GetDefaultSensor(SensorType.RotationVector);
		}

		public void activate()
		{
			if (mSensor == null)
				return;

			mStartX = INVALID_X;
			mSensorManager.RegisterListener(this, mSensor, SENSOR_RATE_uS);
		}

		public void deactivate()
		{
			mSensorManager.UnregisterListener(this);
			mStartX = INVALID_X;
		}

		public void OnAccuracyChanged(Sensor sensor, SensorStatus accuracy)
		{
			mLastAccuracy = accuracy;
		}

		public void OnSensorChanged(SensorEvent e)
		{
			float[] mat = new float[9],
			orientation = new float[3];

			if (mLastAccuracy == SensorStatus.Unreliable)
				return;

			SensorManager.GetRotationMatrixFromVector(mat, e.Values.ToArray());
			SensorManager.RemapCoordinateSystem(mat, Android.Hardware.Axis.X, Android.Hardware.Axis.Z, mat);
			SensorManager.GetOrientation(mat, orientation);

			float z = orientation[0], // see https://developers.google.com/glass/develop/gdk/location-sensors/index
				  x = orientation[1],
				  y = orientation[2];

			if (mStartX == INVALID_X)
				mStartX = x;

			int position = (int)((mStartX - x) * -1 / VELOCITY);

			SmoothScrollToPosition(position);

			if (position < 0)
				mStartX = x;
			else if (position > Count)
			{
				float mEndX = (Count * VELOCITY) + mStartX;
				mStartX += x - mEndX;
			}
		}
	}
}