using System;
using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.Widget;
using Android.Views.Animations;
using Android.Graphics.Drawables;

namespace com.mopro.directconnect
{
    public class Progressbar
    {
        Dialog Screen;
        public bool IsShowing;
        ImageView Progressimage;
        int Screenwidth;
        Activity Context;
        public Progressbar(Activity Context)
        {
            this.Context = Context;
            Screen = new Dialog(Context, Resource.Style.AppThemeTransp);
            Screen.SetCanceledOnTouchOutside(false);
            Screen.SetContentView(Resource.Layout.Progressbar);
            Screenwidth = Context.Resources.DisplayMetrics.WidthPixels;
            Progressimage = Screen.FindViewById<ImageView>(Resource.Id.Progressimage);
            AnimationDrawable Loadingimage = (Android.Graphics.Drawables.AnimationDrawable)Context.Resources.GetDrawable(Resource.Drawable.Loadingimage);
            Progressimage.SetImageDrawable((Android.Graphics.Drawables.Drawable)Loadingimage);
            Loadingimage.Start();
        }
        public void Show()
        {
            RelativeLayout.LayoutParams layoutParams = new RelativeLayout.LayoutParams(AppValidation.ToPixels(Context, 50), AppValidation.ToPixels(Context, 50));
            layoutParams.AddRule(LayoutRules.CenterInParent);
            Progressimage.LayoutParameters = layoutParams;
            Screen.Show();
            IsShowing = Screen.IsShowing;
        }
        public void Show(float positiony)
        {
            RelativeLayout.LayoutParams layoutParams = new RelativeLayout.LayoutParams(AppValidation.ToPixels(Context, 50), AppValidation.ToPixels(Context, 50));
            layoutParams.TopMargin = ((int)Math.Ceiling(positiony)) - AppValidation.ToPixels(Context, 30);
            layoutParams.LeftMargin = (Screenwidth / 2) - AppValidation.ToPixels(Context, 25);
            Progressimage.LayoutParameters = layoutParams;
            Screen.Show();
            IsShowing = Screen.IsShowing;
        }
        public void Dismiss()
        {
            Screen.Dismiss();
            IsShowing = Screen.IsShowing;
        }
    }
}