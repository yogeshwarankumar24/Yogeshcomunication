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
using Android.Views.InputMethods;
using Android.Graphics;
using Android.Content.PM;

namespace com.mopro.directconnect
{
    [Activity(Label = "CallReview", ScreenOrientation = ScreenOrientation.Portrait)]
    public class CallReview : Activity,View.IOnTouchListener
    {
        RatingBar ratingBar;
        int Ratingvalue = 0;
        ImageView ratingicon1, ratingicon2, ratingicon3, ratingicon4, ratingicon5;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.CallReview);
            Window.SetSoftInputMode(SoftInput.AdjustResize | SoftInput.StateHidden);
            // Click Back button Events Occurs below
            ImageView Backbutton = FindViewById<ImageView>(Resource.Id.Backbutton);
            Backbutton.Click += (o, e) => {
                Intent objIntent = new Intent(this, typeof(Home));
                StartActivity(objIntent);
                OverridePendingTransition(Resource.Drawable.slide_from_left, Resource.Drawable.slide_to_right);
            };
            TextView calltext = FindViewById<TextView>(Resource.Id.calltext);
            calltext.SetTypeface(AppFont.GetButton(this), TypefaceStyle.Normal);
            TextView ratingheader = FindViewById<TextView>(Resource.Id.ratingheader);
            ratingheader.SetTypeface(AppFont.GetButton(this), TypefaceStyle.Normal);
            TextView EnterOthertext = FindViewById<TextView>(Resource.Id.EnterOthertext);
            EnterOthertext.SetTypeface(AppFont.GetText(this), TypefaceStyle.Normal);
            EditText EditOthernotes = FindViewById<EditText>(Resource.Id.EditOthernotes);
            EditOthernotes.SetTypeface(AppFont.GetText(this), TypefaceStyle.Normal);
            Button ButtonSave = FindViewById<Button>(Resource.Id.ButtonSave);
            ButtonSave.SetTypeface(AppFont.GetButton(this), TypefaceStyle.Normal);
            ButtonSave.Click += (o, e) => PressSaveButton();
            EditOthernotes.SetOnTouchListener(new EditTextTouch());
            ScrollView Scrollview = FindViewById<ScrollView>(Resource.Id.Scrollview);
            ratingBar = FindViewById<RatingBar>(Resource.Id.ratingBar);
            ratingBar.RatingBarChange += (o, e) => {
                if (ratingBar.Rating < 1)
                    ratingBar.Progress = 1;
            };
             ratingBar.Visibility = ViewStates.Gone;
            ratingicon1 = FindViewById<ImageView>(Resource.Id.ratingicon1);
            ratingicon2 = FindViewById<ImageView>(Resource.Id.ratingicon2);
            ratingicon3 = FindViewById<ImageView>(Resource.Id.ratingicon3);
            ratingicon4 = FindViewById<ImageView>(Resource.Id.ratingicon4);
            ratingicon5 = FindViewById<ImageView>(Resource.Id.ratingicon5);
            ratingicon1.Tag = "1";
            ratingicon1.SetOnTouchListener(this);
            ratingicon2.Tag = "2";
            ratingicon2.SetOnTouchListener(this);
            ratingicon3.Tag = "3";
            ratingicon3.SetOnTouchListener(this);
            ratingicon4.Tag = "4";
            ratingicon4.SetOnTouchListener(this);
            ratingicon5.Tag = "5";
            ratingicon5.SetOnTouchListener(this);
        }
        public bool OnTouch(View v, MotionEvent eve)
        {
            switch (eve.Action & MotionEventActions.Mask)
            {
                case MotionEventActions.Down:
                    StarRating(Int32.Parse(v.Tag.ToString()));
                    break;
                case MotionEventActions.Up:
                    StarRating(Int32.Parse(v.Tag.ToString()));
                    break;
                case MotionEventActions.Move:
                    StarRating(Int32.Parse(v.Tag.ToString()));
                    break;
            }
            return false;
        }
        // When Click Save button method calls
        private void PressSaveButton()
        {
            if(Ratingvalue == 0)
            {
                Alertpopup("Error!", "Please rate your experience");
            } else 
                Alertpopup("Thank You!", "We appreciate the feedback.");
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
        void Alertpopup(String title, String Content)
        {
            AlertDialog.Builder alertDialog = new AlertDialog.Builder(this);
            alertDialog.SetTitle(title);
            alertDialog.SetMessage(Content);
            alertDialog.SetPositiveButton("OK", delegate
            {
                alertDialog.Dispose();
                if (!title.ToLower().Contains("error"))
                {
                    Intent objIntent = new Intent(this, typeof(Home));
                    StartActivity(objIntent);
                    OverridePendingTransition(Resource.Drawable.slide_from_left, Resource.Drawable.slide_to_right);
                }
            });
            alertDialog.Show();
        }
        // Click Back button Events Occurs below
        public override void OnBackPressed()
        {
            base.OnBackPressed();
            Intent objIntent = new Intent(this, typeof(Home));
            StartActivity(objIntent);
            OverridePendingTransition(Resource.Drawable.slide_from_left, Resource.Drawable.slide_to_right);
        }
        void StarRating(int rating)
        {
            Ratingvalue = rating;
            switch (rating)
            {
                case 1:
                    ratingicon1.SetImageResource(Resource.Drawable.star_active);
                    ratingicon2.SetImageResource(Resource.Drawable.star);
                    ratingicon3.SetImageResource(Resource.Drawable.star);
                    ratingicon4.SetImageResource(Resource.Drawable.star);
                    ratingicon5.SetImageResource(Resource.Drawable.star);
                    break;
                case 2:
                    ratingicon1.SetImageResource(Resource.Drawable.star_active);
                    ratingicon2.SetImageResource(Resource.Drawable.star_active);
                    ratingicon3.SetImageResource(Resource.Drawable.star);
                    ratingicon4.SetImageResource(Resource.Drawable.star);
                    ratingicon5.SetImageResource(Resource.Drawable.star);
                    break;
                case 3:
                    ratingicon1.SetImageResource(Resource.Drawable.star_active);
                    ratingicon2.SetImageResource(Resource.Drawable.star_active);
                    ratingicon3.SetImageResource(Resource.Drawable.star_active);
                    ratingicon4.SetImageResource(Resource.Drawable.star);
                    ratingicon5.SetImageResource(Resource.Drawable.star);
                    break;
                case 4:
                    ratingicon1.SetImageResource(Resource.Drawable.star_active);
                    ratingicon2.SetImageResource(Resource.Drawable.star_active);
                    ratingicon3.SetImageResource(Resource.Drawable.star_active);
                    ratingicon4.SetImageResource(Resource.Drawable.star_active);
                    ratingicon5.SetImageResource(Resource.Drawable.star);
                    break;
                case 5:
                    ratingicon1.SetImageResource(Resource.Drawable.star_active);
                    ratingicon2.SetImageResource(Resource.Drawable.star_active);
                    ratingicon3.SetImageResource(Resource.Drawable.star_active);
                    ratingicon4.SetImageResource(Resource.Drawable.star_active);
                    ratingicon5.SetImageResource(Resource.Drawable.star_active);
                    break;
            }
        }
    }
}