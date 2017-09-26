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
using System.IO;
using Android.Content.PM;

namespace com.mopro.directconnect
{
    [Activity(Label = "Designchanges", ScreenOrientation = ScreenOrientation.Portrait)]
    public class Designchanges : Activity
    {
        EditText EditNotes;
        TextView Editimage;
        Dialog Dialog_saveoption;
        public static String notes { get; set; }
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Designchanges);
            Window.SetSoftInputMode(SoftInput.AdjustResize | SoftInput.StateHidden);
            // Click Back button Events Occurs below
            ImageView Backbutton = FindViewById<ImageView>(Resource.Id.Backbutton);
            Backbutton.Click += (o, e) => TextRevision.PressedBackButton(this);
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
                objIntent.PutExtra("Screen", "Design");
                StartActivity(objIntent);
                OverridePendingTransition(Resource.Drawable.slide_from_right, Resource.Drawable.slide_to_left);
            };
            ScrollView Scrollview = FindViewById<ScrollView>(Resource.Id.Scrollview);
            Dialog_saveoption = new Dialog(this, Resource.Style.AppThemeTransp);
            Dialog_saveoption.SetCanceledOnTouchOutside(true);
            Dialog_saveoption.SetContentView(Resource.Layout.Buttonlayout);
            ImageView CloseButtonoption = Dialog_saveoption.FindViewById<ImageView>(Resource.Id.Backbutton2);
            TextView headingtext = Dialog_saveoption.FindViewById<TextView>(Resource.Id.headingtext);
            headingtext.SetTypeface(AppFont.GetTitle(this), TypefaceStyle.Normal);
            headingtext.Text = "DESIGN REQUEST";
            CloseButtonoption.Click += (sender, e) => {
                Website.ObjButtonlist.LastOrDefault().status = true;
                Intent objIntent = new Intent(this, typeof(Website));
                objIntent.PutExtra("status", true);
                StartActivity(objIntent);
                OverridePendingTransition(Resource.Drawable.slide_from_left, Resource.Drawable.slide_to_right);
            };
            TextView ortext = Dialog_saveoption.FindViewById<TextView>(Resource.Id.ortext);
            ortext.SetTypeface(AppFont.GetButton(this), TypefaceStyle.Normal);
            TextView savedtext = Dialog_saveoption.FindViewById<TextView>(Resource.Id.savedtext);
            savedtext.SetTypeface(AppFont.GetButton(this), TypefaceStyle.Normal);
            Button ButtonAnother = Dialog_saveoption.FindViewById<Button>(Resource.Id.ButtonAnother);
            Button ButtonSubmitteam = Dialog_saveoption.FindViewById<Button>(Resource.Id.ButtonSubmitteam);
            ButtonAnother.SetTypeface(AppFont.GetButton(this), TypefaceStyle.Normal);
            ButtonSubmitteam.SetTypeface(AppFont.GetButton(this), TypefaceStyle.Normal);
            ButtonAnother.Click += (sender, e) => {
                Website.ObjButtonlist.LastOrDefault().status = true;
                Intent objIntent = new Intent(this, typeof(Website));
                objIntent.PutExtra("status", true);
                StartActivity(objIntent);
                OverridePendingTransition(Resource.Drawable.slide_from_left, Resource.Drawable.slide_to_right);
            };
            ButtonSubmitteam.Click += (sender, e) => {
                Intent objIntent = new Intent(this, typeof(Home));
                objIntent.PutExtra("request", true);
                StartActivity(objIntent);
                OverridePendingTransition(Resource.Drawable.slide_from_left, Resource.Drawable.slide_to_right);
            };
            EditText EditOthernotes = FindViewById<EditText>(Resource.Id.EditOthernotes);
            EditOthernotes.SetTypeface(AppFont.GetText(this), TypefaceStyle.Normal);
            Editimage = FindViewById<TextView>(Resource.Id.Editimage);
            EditNotes = FindViewById<EditText>(Resource.Id.EditNotes);
            Editimage.SetTypeface(AppFont.GetText(this), TypefaceStyle.Normal);
            EditNotes.SetTypeface(AppFont.GetText(this), TypefaceStyle.Normal);
            ButtonSave.Click += (o, e) => PressSaveButton();
            EditNotes.SetOnTouchListener(new EditTextTouch());
            EditOthernotes.SetOnTouchListener(new EditTextTouch());
            // Handel Events when the Password Text on editing
            EditNotes.AfterTextChanged += delegate (object sender, Android.Text.AfterTextChangedEventArgs e)
            {
                EditNotes.SetBackgroundResource(Resource.Drawable.Edittextbg);
                EditNotes.SetHintTextColor(Color.White);
            };
            // Handel Events when the Password Text on editing
            EditOthernotes.AfterTextChanged += delegate (object sender, Android.Text.AfterTextChangedEventArgs e)
            {
                notes = EditOthernotes.Text;
            };
            String Livestatus = Intent.GetStringExtra("image");
            if (!String.IsNullOrEmpty(Livestatus))
            {
                Editimage.Text = System.IO.Path.GetFileName(Livestatus);
                EditOthernotes.Text = notes;
            }
        }
        public bool OnTouch(View v, MotionEvent e)
        {
            hideSoftKeyboard();
            return false;
        }
        // When Click Save button method calls
        private void PressSaveButton()
        {
            // Validation for the EditNotes
            AppValidation.TextValidation(EditNotes, "");
            if (AppValidation.TextValidation(EditNotes, ""))
            {
                Dialog_saveoption.Show();
            }
            else
            {
                Alertpopup("Missing information");
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
            TextRevision.PressedBackButton(this);
        }
    }
}