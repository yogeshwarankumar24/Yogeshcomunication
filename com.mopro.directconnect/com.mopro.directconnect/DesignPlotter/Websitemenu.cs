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
using Android.Content.PM;
using Antistatic.Spinnerwheel;
using Antistatic.Spinnerwheel.Adapters;
using Android.Graphics;

namespace com.mopro.directconnect
{
    [Activity(Theme = "@style/AppThemeTransp", Label = "Websitemenu", ScreenOrientation = ScreenOrientation.Portrait)]
    public class Websitemenu : Activity
    {
        List<String> objBussinessdata;
        WheelVerticalView _daySpinner;
        Java.Lang.Object[] objectdata;
        String screenstatus, menuname;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Websitemenu);
            Window.SetSoftInputMode(SoftInput.StateHidden);
            TextView headingtext3 = FindViewById<TextView>(Resource.Id.headingtext3);
            headingtext3.SetTypeface(AppFont.GetTitle(this), Android.Graphics.TypefaceStyle.Normal);
            TextView Donebutton = FindViewById<TextView>(Resource.Id.Donebutton);
            Donebutton.Click += (o, e) => PressHeaderButton();
            screenstatus = Intent.GetStringExtra("screen");
            menuname = Intent.GetStringExtra("menuname");
            objBussinessdata = new List<String>() { "Home", "About", "Team", "Gallery", "Contact", "Testimonials" };
           // Click Back button Events Occurs below
            ImageView Backbutton = FindViewById<ImageView>(Resource.Id.Backbutton);
            Backbutton.Click += (o, e) => PressHeaderButton();
            if (screenstatus == "Filter")
                objBussinessdata.Insert(0, "ALL");
            _daySpinner = (WheelVerticalView)FindViewById(Resource.Id.Spinner);
            objectdata = new Java.Lang.Object[objBussinessdata.Count];
            for(int i=0;i< objBussinessdata.Count;i++)
            {
                objectdata[i] = objBussinessdata[i];
            }
            var ampmAdapter = new ArrayWheelAdapter(this, objectdata)
            {
                ItemResource = Resource.Layout.Wheeltext,
                ItemTextResource = Resource.Id.text,
                TextColor = Resource.Color.textcolor,                
            };
            _daySpinner.ViewAdapter = ampmAdapter;  
            _daySpinner.SetSelectionDivider(Resources.GetDrawable(Resource.Drawable.Menubutclick));
            //_daySpinner.SetActiveCoeff(-10);            
            _daySpinner.SetPassiveCoeff(1f);
            _daySpinner.Selected = true;
            if (String.IsNullOrEmpty(menuname))
                _daySpinner.SetCurrentItem(1, false);
            else
            {
                int currentitem = objBussinessdata.ConvertAll(d => d.ToLower()).IndexOf(menuname);
                _daySpinner.SetCurrentItem(currentitem, false);
            }
            _daySpinner.SetAllItemsVisible(true);
            FrameLayout Selectedlayout = FindViewById<FrameLayout>(Resource.Id.Selectedlayout);
            Selectedlayout.Click += (o, e) => PressHeaderButton();
        }
        // Methods call to Show and hide the content screen and Page screen on Click
        private void PressHeaderButton()
        {            
            if (String.IsNullOrEmpty(screenstatus))
            {
                Intent objIntent = new Intent(this, typeof(Website));
                objIntent.PutExtra("menu", true);
                String value = objectdata[_daySpinner.CurrentItem].ToString();
                objIntent.PutExtra("menuname", value);
                StartActivity(objIntent);
                OverridePendingTransition(Resource.Drawable.slide_in_top, Resource.Drawable.slide_out_top);
            }
            else if (screenstatus == "Filter")
            {
                Intent objIntent = new Intent(this, typeof(Requests));
                objIntent.PutExtra("menu", true);
                String value = objectdata[_daySpinner.CurrentItem].ToString();
                objIntent.PutExtra("menuname", value);
                StartActivity(objIntent);
                OverridePendingTransition(Resource.Drawable.slide_in_top, Resource.Drawable.slide_out_top);
            }
            else
            {
                Intent objIntent = new Intent(this, typeof(Linkchanges));
                objIntent.PutExtra("menu", true);
                String value = objectdata[_daySpinner.CurrentItem].ToString();
                objIntent.PutExtra("menuname", value);
                StartActivity(objIntent);
                OverridePendingTransition(Resource.Drawable.slide_in_top, Resource.Drawable.slide_out_top);
            }
        }
        // Click Back button Events Occurs below
        public override void OnBackPressed()
        {
            base.OnBackPressed();
            PressHeaderButton();
        }
    }
}