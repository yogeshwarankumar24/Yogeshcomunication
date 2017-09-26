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
using FFImageLoading;
using FFImageLoading.Views;
using Android.Media;

namespace com.mopro.directconnect
{
    [Activity(Label = "Imageupdates", ScreenOrientation = ScreenOrientation.Portrait)]
    public class Imageupdates : Activity
    {
        TextView imageheader;
        Dialog Dialog_saveoption;
        ImageViewAsync Selectimage;
        RelativeLayout imagelayout, Selectimagelayout;
        public static String notes { get; set; }
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Imageupdates);
            Window.SetSoftInputMode(SoftInput.AdjustResize | SoftInput.StateHidden);
            // Click Back button Events Occurs below
            ImageView Backbutton = FindViewById<ImageView>(Resource.Id.Backbutton);
            Backbutton.Click += (o, e) => TextRevision.PressedBackButton(this);
            TextView headingtxt = FindViewById<TextView>(Resource.Id.headingtext);
            headingtxt.SetTypeface(AppFont.GetTitle(this), TypefaceStyle.Normal);
            TextView EnterOthertext = FindViewById<TextView>(Resource.Id.EnterOthertext);
            EnterOthertext.SetTypeface(AppFont.GetText(this), TypefaceStyle.Normal);
            TextView imagetext = FindViewById<TextView>(Resource.Id.imagetext);
            imagetext.SetTypeface(AppFont.GetText(this), TypefaceStyle.Normal);
            TextView imageinfo = FindViewById<TextView>(Resource.Id.imageinfo);
            imageinfo.SetTypeface(AppFont.GetText(this), TypefaceStyle.Normal);
            EditText EditOthernotes = FindViewById<EditText>(Resource.Id.EditOthernotes);
            EditOthernotes.SetTypeface(AppFont.GetText(this), TypefaceStyle.Normal);
            Button ButtonSave = FindViewById<Button>(Resource.Id.ButtonSave);
            imagelayout = FindViewById<RelativeLayout>(Resource.Id.imagelayout);
            Selectimagelayout = FindViewById<RelativeLayout>(Resource.Id.Selectimagelayout);
            imageheader = FindViewById<TextView>(Resource.Id.imageheader);
            imageheader.SetTypeface(AppFont.GetText(this), TypefaceStyle.Normal);
            ImageView imageicon = FindViewById<ImageView>(Resource.Id.imageicon);
            Selectimage = FindViewById<ImageViewAsync>(Resource.Id.Selectimage);
            ImageView Removeimage = FindViewById<ImageView>(Resource.Id.Removeimage);
            imageicon.Click += (o, e) => PressImageButton();
            imagelayout.Click += (o, e) => imageicon.PerformClick();
            imagetext.Click += (o, e) => imageicon.PerformClick();
            Selectimage.Click += (o, e) => imageicon.PerformClick();
            Removeimage.Click += (o, e) => PressRemoveButton();
            ScrollView Scrollview = FindViewById<ScrollView>(Resource.Id.Scrollview);
            Dialog_saveoption = new Dialog(this, Resource.Style.AppThemeTransp);
            Dialog_saveoption.SetCanceledOnTouchOutside(true);
            Dialog_saveoption.SetContentView(Resource.Layout.Buttonlayout);
            ImageView CloseButtonoption = Dialog_saveoption.FindViewById<ImageView>(Resource.Id.Backbutton2);
            TextView headingtext = Dialog_saveoption.FindViewById<TextView>(Resource.Id.headingtext);
            headingtext.SetTypeface(AppFont.GetTitle(this), TypefaceStyle.Normal);
            headingtext.Text = "IMAGE UPDATES";
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
            String Livestatus = Intent.GetStringExtra("image");
            if (!String.IsNullOrEmpty(Livestatus))
            {
                Selectimagelayout.Visibility = ViewStates.Visible;
                imagelayout.Visibility = ViewStates.Gone;
                imageheader.Text = "Selected image";
                    LoadImage(Livestatus);
                    EditOthernotes.Text = notes;
            }

            EditOthernotes.SetOnTouchListener(new EditTextTouch());
            // Handel Events when the Password Text on editing
            EditOthernotes.AfterTextChanged += delegate (object sender, Android.Text.AfterTextChangedEventArgs e)
            {
                notes = EditOthernotes.Text;
            };

        }
        public void LoadImage(string Livestatus)
        {
            ExifInterface ei = new ExifInterface(Livestatus);
            int orientation = ei.GetAttributeInt(ExifInterface.TagOrientation, 44);
            switch (orientation)
            {
                case (int)Android.Media.Orientation.Rotate90:
                    Rotateimage(Livestatus, 90);
                    break;
                case (int)Android.Media.Orientation.Rotate180:
                    Rotateimage(Livestatus, 180);
                    break;
                case (int)Android.Media.Orientation.Rotate270:
                    Rotateimage(Livestatus, 270);
                    break;
                case 44:
                    Rotateimage(Livestatus, 0);
                    break;
                default:
                    Rotateimage(Livestatus, 0);
                    break;
            }
        }
        void Rotateimage(String Livestatus,int degree)
        {
            try
            {
                Android.Graphics.Bitmap mBitmap = null;
                mBitmap = BitmapFactory.DecodeFile(Livestatus);
                if (mBitmap != null)
                {
                    Matrix matrix = new Matrix();
                    matrix.PostRotate(degree);
                    Selectimage.SetImageBitmap(Bitmap.CreateBitmap(mBitmap, 0, 0, mBitmap.Width, mBitmap.Height, matrix, true));
                } else {
                    PressRemoveButton();
                    Alertpopup("Inavalid Image Format. Try again.");
                }
            } catch {
                PressRemoveButton();
                Alertpopup("Inavalid Image Format. Try again.");
            }
        }
        public bool OnTouch(View v, MotionEvent e)
        {
            hideSoftKeyboard();
            return true;
        }
        // When Click Save button method calls
        private void PressSaveButton()
        {
            if (imagelayout.Visibility == ViewStates.Gone)
            {
                Dialog_saveoption.Show();
            }
            else
            {
                Alertpopup("Missing image");
            }
        }
        // When Click Save button method calls
        private void PressImageButton()
        {
            StartActivity(new Intent(this, typeof(Findimage)));
            OverridePendingTransition(Resource.Drawable.slide_from_right, Resource.Drawable.slide_to_left);
        }
        // When Click Save button method calls
        private void PressRemoveButton()
        {
            Selectimagelayout.Visibility = ViewStates.Gone;
            imagelayout.Visibility = ViewStates.Visible;
            imageheader.Text = "Upload new image";
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