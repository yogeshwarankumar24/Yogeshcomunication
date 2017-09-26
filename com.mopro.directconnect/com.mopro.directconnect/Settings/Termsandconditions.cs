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
    [Activity(Label = "Termsandconditions", ScreenOrientation = ScreenOrientation.Portrait)]
    public class Termsandconditions : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Termsandconditions);
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
            web_view.LoadUrl("http://pulse.dev.cmlmediasoft.com/mobile/terms.html");
            web_view.SetWebViewClient(new Webviewclient(this));
            web_view.SetBackgroundColor(Resources.GetColor(Resource.Color.Transparent));
        }
        public class Webviewclient : WebViewClient
        {
            public Activity mActivity;
            public Webviewclient(Activity mActivity)
            {
                this.mActivity = mActivity;
            }
            public override void OnPageStarted(WebView view, string url, Bitmap favicon)
            {
                base.OnPageStarted(view, url, favicon);
            }
            public override bool ShouldOverrideUrlLoading(WebView view, string url)
            {

                if (url.ToLower().Contains("privacy.html"))
                {
                    mActivity.StartActivity(new Intent(mActivity, typeof(Privacypolicy)));
                    mActivity.OverridePendingTransition(Resource.Drawable.slide_from_right, Resource.Drawable.slide_to_left);
                }
                else if (url.ToLower().Contains("mailto:legal@mopro.com"))
                {
                    Intent intent = new Intent(Intent.ActionView, Android.Net.Uri.Parse("mailto:legal@mopro.com?subject=Terms&Conditions"));
                    mActivity.StartActivity(intent);
                }
                else
                    view.LoadUrl(url);
                return true;
            }
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