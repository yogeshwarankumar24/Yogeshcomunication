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
using Android.Views.Animations;
using Android.Content.PM;

namespace com.mopro.directconnect
{
    [Activity(Label = "Linkchanges", ScreenOrientation = ScreenOrientation.Portrait)]
    public class Linkchanges : Activity
    {
        EditText EditNotes, EditLink;
        Dialog Dialog_saveoption;
        List<String> objBussinessdata;
        RelativeLayout Pagelayout;
        TextView EditPagetxt;
        public static String notes { get; set; }
        public static String button { get; set; }
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Linkchanges);
            Window.SetSoftInputMode(SoftInput.AdjustResize | SoftInput.StateHidden);
            // Click Back button Events Occurs below
            ImageView Backbutton = FindViewById<ImageView>(Resource.Id.Backbutton);
            Backbutton.Click += (o, e) => TextRevision.PressedBackButton(this);

            TextView headingtxt = FindViewById<TextView>(Resource.Id.headingtext);
            headingtxt.SetTypeface(AppFont.GetTitle(this), TypefaceStyle.Normal);
            TextView EnterNotes = FindViewById<TextView>(Resource.Id.EnterNotes);
            EnterNotes.SetTypeface(AppFont.GetText(this), TypefaceStyle.Normal);
            //ButtonScreen.Visibility = ViewStates.Gone;
            Button ButtonSave = FindViewById<Button>(Resource.Id.ButtonSave);
            ScrollView Scrollview = FindViewById<ScrollView>(Resource.Id.Scrollview);
            Dialog_saveoption = new Dialog(this, Resource.Style.AppThemeTransp);
            Dialog_saveoption.SetCanceledOnTouchOutside(true);
            Dialog_saveoption.SetContentView(Resource.Layout.Buttonlayout);
            ImageView CloseButtonoption = Dialog_saveoption.FindViewById<ImageView>(Resource.Id.Backbutton2);
            TextView headingtext = Dialog_saveoption.FindViewById<TextView>(Resource.Id.headingtext);
            headingtext.SetTypeface(AppFont.GetTitle(this), TypefaceStyle.Normal);
            headingtext.Text = "BUTTON UPDATES";
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
            EditNotes = FindViewById<EditText>(Resource.Id.EditNotes);            
            TextView Enterlink = FindViewById<TextView>(Resource.Id.Enterlink);
            Enterlink.SetTypeface(AppFont.GetText(this), TypefaceStyle.Normal);
            TextView EnterOthernotes = FindViewById<TextView>(Resource.Id.EnterOthernotes);
            EnterOthernotes.SetTypeface(AppFont.GetText(this), TypefaceStyle.Normal);            
            EditNotes.SetTypeface(AppFont.GetText(this), TypefaceStyle.Normal);
            ButtonSave.Click += (o, e) => PressSaveButton();
            ButtonSave.SetTypeface(AppFont.GetButton(this), TypefaceStyle.Normal);
            RadioButton radio_internal = FindViewById<RadioButton>(Resource.Id.radio_internal);
            radio_internal.Tag = "1";
            RadioButton radio_external = FindViewById<RadioButton>(Resource.Id.radio_external);
            radio_external.Tag = "2";
            radio_internal.Click += RadioButtonClick;
            radio_external.Click += RadioButtonClick;
            radio_internal.SetTypeface(AppFont.GetText(this), TypefaceStyle.Normal);
            radio_external.SetTypeface(AppFont.GetText(this), TypefaceStyle.Normal);
            Pagelayout = FindViewById<RelativeLayout>(Resource.Id.Pagelayout);
            EditPagetxt = FindViewById<TextView>(Resource.Id.EditPagetxt);
            EditLink = FindViewById<EditText>(Resource.Id.EditLink);
            EditLink.SetTypeface(AppFont.GetText(this), TypefaceStyle.Normal);
            EditText EditOthernotes = FindViewById<EditText>(Resource.Id.EditOthernotes);
            EditOthernotes.SetTypeface(AppFont.GetText(this), TypefaceStyle.Normal);
            ButtonSave.SetTypeface(AppFont.GetButton(this), TypefaceStyle.Normal);
            EditPagetxt.Click += (sender, e) => PressHeaderButton();
            EditPagetxt.SetTypeface(AppFont.GetText(this), TypefaceStyle.Normal);
            EditNotes.SetTypeface(AppFont.GetText(this), TypefaceStyle.Normal);
            EditNotes.SetOnTouchListener(new EditTextTouch());
            EditOthernotes.SetOnTouchListener(new EditTextTouch());
            // Handel Events when the Password Text on editing
            EditNotes.AfterTextChanged += delegate (object sender, Android.Text.AfterTextChangedEventArgs e)
            {
                EditNotes.SetBackgroundResource(Resource.Drawable.Edittextbg);
                EditNotes.SetHintTextColor(Color.White);
            };
            radio_internal.Checked = true;
            EditLink.Visibility = ViewStates.Gone;
            Pagelayout.Visibility = ViewStates.Visible;

            bool Menustatus = Intent.GetBooleanExtra("menu", false);
            if (Menustatus)
            {
                EditPagetxt.Text = Intent.GetStringExtra("menuname");
                EditOthernotes.Text = notes;
                EditNotes.Text = button;
            }

            // Handel Events when the Password Text on editing
            EditOthernotes.AfterTextChanged += delegate (object sender, Android.Text.AfterTextChangedEventArgs e)
            {
                notes = EditOthernotes.Text;
            };
            // Handel Events when the Password Text on editing
            EditNotes.AfterTextChanged += delegate (object sender, Android.Text.AfterTextChangedEventArgs e)
            {
                button = EditNotes.Text;
            };
        }
        // Methods call to Show and hide the content screen and Page screen on Click
        private void PressHeaderButton()
        {
            Intent objIntent = new Intent(this, typeof(Websitemenu));
            objIntent.PutExtra("screen", "Link");
            if(!String.IsNullOrEmpty(EditPagetxt.Text))
            objIntent.PutExtra("menuname", EditPagetxt.Text.ToLower());
            StartActivity(objIntent);
            OverridePendingTransition(Resource.Drawable.slide_in_bottom, Resource.Drawable.slide_out_bottom);
        }
        private void RadioButtonClick(object sender, EventArgs e)
        {
            RadioButton rb = (RadioButton)sender;
            if(rb.Tag.ToString().Equals("1"))
            {
                EditLink.Visibility = ViewStates.Gone;
                Pagelayout.Visibility = ViewStates.Visible;
            } else
            {
                EditLink.Visibility = ViewStates.Visible;
                Pagelayout.Visibility = ViewStates.Gone;
            }         
           // Toast.MakeText(this, rb.Tag.ToString(), ToastLength.Short).Show();
        }
        public bool OnTouch(View v, MotionEvent e)
        {
            hideSoftKeyboard();
            return true;
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
                Alertpopup("Please fill in data before submitting the ticket.");
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
            TextRevision.PressedBackButton(this);
            base.OnBackPressed();
        }
    }
}