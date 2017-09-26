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
using System.Threading.Tasks;

namespace com.mopro.directconnect
{
    [Activity(Label = "Direct Connect", ScreenOrientation = ScreenOrientation.Portrait)]
    public class Login : Activity
    {
        EditText EditUsername, EditPassword;
        Button CheckRemember;
        Button ButtonForget;
        bool Checkbox;
        Button ButtonLogin;
        Progressbar objProgressbar;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Login);
            Window.SetSoftInputMode(SoftInput.AdjustPan|SoftInput.StateHidden);
            VideoView Videoview = FindViewById<VideoView>(Resource.Id.videoview);
            ImageHandler.AppBackgroundview(Videoview);
            objProgressbar = new Progressbar(this);
            EditUsername = FindViewById<EditText>(Resource.Id.EditUsername);
            EditPassword = FindViewById<EditText>(Resource.Id.EditPassword);
            ButtonLogin = FindViewById<Button>(Resource.Id.ButtonLogin);
            ButtonLogin.SetTypeface(AppFont.GetText(this), TypefaceStyle.Normal);
            ButtonForget = FindViewById<Button>(Resource.Id.ButtonForget);
            CheckRemember = FindViewById<Button>(Resource.Id.CheckRemember);
            TextView checkedtext = FindViewById<TextView>(Resource.Id.checkedtext);
            ButtonForget.SetTypeface(AppFont.GetText(this), TypefaceStyle.Normal);
            EditUsername.SetTypeface(AppFont.GetText(this), TypefaceStyle.Normal);
            EditPassword.SetTypeface(AppFont.GetText(this), TypefaceStyle.Normal);
            checkedtext.SetTypeface(AppFont.GetText(this), TypefaceStyle.Normal);
            CheckRemember.Click += (o, e) => PressCheckBox();
            checkedtext.Click += (o, e) => PressCheckBox();
            ButtonLogin.Click += (o, e) => PressLoginButton();
            ButtonForget.Click += (o, e) => PressForgetButton();
            // Handel Events when the Username Text on editing
            EditUsername.AfterTextChanged += delegate (object sender, Android.Text.AfterTextChangedEventArgs e)
            {
                EditUsername.SetBackgroundResource(Resource.Drawable.Edittextbg);
                EditUsername.SetHintTextColor(Resources.GetColor(Resource.Color.textcolor));
            };
            // Handel Events when the Password Text on editing
            EditPassword.AfterTextChanged += delegate (object sender, Android.Text.AfterTextChangedEventArgs e)
            {
                EditPassword.SetBackgroundResource(Resource.Drawable.Edittextbg);
                EditPassword.SetHintTextColor(Resources.GetColor(Resource.Color.textcolor));
            };
            EditUsername.RequestFocus();
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
        // Method used to Update remember me checkbox
        private void PressCheckBox()
        {
            if (Checkbox)
            {
                Checkbox = false;
                CheckRemember.SetBackgroundResource(Resource.Drawable.uncheckbox);
            }
            else
            {
                Checkbox = true;
                CheckRemember.SetBackgroundResource(Resource.Drawable.checkedbox);
            }
        }
        
        // When Click login button method calls
       async private void PressLoginButton()
        {
            AppValidation.EmailValidation(EditUsername);
            AppValidation.PasswordValidation(EditPassword, EditPassword);
            
            if (String.IsNullOrEmpty(EditUsername.Text.ToString()) && String.IsNullOrEmpty(EditPassword.Text.ToString()))
            {
                // Alert for invalid users
                Alertpopup("Please fill in all Information");
                return;
            }
            else if (String.IsNullOrEmpty(EditUsername.Text.ToString()) && !String.IsNullOrEmpty(EditPassword.Text.ToString()))
            {
                // Alert for invalid users
                Alertpopup("Please fill in all Information");
                return;
            }
            else if(!String.IsNullOrEmpty(EditUsername.Text.ToString()) && String.IsNullOrEmpty(EditPassword.Text.ToString()))
            {
                // Alert for invalid users
                if (!AppValidation.EmailValidation(EditUsername))
                    Alertpopup(Resources.GetString(Resource.String.invalidnamepassword));
                else  // Alert for invalid users
                    Alertpopup("Please fill in all Information");

                return;
            }
            if (AppValidation.EmailValidation(EditUsername) && AppValidation.PasswordValidation(EditPassword, EditPassword))
            {
                // Validation for the Default users for testing
                if (EditUsername.Text == "mopro@dc.com" && EditPassword.Text == "mopro123")
                {
                    objProgressbar.Show(AppValidation.PointOfView(ButtonLogin));
                    await Task.Delay(1000);
                    AppPreferences objAppPreferences = new AppPreferences(this);
                    objAppPreferences.SaveUserdetails(EditUsername.Text, EditPassword.Text);
                    StartActivity(new Intent(this, typeof(Getstarted)));
                    OverridePendingTransition(Resource.Drawable.fade_in, Resource.Drawable.fade_out);
                }
                else if (EditUsername.Text == "mopro@dc.com" && EditPassword.Text == "mopro456")
                {
                    objProgressbar.Show(AppValidation.PointOfView(ButtonLogin));
                    AppPreferences objAppPreferences = new AppPreferences(this);
                    objAppPreferences.SaveUserdetails(EditUsername.Text, EditPassword.Text);
                    Intent objIntent = new Intent(this, typeof(Home));
                    objIntent.PutExtra("status", true);
                    StartActivity(objIntent);
                    OverridePendingTransition(Resource.Drawable.slide_from_left, Resource.Drawable.slide_to_right);
                }
                else
                {
                    // Alert for invalid users
                    Alertpopup(Resources.GetString(Resource.String.invalidnamepassword));
                }
            }
            else if (!Android.Util.Patterns.EmailAddress.Matcher(EditUsername.Text.ToString()).Matches())
            {
                // Alert for invalid users
                Alertpopup("Please enter valid email");
                return;
            }
        }        
        protected override void OnResume()
        {
            base.OnResume();
            if(objProgressbar.IsShowing)
            objProgressbar.Dismiss();
        }
        // When Click Forget Password button method calls to redirect
        private void PressForgetButton()
        {
            objProgressbar.Show();
            StartActivity(new Intent(this, typeof(Forgetpassword)));
            OverridePendingTransition(Resource.Drawable.slide_in_bottom, Resource.Drawable.slide_out_bottom);
        }
        // Shows Alert for Inavlid User Login
        void Alertpopup(String Content)
        {
            AlertDialog.Builder alertDialog = new AlertDialog.Builder(this);
            alertDialog.SetTitle(Resources.GetString(Resource.String.error));
            alertDialog.SetMessage(Content);
            alertDialog.SetPositiveButton("OK", delegate
            {
                EditPassword.Text = "";
                alertDialog.Dispose();
            });
            alertDialog.Show();
        }
        // Click Back button Events Occurs below
        public override void OnBackPressed()
        {
            FinishAffinity();
            base.OnBackPressed();
        }
    }
}