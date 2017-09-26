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
    public class Forgetpassword : Activity
    {
        EditText EditEmailid;
        TextView Forgetcontent;
        LinearLayout Forgetscreen, ForgetSucess;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Forget);
            Window.SetSoftInputMode(SoftInput.AdjustPan | SoftInput.StateHidden);
            Button ButtonCancel = FindViewById<Button>(Resource.Id.ButtonCancel);
            Button ButtonLogin = FindViewById<Button>(Resource.Id.ButtonLogin);
            Button ButtonReset = FindViewById<Button>(Resource.Id.ButtonReset);
            ButtonReset.SetTypeface(AppFont.GetText(this), TypefaceStyle.Normal);
            ButtonLogin.SetTypeface(AppFont.GetText(this), TypefaceStyle.Normal);
            EditEmailid = FindViewById<EditText>(Resource.Id.EditEmailid);
            Forgetcontent = FindViewById<TextView>(Resource.Id.Forgetcontent);
            Forgetscreen = FindViewById<LinearLayout>(Resource.Id.Forgetscreen);
            ForgetSucess = FindViewById<LinearLayout>(Resource.Id.ForgetSucess);
            EditEmailid.SetTypeface(AppFont.GetText(this), TypefaceStyle.Normal);
            Forgetcontent.SetTypeface(AppFont.GetText(this), TypefaceStyle.Normal);
            TextView Sucesscontent = FindViewById<TextView>(Resource.Id.Sucesscontent);
            Sucesscontent.SetTypeface(AppFont.GetText(this), TypefaceStyle.Normal);
            ButtonCancel.Click += (o, e) => PressCancelButton();
            ButtonLogin.Click += (o, e) => PressCancelButton();
            ButtonReset.Click += (o, e) => PressResetButton();
            TextView Senttext = FindViewById<TextView>(Resource.Id.Senttext);
            Senttext.SetTypeface(AppFont.GetButton(this), TypefaceStyle.Normal);

            // Handel Events when the Email Text on editing
            EditEmailid.AfterTextChanged += delegate (object sender, Android.Text.AfterTextChangedEventArgs e)
            {
                EditEmailid.SetBackgroundResource(Resource.Drawable.Edittextbg);
                EditEmailid.SetHintTextColor(Resources.GetColor(Resource.Color.textcolor));
            };
        }
        // Handel Events Onoutside Touch
        public override bool OnTouchEvent(MotionEvent e)
        {
            hideSoftKeyboard();
            return base.OnTouchEvent(e);
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
        // When Click Cancel button method calls to redirect
        private void PressCancelButton()
        {
            Finish();
            OverridePendingTransition(Resource.Drawable.slide_in_top, Resource.Drawable.slide_out_top);
        }
        // When Click Reset button method calls to redirect
        private void PressResetButton()
        {
            // Validation for the Email-id
            AppValidation.EmailValidation(EditEmailid);
            if (AppValidation.EmailValidation(EditEmailid))
            {
                hideSoftKeyboard();
                Forgetscreen.Visibility = ViewStates.Gone;
                ForgetSucess.Visibility = ViewStates.Visible;
            } else
            {
                if(!String.IsNullOrWhiteSpace(EditEmailid.Text))
                    // Alert for invalid Email-id
                    Alertpopup(Resources.GetString(Resource.String.invalidemail));
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
            PressCancelButton();
        }
    }
}