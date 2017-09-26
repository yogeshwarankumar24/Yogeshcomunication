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
    [Activity(Label = "Requestedit", ScreenOrientation = ScreenOrientation.Portrait)]
    public class Requestedit : Activity
    {
        EditText EditNotes;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Requestedit);
            Window.SetSoftInputMode(SoftInput.AdjustPan | SoftInput.StateHidden);
            // Click Back button Events Occurs below
            ImageView Backbutton = FindViewById<ImageView>(Resource.Id.Backbutton);
            Backbutton.Click += (o, e) =>
            {
                Intent objIntent = new Intent(this, typeof(Requestdetails));
                StartActivity(objIntent);
                OverridePendingTransition(Resource.Drawable.slide_from_left, Resource.Drawable.slide_to_right);
            };
            
            TextView Backtext = FindViewById<TextView>(Resource.Id.Backtext);
            Backtext.SetTypeface(AppFont.GetText(this), TypefaceStyle.Normal);
            Backtext.Click += delegate { Backbutton.PerformClick(); };
            TextView headingtxt = FindViewById<TextView>(Resource.Id.headingtext);
            headingtxt.SetTypeface(AppFont.GetTitle(this), TypefaceStyle.Normal);
            TextView Biznotes = FindViewById<TextView>(Resource.Id.Biznotes);
            Biznotes.SetTypeface(AppFont.GetTitle(this), TypefaceStyle.Normal);
            TextView Bizreference = FindViewById<TextView>(Resource.Id.Bizreference);
            Bizreference.SetTypeface(AppFont.GetText(this), TypefaceStyle.Normal);
            RelativeLayout Imagelayout = FindViewById<RelativeLayout>(Resource.Id.NoImagelayout);
            TextView Imagetext = FindViewById<TextView>(Resource.Id.Imagetext);
            ImageView Imageicon = FindViewById<ImageView>(Resource.Id.Imageicon);
            Imagetext.SetTypeface(AppFont.GetText(this), TypefaceStyle.Normal);
            Imageicon.Click += (o, e) => PressImageButton();
            Imagetext.Click += (o, e) => Imageicon.PerformClick();
            Imagelayout.Click += (o, e) => Imageicon.PerformClick();
            TextView Entertext = FindViewById<TextView>(Resource.Id.Entertext);
            Entertext.SetTypeface(AppFont.GetText(this), TypefaceStyle.Normal);
            TextView EnterOthertext = FindViewById<TextView>(Resource.Id.EnterOthertext);
            EnterOthertext.SetTypeface(AppFont.GetText(this), TypefaceStyle.Normal);
            EditText EditOthernotes = FindViewById<EditText>(Resource.Id.EditOthernotes);
            EditOthernotes.SetTypeface(AppFont.GetText(this), TypefaceStyle.Normal);
            ScrollView Scrollview = FindViewById<ScrollView>(Resource.Id.Scrollview);
            EditNotes = FindViewById<EditText>(Resource.Id.EditNotes);
            EditNotes.SetTypeface(AppFont.GetText(this), TypefaceStyle.Normal);
            EditNotes.SetOnTouchListener(new EditTextTouch());
            EditOthernotes.SetOnTouchListener(new EditTextTouch());
            // Handel Events when the Password Text on editing
            EditNotes.AfterTextChanged += delegate (object sender, Android.Text.AfterTextChangedEventArgs e)
            {
                EditNotes.SetBackgroundResource(Resource.Drawable.Edittextbg);
                EditNotes.SetHintTextColor(Color.White);
            };

            Button Submitbutton = FindViewById<Button>(Resource.Id.Submitbutton);
            Submitbutton.SetTypeface(AppFont.GetButton(this), TypefaceStyle.Normal);
            Submitbutton.Click += (o, e) => Alertpopup();
        }
        public bool OnTouch(View v, MotionEvent e)
        {
            hideSoftKeyboard();
            return false;
        }
        // Method used to hide keyboard when not in use
        public void PressImageButton()
        {
            StartActivity(new Intent(this, typeof(Referscreen)));
            OverridePendingTransition(Resource.Drawable.slide_from_right, Resource.Drawable.slide_to_left);
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
            alertDialog.SetTitle("Success");
            alertDialog.SetMessage("The updated request has been submitted to the queue.");
            alertDialog.SetPositiveButton("OK", (sender, e) =>
            {
                alertDialog.Dispose();
                Intent objIntent = new Intent(this, typeof(Requestdetails));
                StartActivity(objIntent);
                OverridePendingTransition(Resource.Drawable.slide_from_left, Resource.Drawable.slide_to_right);
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