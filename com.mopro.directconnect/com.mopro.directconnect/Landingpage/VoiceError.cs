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
using Android.Views.InputMethods;
using Android.Content.PM;

namespace com.mopro.directconnect
{
    [Activity(Theme = "@style/AppThemeTransp", Label = "DirectConnect", ScreenOrientation = ScreenOrientation.Portrait)]
    public class VoiceError : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.voiceerror);
            Window.SetSoftInputMode(SoftInput.StateHidden);
            Button ButtonClear = FindViewById<Button>(Resource.Id.ButtonClear);
            Button Buttontry = FindViewById<Button>(Resource.Id.Buttontry);
            Button Buttonchat = FindViewById<Button>(Resource.Id.Buttonchat);
            ButtonClear.SetTypeface(AppFont.GetText(this), TypefaceStyle.Normal);
            Buttontry.SetTypeface(AppFont.GetButton(this), TypefaceStyle.Normal);
            Buttonchat.SetTypeface(AppFont.GetButton(this), TypefaceStyle.Normal);
            TextView ortext = FindViewById<TextView>(Resource.Id.ortext);
            ortext.SetTypeface(AppFont.GetText(this), TypefaceStyle.Normal);
            ImageView Backbutton = FindViewById<ImageView>(Resource.Id.Backbutton);
            Backbutton.Click += (o, e) => PressCancelButton();
            ButtonClear.Click += (o, e) => PressCancelButton();
            Buttontry.Click += (o, e) => PressCancelButton();
            Buttonchat.Click += (o, e) => PressChatButton();
            TextView contenttext = FindViewById<TextView>(Resource.Id.contenttext);
            contenttext.SetTypeface(AppFont.GetText(this), TypefaceStyle.Normal);
        }
        // Click Chat button Events Occurs below and Redirect to the Screen
        private void PressChatButton()
        {
            StartActivity(new Intent(this, typeof(Chatting)));
            OverridePendingTransition(Resource.Drawable.slide_from_right, Resource.Drawable.slide_to_left);
        }
        // When Click Cancel button method calls to redirect
        private void PressCancelButton()
        {
            StartActivity(new Intent(this, typeof(Home)));
            OverridePendingTransition(Resource.Drawable.slide_from_left, Resource.Drawable.slide_to_right);
        }
        // Click Back button Events Occurs below
        public override void OnBackPressed()
        {
            base.OnBackPressed();
            PressCancelButton();
        }
    }
}