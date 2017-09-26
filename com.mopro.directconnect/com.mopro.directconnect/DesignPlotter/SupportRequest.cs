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
using Android.Content.PM;
using Android.Graphics;

namespace com.mopro.directconnect
{
    [Activity(Label = "SupportRequest", ScreenOrientation = ScreenOrientation.Portrait)]
    public class SupportRequest : Activity
    {
        EditText EditNotes;
        TextView Editimage;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.SupportRequest);
            Window.SetSoftInputMode(SoftInput.AdjustResize | SoftInput.StateHidden);
            // Click Back button Events Occurs below
            ImageView Backbutton = FindViewById<ImageView>(Resource.Id.Backbutton);
            Backbutton.Click += (o, e) => PressBackbutton();
            TextView headingtxt = FindViewById<TextView>(Resource.Id.headingtext);
            headingtxt.SetTypeface(AppFont.GetTitle(this), TypefaceStyle.Normal);
            TextView EnterNotes = FindViewById<TextView>(Resource.Id.EnterNotes);
            EnterNotes.SetTypeface(AppFont.GetText(this), TypefaceStyle.Normal);
            TextView EnterImage = FindViewById<TextView>(Resource.Id.EnterImage);
            EnterImage.SetTypeface(AppFont.GetText(this), TypefaceStyle.Normal);
            TextView EnterOthernotes = FindViewById<TextView>(Resource.Id.EnterOthernotes);
            EnterOthernotes.SetTypeface(AppFont.GetText(this), TypefaceStyle.Normal);
            Button ButtonSave = FindViewById<Button>(Resource.Id.ButtonSave);
            Button ButtonBrowse = FindViewById<Button>(Resource.Id.ButtonBrowse);
            ButtonSave.SetTypeface(AppFont.GetButton(this), TypefaceStyle.Normal);
            ButtonBrowse.SetTypeface(AppFont.GetText(this), TypefaceStyle.Normal);
            ButtonBrowse.Click += (sender, e) => {
                Intent objIntent = new Intent(this, typeof(Viewimages));
                objIntent.PutExtra("Screen", "Support");
                StartActivity(objIntent);
                OverridePendingTransition(Resource.Drawable.slide_from_right, Resource.Drawable.slide_to_left);
            };
            ScrollView Scrollview = FindViewById<ScrollView>(Resource.Id.Scrollview);
           
            Editimage = FindViewById<TextView>(Resource.Id.Editimage);
            EditNotes = FindViewById<EditText>(Resource.Id.EditNotes);
            Editimage.SetTypeface(AppFont.GetText(this), TypefaceStyle.Normal);
            EditNotes.SetTypeface(AppFont.GetText(this), TypefaceStyle.Normal);
            ButtonSave.Click += (o, e) => PressSaveButton();
            EditNotes.SetOnTouchListener(new EditTextTouch());
            // Handel Events when the Password Text on editing
            EditNotes.AfterTextChanged += delegate (object sender, Android.Text.AfterTextChangedEventArgs e)
            {
                EditNotes.SetBackgroundResource(Resource.Drawable.Edittextbg);
                EditNotes.SetHintTextColor(Color.White);
            };
            String Livestatus = Intent.GetStringExtra("image");
            if (!String.IsNullOrEmpty(Livestatus))
            {
                Editimage.Text = System.IO.Path.GetFileName(Livestatus);
            }
        }
        private void PressBackbutton()
        {
            Intent objIntent = new Intent(this, typeof(Voice));
            objIntent.PutExtra("search", true);
            objIntent.PutExtra("searchtext", Voice.Searchtextstr);
            OverridePendingTransition(Resource.Drawable.slide_from_left, Resource.Drawable.slide_to_right);
        }
        public bool OnTouch(View v, MotionEvent e)
        {
            hideSoftKeyboard();
            return false;
        }
        // When Click Save button method calls
        private void PressSaveButton()
        {
            if (String.IsNullOrEmpty(Editimage.Text))
            {
                // Validation for the EditNotes
                AppValidation.TextValidation(EditNotes, "");
                if (AppValidation.TextValidation(EditNotes, ""))
                {
                    Intent objIntent = new Intent(this, typeof(Home));
                    objIntent.PutExtra("request", true);
                    StartActivity(objIntent);
                    OverridePendingTransition(Resource.Drawable.slide_from_left, Resource.Drawable.slide_to_right);
                }
                else
                {
                    Alertpopup("Missing information");
                }
            }
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
        void Alertpopup(String Content)
        {
            AlertDialog.Builder alertDialog = new AlertDialog.Builder(this);
            alertDialog.SetTitle(Resources.GetString(Resource.String.error));
            alertDialog.SetMessage(Content);
            alertDialog.SetPositiveButton("OK", delegate
            {
                alertDialog.Dispose();
            });
            alertDialog.Show();
        }
        // Click Back button Events Occurs below
        public override void OnBackPressed()
        {
            base.OnBackPressed();
            PressBackbutton();
        }
    }
}