using Android.App;
using Android.Widget;
using Android.OS;
using System.Threading.Tasks;
using System;
using Android.Content;
using Android.Support.V4.Content;
using Android;
using Android.Support.V4.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Graphics;

namespace com.mopro.directconnect
{
    [Activity(Label = "DirectConnect", MainLauncher = true, ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            // Set our view from the "main" layout resource
            SetContentView (Resource.Layout.Main);
            if (Build.VERSION.SdkInt >= Build.VERSION_CODES.Lollipop)
            {
                Window.SetStatusBarColor(Resources.GetColor(Resource.Color.abc_primary_text_material_light));
            }
            AccessPermissions();
        }
        private void AccessPermissions()
        {
            if (Build.VERSION.SdkInt >= Build.VERSION_CODES.M)
            {
                if ((ContextCompat.CheckSelfPermission(this, Manifest.Permission.Camera) != Permission.Granted) && (ContextCompat.CheckSelfPermission(this, Manifest.Permission.RecordAudio) != Permission.Granted) && (ContextCompat.CheckSelfPermission(this, Manifest.Permission.WriteExternalStorage) != Permission.Granted))
                {
                    ActivityCompat.RequestPermissions(this, new String[] { Manifest.Permission.Camera, Manifest.Permission.RecordAudio, Manifest.Permission.ReadExternalStorage, Manifest.Permission.WriteExternalStorage }, 100);
                }
                else if ((ContextCompat.CheckSelfPermission(this, Manifest.Permission.Camera) == Permission.Granted) && (ContextCompat.CheckSelfPermission(this, Manifest.Permission.RecordAudio) == Permission.Granted) && (ContextCompat.CheckSelfPermission(this, Manifest.Permission.WriteExternalStorage) == Permission.Granted))
                {
                    StartMethod();
                }
                else
                {                     
                    if (ContextCompat.CheckSelfPermission(this, Manifest.Permission.Camera) != (int)Permission.Granted)
                    {
                        ActivityCompat.RequestPermissions(this, new String[] { Manifest.Permission.Camera }, 103);
                    }
                    if (ContextCompat.CheckSelfPermission(this, Manifest.Permission.ReadExternalStorage) != Permission.Granted)
                    {
                        ActivityCompat.RequestPermissions(this, new String[] { Manifest.Permission.ReadExternalStorage }, 103);
                    }
                    if (ContextCompat.CheckSelfPermission(this, Manifest.Permission.WriteExternalStorage) != Permission.Granted)
                    {
                        ActivityCompat.RequestPermissions(this, new String[] { Manifest.Permission.WriteExternalStorage }, 102);
                    }
                   
                   if (ContextCompat.CheckSelfPermission(this, Manifest.Permission.RecordAudio) != Permission.Granted)
                    {
                        ActivityCompat.RequestPermissions(this, new String[] { Manifest.Permission.RecordAudio }, 102);
                    }
                }
            }
            else
            {
                StartMethod();
            }
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            StartMethod();
        }
        // Start method to start Application
        async void StartMethod()
        {
            try
            {
                // Splash Screen is visible upto 2 secounds
                await Task.Delay(2000);
                AppPreferences objAppPreferences = new AppPreferences(this);
                if(objAppPreferences.Validateuser())
                {
                    if (objAppPreferences.username == "mopro@dc.com" && objAppPreferences.password == "mopro456")
                    {
                        Intent objIntent = new Intent(this, typeof(Home));
                        objIntent.PutExtra("status", true);
                        StartActivity(objIntent);
                        OverridePendingTransition(Resource.Drawable.fade_in, Resource.Drawable.fade_out);
                    }  else
                    {
                        // Redirect to the First Activity
                        StartActivity(new Intent(this, typeof(Bizoption)));
                        OverridePendingTransition(Resource.Drawable.fade_in, Resource.Drawable.fade_out);
                    }
                }
                else
                {
                    // Redirect to the First Activity
                    StartActivity(new Intent(this, typeof(Login)));
                    OverridePendingTransition(Resource.Drawable.fade_in, Resource.Drawable.fade_out);
                }
            }
            catch (Java.Lang.Exception ex)
            {
                Console.WriteLine("Error" + ex.ToString());
            }
        }
        // Click Back button Events Occurs below
        public override void OnBackPressed()
        {
            FinishAffinity();
            base.OnBackPressed();
        }
    }
    
}

