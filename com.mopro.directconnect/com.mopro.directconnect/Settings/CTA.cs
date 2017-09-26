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
    [Activity(Label = "CTA", ScreenOrientation = ScreenOrientation.Portrait)]
    public class CTA : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.CTA);
            Window.SetSoftInputMode(SoftInput.StateHidden);
            ImageView Backbutton = FindViewById<ImageView>(Resource.Id.Backbutton);
            Backbutton.Click += (o, e) =>
            {
                Finish();
                OverridePendingTransition(Resource.Drawable.slide_from_left, Resource.Drawable.slide_to_right);
            };
            WebView web_view = FindViewById<WebView>(Resource.Id.Webview);
            web_view.Settings.JavaScriptEnabled = true;
            web_view.SetInitialScale(1);
            web_view.Settings.LoadWithOverviewMode = true;
            web_view.Settings.UseWideViewPort = true;
            web_view.Settings.DefaultZoom = WebSettings.ZoomDensity.Far;
            web_view.LoadUrl("http://pulse.dev.cmlmediasoft.com/mobile/faq.html");            
            web_view.SetBackgroundColor(Color.White);
            ImageView likebutton = FindViewById<ImageView>(Resource.Id.likebutton);
            ImageView unlikebutton = FindViewById<ImageView>(Resource.Id.unlikebutton);
            TextView liketext = FindViewById<TextView>(Resource.Id.liketext);
            liketext.SetTypeface(AppFont.GetText(this), TypefaceStyle.Normal);
            likebutton.Click += (o, e) =>
            {
                liketext.Text = "Thank you for your feedback!";
                likebutton.Visibility = ViewStates.Gone;
                unlikebutton.Visibility = ViewStates.Gone;
            };
            unlikebutton.Click += (o, e) =>
            {
                liketext.Text = "Thank you for your feedback!";
                likebutton.Visibility = ViewStates.Gone;
                unlikebutton.Visibility = ViewStates.Gone;
            };
        }
        // Click Back button Events Occurs below
        public override void OnBackPressed()
        {
            base.OnBackPressed();
            Finish();
            OverridePendingTransition(Resource.Drawable.slide_from_left, Resource.Drawable.slide_to_right);
        }
    }
}