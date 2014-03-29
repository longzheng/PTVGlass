using System;
using Android.Widget;
using Android.Animation;
using Java.Lang;
using Android.Views;
using Android.Content;
using Android.Util;
using Android.OS;
using Android.Views.Animations;
using Android.Graphics.Drawables;

namespace GlassProgressBar
{

	public class SliderView : FrameLayout
	{
		public override bool PostDelayed (IRunnable action, long delayMillis)
		{
			return base.PostDelayed (action, delayMillis);
		}

		private const long HIDE_SLIDER_TIMEOUT_MILLIS = 1000L;
		private const int MIN_SLIDER_WIDTH_PX = 40;
		private const long SHOW_HIDE_ANIMATION_DURATION_MILLIS = 300L;
		private const long SLIDER_BAR_RESIZE_ANIMATION_DURATION_MILLIS = 300L;
		private float animatedCount = 0.0F;
		private int count = 0;
		private ObjectAnimator countAnimator;
		Runnable hideSliderRunnable;

		ImageView indeterminateSlider;
		float index = 0.0f;
		float slideableScale = 1.0f;
		View slider;
		bool sliderShowing = true;

		public SliderView(Context paramContext) : this (paramContext, null)
		{
		}

		public SliderView(Context paramContext, IAttributeSet paramAttributeSet) : this (paramContext, paramAttributeSet, 0)
		{
		}

		public SliderView(Context paramContext, IAttributeSet paramAttributeSet, int paramInt ) : base (paramContext, paramAttributeSet, paramInt)
		{
			hideSliderRunnable = new Runnable(() => HideSlider(true));

			LayoutInflater.From(Context).Inflate(Resource.Layout.slider, this);
			slider = FindViewById (Resource.Id.slider_control);
			indeterminateSlider = FindViewById<ImageView> (Resource.Id.indeterminate_slider);
			HideSlider (false);
			HideIndeterminateSlider (false);
		}

		void AnimateCountTo(float paramFloat)
		{
			if ((this.countAnimator != null) && (this.countAnimator.IsRunning))
				this.countAnimator.Cancel ();
			var arrayOfFloat = new float[2];
			arrayOfFloat [0] = animatedCount;
			arrayOfFloat [1] = paramFloat;
			this.countAnimator = ObjectAnimator.OfFloat (this, "animatedCount", arrayOfFloat);
			this.countAnimator.SetDuration(300L);
			this.countAnimator.Start ();
		}

		int GetBaseSliderWidth() {
			return Java.Lang.Math.Max ((int)(Resources.DisplayMetrics.WidthPixels / animatedCount), 40);
		}

		void HideIndeterminateSlider (bool paramBoolean)
		{
			int i = Resources.GetDimensionPixelSize (Resource.Dimension.slider_bar_height);
			if (paramBoolean) {
				this.indeterminateSlider.Animate().TranslationY(i)
					.SetDuration(300L);
				return;
			}
			this.indeterminateSlider.TranslationY = i;
		}

		public void HideSlider (bool paramBoolean)
		{
			if (!sliderShowing)
				return;
			int i = Resources.GetDimensionPixelSize(Resource.Dimension.slider_bar_height);
			if (paramBoolean)
				slider.Animate().TranslationY(i).SetDuration(300L);
			sliderShowing = false;
			slider.TranslationY = i;
		}

		void HideSliderAfterTimeout() {
			RemoveCallbacks (hideSliderRunnable);
			PostDelayed (hideSliderRunnable, 1000L);
		}

		void ShowIndeterminateSlider(bool paramBoolean) {
			if (paramBoolean) {
				indeterminateSlider.Animate ().TranslationY (0f).SetDuration (300L);
				return;
			}
			indeterminateSlider.TranslationY = 0f;
		}

		void ShowSlider(bool paramBoolean)
		{
			RemoveCallbacks (hideSliderRunnable);
			if (sliderShowing)
				return;
			if (paramBoolean)
				slider.Animate ().TranslationY (0f).SetDuration (300L);
			sliderShowing = true;
			slider.TranslationY = 0f;
		}

		void UpdateSliderWidth() 
		{
			if (count < 2) {
				HideSlider (true);
				return;
			}
			FrameLayout.LayoutParams localLayoutParams = (FrameLayout.LayoutParams)slider.LayoutParameters;
			localLayoutParams.Width = ((int) (1.0f / slideableScale*GetBaseSliderWidth()));
			localLayoutParams.LeftMargin = 0;
			slider.LayoutParameters = localLayoutParams;
			ShowSlider (true);
			SetProportionalIndex (index);
		}

		public void DismissManualProgress() {
			HideSlider(true);
		}

		float GetAnimatedCount() {
			return this.animatedCount;
		}

		void SetAnimatedCount(float paramFloat) {
			this.animatedCount = paramFloat;
			UpdateSliderWidth();
		}

		void SetCount(int paramInt) {
			HideIndeterminateSlider (true);
			HideSlider (true);
			count = paramInt;
			index = Java.Lang.Math.Max (Java.Lang.Math.Min (index, paramInt - 1), 0f);
			AnimateCountTo (paramInt);
		}

		public void SetManualProgress(float paramFloat) {
			SetManualProgress(paramFloat, false);
		}


		public void SetManualProgress(float paramFloat, bool paramBoolean) {
			HideIndeterminateSlider(true);
			ShowSlider(false);
			int i = Resources.DisplayMetrics.WidthPixels;
			FrameLayout.LayoutParams localLayoutParams = (FrameLayout.LayoutParams) this.slider.LayoutParameters;
			localLayoutParams.Width = i;
			localLayoutParams.SetMargins(-i, 0, 0, 0);
			this.slider.LayoutParameters = localLayoutParams;
			if (paramBoolean) {
				this.slider.Animate().TranslationX(paramFloat * i);
				return;
			}
			this.slider.TranslationX = paramFloat * i;
		}

		void SetProportionalIndex (float paramFloat)
		{
			SetProportionalIndex(paramFloat, 0);
		}

		public void SetProportionalIndex(float paramFloat, int paramInt) {
			if (this.count < 2) {
				HideSlider(true);
				return;
			}
			this.index = paramFloat;
			float f1 = 1.0F / this.slideableScale;
			float f2 = (0.5F + this.index - f1 / 2.0F)
				* (Resources.DisplayMetrics.WidthPixels / this.count);
			if (paramInt != 0)
				this.slider.Animate().TranslationX(f2).SetDuration(paramInt)
					.SetInterpolator(new AccelerateDecelerateInterpolator());
			ShowSlider(true);
			HideSliderAfterTimeout();
			this.slider.TranslationX = f2;
		}

		public void SetScale(float paramFloat) {
			this.slideableScale = paramFloat;
			UpdateSliderWidth();
		}

		public void StartIndeterminate() {
			int i = Resources.DisplayMetrics.WidthPixels;
			FrameLayout.LayoutParams localLayoutParams = (FrameLayout.LayoutParams) this.slider.LayoutParameters;
			localLayoutParams.Width = i;
			localLayoutParams.SetMargins(0, 0, 0, 0);
			this.slider.LayoutParameters = localLayoutParams;
			HideSlider(true);
			ShowIndeterminateSlider(true);
			((AnimationDrawable) this.indeterminateSlider.Background).Start();
		}

		public void StartProgress(long paramLong) {
			StartProgress(paramLong, new AccelerateDecelerateInterpolator());
		}

		public void StartProgress(long paramLong, ITimeInterpolator paramTimeInterpolator) {
			HideIndeterminateSlider(true);
			ShowSlider(false);
			int i = Resources.DisplayMetrics.WidthPixels;
			FrameLayout.LayoutParams localLayoutParams = (FrameLayout.LayoutParams) this.slider.LayoutParameters;
			localLayoutParams.Width = i;
			localLayoutParams.SetMargins(-i, 0, 0, 0);
			this.slider.LayoutParameters = localLayoutParams;
			this.slider.Animate().TranslationX(i).SetDuration(paramLong)
				.SetInterpolator(paramTimeInterpolator);
		}

		public void StopIndeterminate() {
			ShowSlider(true);
			((AnimationDrawable) this.indeterminateSlider.Background).Stop();
			HideIndeterminateSlider(true);
		}
	}
}
	
		
