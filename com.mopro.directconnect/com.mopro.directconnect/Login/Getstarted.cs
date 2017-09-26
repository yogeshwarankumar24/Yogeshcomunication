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
using Android.Graphics;
using Android.Content.PM;

namespace com.mopro.directconnect
{
    [Activity(Label = "Direct Connect", ScreenOrientation = ScreenOrientation.Portrait)]
    public class Getstarted : Activity
    {
        Progressbar objProgressbar;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Getstarted);
            Window.SetSoftInputMode(SoftInput.StateHidden);
            objProgressbar = new Progressbar(this);
            Button ButtonGetstarted = FindViewById<Button>(Resource.Id.ButtonGetstarted);
            ButtonGetstarted.Click += (o, e) => PressStartedButton();
            ButtonGetstarted.SetTypeface(AppFont.GetText(this), TypefaceStyle.Normal);
            TextView welcomepulse = FindViewById<TextView>(Resource.Id.welcomepulse);
            welcomepulse.SetTypeface(AppFont.GetButton(this), TypefaceStyle.Normal);
            TextView welcomecontent = FindViewById<TextView>(Resource.Id.welcomecontent);
            welcomecontent.SetTypeface(AppFont.GetText(this), TypefaceStyle.Normal);
            TextView welcomecontent2 = FindViewById<TextView>(Resource.Id.welcomecontent2);
            welcomecontent2.SetTypeface(AppFont.GetText(this), TypefaceStyle.Normal);
        }
        // Click Get Started button Events and redirect to Screen
        private void PressStartedButton()
        {
            objProgressbar.Show();
            StartActivity(new Intent(this, typeof(Bizoption)));
            OverridePendingTransition(Resource.Drawable.fade_in, Resource.Drawable.fade_out);
        }
        protected override void OnResume()
        {
            base.OnResume();
            if (objProgressbar.IsShowing)
                objProgressbar.Dismiss();
        }
        // Click Back button Events Occurs below
        public override void OnBackPressed()
        {
            FinishAffinity();
            base.OnBackPressed();
        }
    }
}