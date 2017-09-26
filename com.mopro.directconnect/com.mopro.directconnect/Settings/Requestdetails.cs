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
    [Activity(Label = "Requestdetails", ScreenOrientation = ScreenOrientation.Portrait)]
    public class Requestdetails : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Requestdetail);
            Window.SetSoftInputMode(SoftInput.AdjustPan | SoftInput.StateHidden);
            // Click Back button Events Occurs below
            ImageView Backbutton = FindViewById<ImageView>(Resource.Id.Backbutton);
            Backbutton.Click += (o, e) =>
            {
                Intent objIntent = new Intent(this, typeof(Requests));
                StartActivity(objIntent);
                OverridePendingTransition(Resource.Drawable.slide_from_left, Resource.Drawable.slide_to_right);
            };
            TextView headingtxt = FindViewById<TextView>(Resource.Id.headingtext);
            headingtxt.SetTypeface(AppFont.GetTitle(this), TypefaceStyle.Normal);

            TextView Biznotes = FindViewById<TextView>(Resource.Id.Biznotes);
            Biznotes.SetTypeface(AppFont.GetTitle(this), TypefaceStyle.Normal);

            TextView Bizstatus2 = FindViewById<TextView>(Resource.Id.Bizstatus2);
            Bizstatus2.SetTypeface(AppFont.GetText(this), TypefaceStyle.Normal);

            TextView Bizstatus = FindViewById<TextView>(Resource.Id.Bizstatus);
            Bizstatus.SetTypeface(AppFont.GetText(this), TypefaceStyle.Normal);

            TextView Bizname = FindViewById<TextView>(Resource.Id.Bizname);
            Bizname.SetTypeface(AppFont.GetTitle(this), TypefaceStyle.Normal);

            TextView Bizlocation = FindViewById<TextView>(Resource.Id.Bizlocation);
            Bizlocation.SetTypeface(AppFont.GetText(this), TypefaceStyle.Normal);

            TextView Bizhome = FindViewById<TextView>(Resource.Id.Bizhome);
            Bizhome.SetTypeface(AppFont.GetText(this), TypefaceStyle.Normal);

            TextView Bizwebsite = FindViewById<TextView>(Resource.Id.Bizwebsite);
            Bizwebsite.SetTypeface(AppFont.GetText(this), TypefaceStyle.Normal);

            TextView Bizwebsite2 = FindViewById<TextView>(Resource.Id.Bizwebsite2);
            Bizwebsite2.SetTypeface(AppFont.GetText(this), TypefaceStyle.Normal);

            TextView Bizreference = FindViewById<TextView>(Resource.Id.Bizreference);
            Bizreference.SetTypeface(AppFont.GetText(this), TypefaceStyle.Normal);

            ImageView Imageicon = FindViewById<ImageView>(Resource.Id.Imageicon);
            Imageicon.Click += (o, e) => PressEditButton();

            TextView Imagetext = FindViewById<TextView>(Resource.Id.Imagetext);
            Imagetext.SetTypeface(AppFont.GetText(this), TypefaceStyle.Normal);
            Imagetext.Click += (o, e) => Imageicon.PerformClick();

            RelativeLayout NoImagelayout = FindViewById<RelativeLayout>(Resource.Id.NoImagelayout);
            NoImagelayout.Click += (o, e) => Imageicon.PerformClick();

            TextView Biztext = FindViewById<TextView>(Resource.Id.Biztext);
            Biztext.SetTypeface(AppFont.GetText(this), TypefaceStyle.Normal);
                        
            TextView Biztextview = FindViewById<TextView>(Resource.Id.Biztextview);
            Biztextview.SetTypeface(AppFont.GetText(this), TypefaceStyle.Normal);
            Biztextview.Click += (o, e) => PressEditButton();

            TextView Bizrevision = FindViewById<TextView>(Resource.Id.Bizrevision);
            Bizrevision.SetTypeface(AppFont.GetText(this), TypefaceStyle.Normal);
            Bizrevision.Click += (o, e) => PressEditButton();

            TextView Bizrevisionview = FindViewById<TextView>(Resource.Id.Bizrevisionview);
            Bizrevisionview.SetTypeface(AppFont.GetText(this), TypefaceStyle.Normal);
            Bizrevisionview.Click += (o, e) => PressEditButton();

            TextView Biztabchange = FindViewById<TextView>(Resource.Id.Biztabchange);
            Biztabchange.SetTypeface(AppFont.GetText(this), TypefaceStyle.Normal);
            Biztabchange.Click += (o, e) => PressEditButton();

            Button Submitbutton = FindViewById<Button>(Resource.Id.Submitbutton);
            Submitbutton.SetTypeface(AppFont.GetButton(this), TypefaceStyle.Normal);
            Submitbutton.Click += (o, e) => Alertpopup();

            //ScrollView Scrollview = FindViewById<ScrollView>(Resource.Id.Scrollview);
            //Scrollview.SetOnTouchListener(this);
        }
        // When Click Forget Password button method calls to redirect
        private void PressEditButton()
        {
            StartActivity(new Intent(this, typeof(Requestedit)));
            OverridePendingTransition(Resource.Drawable.slide_in_bottom, Resource.Drawable.slide_out_bottom);
        }
        // Method used to hide keyboard when not in use
        public void hideSoftKeyboard()
        {
            if (CurrentFocus != null)
            {
                InputMethodManager inputMethodManager = (InputMethodManager)GetSystemService(Context.InputMethodService);
                inputMethodManager.HideSoftInputFromWindow(CurrentFocus.WindowToken, 0);
            }
        }
        // Shows Alert for Inavlid User Login
        void Alertpopup()
        {
            AlertDialog.Builder alertDialog = new AlertDialog.Builder(this);
            alertDialog.SetTitle("Confirm Cancellation");
            alertDialog.SetMessage("Do you want to abort & revert the request submitted?");
            alertDialog.SetPositiveButton("Confirm", (sender, e) =>
            {
                Intent objIntent = new Intent(this, typeof(Requests));
                StartActivity(objIntent);
                OverridePendingTransition(Resource.Drawable.slide_from_left, Resource.Drawable.slide_to_right);
            });
            alertDialog.SetNegativeButton("Cancel", (sender, e) =>
            {
                alertDialog.Dispose();
            });
            alertDialog.Show();
        }
        // Click Back button Events Occurs below
        public override void OnBackPressed()
        {
            base.OnBackPressed();
            Intent objIntent = new Intent(this, typeof(Requests));
            StartActivity(objIntent);
            OverridePendingTransition(Resource.Drawable.slide_from_left, Resource.Drawable.slide_to_right);
        }
    }
}