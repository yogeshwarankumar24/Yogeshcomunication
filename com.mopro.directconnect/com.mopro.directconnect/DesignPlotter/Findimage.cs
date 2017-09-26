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
    [Activity(Label = "Findimage", ScreenOrientation = ScreenOrientation.Portrait)]
    public class Findimage : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Findimage);
            Window.SetSoftInputMode(SoftInput.AdjustResize | SoftInput.StateHidden);
            // Click Back button Events Occurs below
            ImageView Backbutton = FindViewById<ImageView>(Resource.Id.Backbutton);
            Backbutton.Click += (o, e) => {
                StartActivity(new Intent(this, typeof(Imageupdates)));
                OverridePendingTransition(Resource.Drawable.slide_from_left, Resource.Drawable.slide_to_right);
            };
            TextView headingtxt = FindViewById<TextView>(Resource.Id.headingtext);
            headingtxt.SetTypeface(AppFont.GetButton(this), TypefaceStyle.Normal);
            TextView Facebooktext = FindViewById<TextView>(Resource.Id.Facebooktext);
            Facebooktext.SetTypeface(AppFont.GetText(this), TypefaceStyle.Normal);
            ImageView Facebookimage = FindViewById<ImageView>(Resource.Id.Facebookimage);
            Facebooktext.Click += new EventHandler(this.PressFacebookButton);
            Facebookimage.Click += new EventHandler(this.PressFacebookButton);

            TextView Twittertext = FindViewById<TextView>(Resource.Id.Twittertext);
            Twittertext.SetTypeface(AppFont.GetText(this), TypefaceStyle.Normal);
            ImageView Twitterimage = FindViewById<ImageView>(Resource.Id.Twitterimage);
            Twittertext.Click += new EventHandler(this.PressTwitterButton);
            Twitterimage.Click += new EventHandler(this.PressTwitterButton);

            TextView Pinteresttext = FindViewById<TextView>(Resource.Id.Pinteresttext);
            Pinteresttext.SetTypeface(AppFont.GetText(this), TypefaceStyle.Normal);
            ImageView Pinterestimage = FindViewById<ImageView>(Resource.Id.Pinterestimage);
            Pinteresttext.Click += new EventHandler(this.PressPinterestButton);
            Pinterestimage.Click += new EventHandler(this.PressPinterestButton);

            TextView Flickrtext = FindViewById<TextView>(Resource.Id.Flickrtext);
            Flickrtext.SetTypeface(AppFont.GetText(this), TypefaceStyle.Normal);
            ImageView Flickrimage = FindViewById<ImageView>(Resource.Id.Flickrimage);
            Flickrtext.Click += new EventHandler(this.PressFlickrButton);
            Flickrimage.Click += new EventHandler(this.PressFlickrButton);

            TextView Googletext = FindViewById<TextView>(Resource.Id.Googletext);
            Googletext.SetTypeface(AppFont.GetText(this), TypefaceStyle.Normal);
            ImageView Googleimage = FindViewById<ImageView>(Resource.Id.Googleimage);
            Googletext.Click += new EventHandler(this.PressGoogleButton);
            Googleimage.Click += new EventHandler(this.PressGoogleButton);

            TextView Phonetext = FindViewById<TextView>(Resource.Id.Phonetext);
            Phonetext.SetTypeface(AppFont.GetText(this), TypefaceStyle.Normal);
            ImageView Phoneimage = FindViewById<ImageView>(Resource.Id.Phoneimage);
            Phonetext.Click += new EventHandler(this.PressPhoneButton);
            Phoneimage.Click += new EventHandler(this.PressPhoneButton);
        }
        void PressFacebookButton(Object sender, EventArgs e)
        {
           // StartActivity(new Intent(this, typeof(Viewimages)));
        }
        void PressTwitterButton(Object sender, EventArgs e)
        {
           // StartActivity(new Intent(this, typeof(Viewimages)));
        }
        void PressPinterestButton(Object sender, EventArgs e)
        {
           // StartActivity(new Intent(this, typeof(Viewimages)));
        }
        void PressFlickrButton(Object sender, EventArgs e)
        {
           // StartActivity(new Intent(this, typeof(Viewimages)));
        }
        void PressGoogleButton(Object sender, EventArgs e)
        {
           // StartActivity(new Intent(this, typeof(Viewimages)));
        }
        void PressPhoneButton(Object sender, EventArgs e)
        {
            StartActivity(new Intent(this, typeof(Viewimages)));
            OverridePendingTransition(Resource.Drawable.slide_from_right, Resource.Drawable.slide_to_left);
        }
        // Click Back button Events Occurs below
        public override void OnBackPressed()
        {
            base.OnBackPressed();
            StartActivity(new Intent(this, typeof(Imageupdates)));
            OverridePendingTransition(Resource.Drawable.slide_from_left, Resource.Drawable.slide_to_right);
        }
    }
}