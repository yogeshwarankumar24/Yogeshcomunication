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
using Android.Media;
using Android.Graphics;
using Android.Content.PM;

namespace com.mopro.directconnect
{
    [Activity(Label = "Videoupdates", ScreenOrientation = ScreenOrientation.Portrait)]
    public class Videoupdates : Activity
    {
        Dialog Dialog_saveoption;
        RelativeLayout videolayout, Selectimagelayout;
        TextView videoheader;
        Progressbar objProgressbar;
        public static String notes { get; set; }
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Videoupdates);
            Window.SetSoftInputMode(SoftInput.AdjustResize | SoftInput.StateHidden);
            objProgressbar = new Progressbar(this);
            // Click Back button Events Occurs below
            ImageView Backbutton = FindViewById<ImageView>(Resource.Id.Backbutton);
            Backbutton.Click += (o, e) => TextRevision.PressedBackButton(this);
            TextView headingtxt = FindViewById<TextView>(Resource.Id.headingtext);
            headingtxt.SetTypeface(AppFont.GetTitle(this), TypefaceStyle.Normal);
            Button ButtonSave = FindViewById<Button>(Resource.Id.ButtonSave);
            videolayout = FindViewById<RelativeLayout>(Resource.Id.videolayout);
            ImageView videoicon = FindViewById<ImageView>(Resource.Id.videoicon);
            TextView videoinfo = FindViewById<TextView>(Resource.Id.videoinfo);
            videoinfo.SetTypeface(AppFont.GetText(this), TypefaceStyle.Normal);
            TextView videotext = FindViewById<TextView>(Resource.Id.videotext);
            videotext.SetTypeface(AppFont.GetText(this), TypefaceStyle.Normal);
            videoheader = FindViewById<TextView>(Resource.Id.videoheader);
            videoheader.SetTypeface(AppFont.GetText(this), TypefaceStyle.Normal);
            Selectimagelayout = FindViewById<RelativeLayout>(Resource.Id.Selectimagelayout);
            ImageView Selectimage = FindViewById<ImageView>(Resource.Id.Selectimage);
            ImageView Selectimage2 = FindViewById<ImageView>(Resource.Id.Selectimage2);
            ImageView Removeimage = FindViewById<ImageView>(Resource.Id.Removeimage);
            TextView EnterOthernotes = FindViewById<TextView>(Resource.Id.EnterOthernotes);
            EnterOthernotes.SetTypeface(AppFont.GetText(this), TypefaceStyle.Normal);
            EditText EditOthernotes = FindViewById<EditText>(Resource.Id.EditOthernotes);
            EditOthernotes.SetTypeface(AppFont.GetText(this), TypefaceStyle.Normal);
            videoicon.Click += (o, e) => PressvideoButton();
            videolayout.Click += (o, e) => videoicon.PerformClick();
            videotext.Click += (o, e) => videoicon.PerformClick();
            Removeimage.Click += (o, e) => PressRemoveButton();
            ScrollView Scrollview = FindViewById<ScrollView>(Resource.Id.Scrollview);
            Dialog_saveoption = new Dialog(this, Resource.Style.AppThemeTransp);
            Dialog_saveoption.SetCanceledOnTouchOutside(true);
            Dialog_saveoption.SetContentView(Resource.Layout.Buttonlayout);
            ImageView CloseButtonoption = Dialog_saveoption.FindViewById<ImageView>(Resource.Id.Backbutton2);
            TextView headingtext = Dialog_saveoption.FindViewById<TextView>(Resource.Id.headingtext);
            headingtext.SetTypeface(AppFont.GetTitle(this), TypefaceStyle.Normal);
            headingtext.Text = "VIDEO UPDATES";
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
            ButtonSave.Click += (o, e) => PressSaveButton();
            ButtonSave.SetTypeface(AppFont.GetButton(this), TypefaceStyle.Normal);
            String Livestatus = Intent.GetStringExtra("video");
            if (!String.IsNullOrEmpty(Livestatus))
            {
                Selectimagelayout.Visibility = ViewStates.Visible;
                videolayout.Visibility = ViewStates.Gone;
                videoheader.Text = "Upload new video";
                Bitmap bmThumbnail = ThumbnailUtils.CreateVideoThumbnail(Livestatus, Android.Provider.ThumbnailKind.MiniKind);
                Selectimage.SetImageBitmap(bmThumbnail);
                Selectimage.Click += (o, e) => PlayvideoButton(Livestatus);
                Selectimage2.Click += (o, e) => PlayvideoButton(Livestatus);
                //Selectimage.SetImageURI(Android.Net.Uri.Parse(Livestatus));
                EditOthernotes.Text = notes;
            }

            EditOthernotes.SetOnTouchListener(new EditTextTouch());
            // Handel Events when the Password Text on editing
            EditOthernotes.AfterTextChanged += delegate (object sender, Android.Text.AfterTextChangedEventArgs e)
            {
                notes = EditOthernotes.Text;
            };
        }
        // When Click Save button method calls
        private void PressRemoveButton()
        {
            Selectimagelayout.Visibility = ViewStates.Gone;
            videolayout.Visibility = ViewStates.Visible;
            videoheader.Text = "Upload new video";
        }
        public bool OnTouch(View v, MotionEvent e)
        {
            hideSoftKeyboard();
            return false;
        }
        protected override void OnResume()
        {
            base.OnResume();
            if (objProgressbar.IsShowing)
                objProgressbar.Dismiss();
        }
        // When Click Save button method calls
        private void PlayvideoButton(String videourl)
        {
            Android.Net.Uri uri = Android.Net.Uri.Parse(videourl);
            Intent intent = new Intent(Intent.ActionView, uri);
            intent.SetDataAndType(uri, "video/*");
            StartActivity(intent);
        }
        // When Click Save button method calls
        private void PressSaveButton()
        {
            if (videolayout.Visibility == ViewStates.Gone)
            {
                Dialog_saveoption.Show();
            }
            else
            {
                Alertpopup("Missing video");
            }
        }
        // When Click Save button method calls
        private void PressvideoButton()
        {
           // objProgressbar.Show();
            StartActivity(new Intent(this, typeof(Findvideo)));
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