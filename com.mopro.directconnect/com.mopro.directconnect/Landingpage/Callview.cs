using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Views.Animations;
using Android.Widget;
using static Android.Resource;
using static Android.Views.Animations.Animation;
using Android.Graphics;
using Android.Content.PM;

namespace com.mopro.directconnect
{
    [Activity(Theme = "@style/AppThemeTransp", Label = "Callview", ScreenOrientation = ScreenOrientation.Portrait)]
    public class Callview : Activity, IAnimationListener
    {
        Android.Views.Animations.Animation Zoomin, Zoomout;
        ImageView Calllogo;
        bool animationss;
        ImageView mikedisable, speakdisable;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Callview);
            Window.SetSoftInputMode(SoftInput.StateHidden);
            Zoomin = AnimationUtils.LoadAnimation(this, Resource.Drawable.Zoomin);
            Zoomout = AnimationUtils.LoadAnimation(this, Resource.Drawable.Zoomout);
            Calllogo = FindViewById<ImageView>(Resource.Id.Calllogo);
            Calllogo.StartAnimation(Zoomin);
            Zoomin.SetAnimationListener(this);
            TextView Calltext = FindViewById<TextView>(Resource.Id.Calltext);
            Calltext.SetTypeface(AppFont.GetText(this), TypefaceStyle.Normal);
            TextView Callname = FindViewById<TextView>(Resource.Id.Callname);
            Callname.SetTypeface(AppFont.GetText(this), TypefaceStyle.Normal);
            ImageView Calldisable = FindViewById<ImageView>(Resource.Id.Calldisable);
            Calldisable.Click += (o, e) => {
                StartActivity(new Intent(this,typeof(CallReview)));
                OverridePendingTransition(Resource.Drawable.slide_from_right, Resource.Drawable.slide_to_left);
            };
            mikedisable = FindViewById<ImageView>(Resource.Id.mikedisable);
            mikedisable.Click += (o, e) => PressMikeButton();
            mikedisable.SetImageResource(Resource.Drawable.mute);
            speakdisable = FindViewById<ImageView>(Resource.Id.speakdisable);
            speakdisable.Click += (o, e) => PressSpeakButton();
            speakdisable.SetImageResource(Resource.Drawable.speaker);
        }
        bool mikestatus;
        // When Click Mike button method calls
        private void PressMikeButton()
        {
            if (mikestatus)
            {
                mikestatus = false;
                mikedisable.SetImageResource(Resource.Drawable.mute);
            } else
            {
                mikestatus = true;
                mikedisable.SetImageResource(Resource.Drawable.mute_c);
            }
        }
        bool speakstatus;
        // When Click Speak button method calls
        private void PressSpeakButton()
        {
            if (speakstatus)
            {
                speakstatus = false;
                speakdisable.SetImageResource(Resource.Drawable.speaker);
            }
            else
            {
                speakstatus = true;
                speakdisable.SetImageResource(Resource.Drawable.Speakoff);
            }
        }
        // Click Back button Events Occurs below
        public override void OnBackPressed()
        {
            base.OnBackPressed();
            StartActivity(new Intent(this, typeof(Home)));
            OverridePendingTransition(Resource.Drawable.slide_zoom_out, Resource.Drawable.fade_out);
        }
        public void OnAnimationEnd(Android.Views.Animations.Animation animation)
        {
            if (animationss)
            {
                Calllogo.StartAnimation(Zoomin);
                Zoomin.SetAnimationListener(this);
            }
            else
            {
                Calllogo.StartAnimation(Zoomout);
                Zoomout.SetAnimationListener(this);
            }
            animationss = !animationss;
        }
        public void OnAnimationRepeat(Android.Views.Animations.Animation animation)
        {
        }
        public void OnAnimationStart(Android.Views.Animations.Animation animation)
        {
        }
    }
}