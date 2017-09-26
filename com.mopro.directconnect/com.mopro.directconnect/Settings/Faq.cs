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
using Android.Webkit;
using Android.Graphics;
using Android.Content.PM;

namespace com.mopro.directconnect
{
    [Activity(Label = "Faq", ScreenOrientation = ScreenOrientation.Portrait)]
    public class Faq : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Faq);
            Window.SetSoftInputMode(SoftInput.StateHidden);
            ImageView Backbutton = FindViewById<ImageView>(Resource.Id.Backbutton);
            Backbutton.Click += (o, e) =>
            {
                StartActivity(new Intent(this, typeof(Home)));
                OverridePendingTransition(Resource.Drawable.slide_from_left, Resource.Drawable.slide_to_right);
            };
            WebView web_view = FindViewById<WebView>(Resource.Id.Webview);
            web_view.Settings.JavaScriptEnabled = true;
            web_view.SetInitialScale(1);
            web_view.Settings.LoadWithOverviewMode = true;
            web_view.Settings.UseWideViewPort = true;
            web_view.Settings.DefaultZoom = WebSettings.ZoomDensity.Far;
            web_view.LoadUrl("http://pulse.dev.cmlmediasoft.com/mobile/faq.html");
            web_view.SetBackgroundColor(Resources.GetColor(Resource.Color.Transparent));
        }
        // Click Back button Events Occurs below
        public override void OnBackPressed()
        {
            base.OnBackPressed();
            StartActivity(new Intent(this, typeof(Home)));
            OverridePendingTransition(Resource.Drawable.slide_from_left, Resource.Drawable.slide_to_right);
        }
    }
}