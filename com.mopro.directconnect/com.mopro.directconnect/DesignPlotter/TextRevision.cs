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
    [Activity(Label = "TextRevision", ScreenOrientation = ScreenOrientation.Portrait)]
    public class TextRevision : Activity
    {
        EditText EditNotes;
        Dialog Dialog_saveoption;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.TextRevision);
            Window.SetSoftInputMode(SoftInput.AdjustResize | SoftInput.StateHidden);
            // Click Back button Events Occurs below
            ImageView Backbutton = FindViewById<ImageView>(Resource.Id.Backbutton);
            Backbutton.Click += (o, e) => PressedBackButton(this);
            TextView headingtxt = FindViewById<TextView>(Resource.Id.headingtext);
            headingtxt.SetTypeface(AppFont.GetTitle(this), TypefaceStyle.Normal);
            TextView Entertext = FindViewById<TextView>(Resource.Id.Entertext);
            Entertext.SetTypeface(AppFont.GetText(this), TypefaceStyle.Normal);
            TextView EnterOthertext = FindViewById<TextView>(Resource.Id.EnterOthertext);
            EnterOthertext.SetTypeface(AppFont.GetText(this), TypefaceStyle.Normal);
            EditText EditOthernotes = FindViewById<EditText>(Resource.Id.EditOthernotes);
            EditOthernotes.SetTypeface(AppFont.GetText(this), TypefaceStyle.Normal);

            //ButtonScreen.Visibility = ViewStates.Gone;
            Button ButtonSave = FindViewById<Button>(Resource.Id.ButtonSave);
            ScrollView Scrollview = FindViewById<ScrollView>(Resource.Id.Scrollview);
            Dialog_saveoption = new Dialog(this, Resource.Style.AppThemeTransp);
            Dialog_saveoption.SetCanceledOnTouchOutside(true);
            Dialog_saveoption.SetContentView(Resource.Layout.Buttonlayout);
            ImageView CloseButtonoption = Dialog_saveoption.FindViewById<ImageView>(Resource.Id.Backbutton2);
            TextView headingtext = Dialog_saveoption.FindViewById<TextView>(Resource.Id.headingtext);
            headingtext.Text = "TEXT UPDATES";
            headingtext.SetTypeface(AppFont.GetTitle(this), TypefaceStyle.Normal);
            CloseButtonoption.Click += (sender, e) =>
            {
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
            ButtonAnother.Click += (sender, e) =>
            {
                Website.ObjButtonlist.LastOrDefault().status = true;
                Intent objIntent = new Intent(this, typeof(Website));
                objIntent.PutExtra("status", true);
                StartActivity(objIntent);
                OverridePendingTransition(Resource.Drawable.slide_from_left, Resource.Drawable.slide_to_right);
            };
            ButtonSubmitteam.Click += (sender, e) =>
            {
                Intent objIntent = new Intent(this, typeof(Home));
                objIntent.PutExtra("request", true);
                StartActivity(objIntent);
                OverridePendingTransition(Resource.Drawable.slide_from_left, Resource.Drawable.slide_to_right);
            };
            EditNotes = FindViewById<EditText>(Resource.Id.EditNotes);
            EditNotes.SetTypeface(AppFont.GetText(this), TypefaceStyle.Normal);
            ButtonSave.Click += (o, e) => PressSaveButton();
            ButtonSave.SetTypeface(AppFont.GetButton(this), TypefaceStyle.Normal);
            EditNotes.SetOnTouchListener(new EditTextTouch());
            EditOthernotes.SetOnTouchListener(new EditTextTouch());
            // Handel Events when the Password Text on editing
            EditNotes.AfterTextChanged += delegate (object sender, Android.Text.AfterTextChangedEventArgs e)
            {
                EditNotes.SetBackgroundResource(Resource.Drawable.Edittextbg);
                EditNotes.SetHintTextColor(Color.White);
            };
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
            PressedBackButton(this);
        }
        public static void PressedBackButton(Activity objactivity)
        {
            Website.ObjButtonlist.LastOrDefault().status = false;
            Intent objIntent = new Intent(objactivity, typeof(Website));
            objIntent.PutExtra("status", true);
            objactivity.StartActivity(objIntent);
            objactivity.OverridePendingTransition(Resource.Drawable.slide_from_left, Resource.Drawable.slide_to_right);
        }
    }
}