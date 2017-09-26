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
using Android.Content.PM;

namespace com.mopro.directconnect
{
    [Activity(Label = "Requests", ScreenOrientation = ScreenOrientation.Portrait)]
    public class Requests : Activity
    {
        RelativeLayout Progresslayout, Completedlayout;
        RelativeLayout Infolayout;
        TextView Filtercontenttext;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Requests);
            Window.SetSoftInputMode(SoftInput.StateHidden);
            ImageView Backbutton = FindViewById<ImageView>(Resource.Id.Backbutton);
            Backbutton.Click += (o, e) => {
                StartActivity(new Intent(this, typeof(Home)));
                OverridePendingTransition(Resource.Drawable.slide_from_left, Resource.Drawable.slide_to_right);
            };
            TextView headingtxt = FindViewById<TextView>(Resource.Id.headingtext);
            headingtxt.SetTypeface(AppFont.GetTitle(this), TypefaceStyle.Normal);
            ListView Biz_ListView = FindViewById<ListView>(Resource.Id.Listview);
            // Populate Default values for testing
            List<Bussinessclass> objBussinessdata = new List<Bussinessclass>();
            objBussinessdata.Add(new Bussinessclass() { id = "1" });
            objBussinessdata.Add(new Bussinessclass() { id = "2" });
            Biz_ListView.SetAdapter(new AdapterRequestList(this, objBussinessdata));
            AppValidation.ListViewHeight(Biz_ListView, objBussinessdata.Count, Biz_ListView.Adapter);
            // Click Listview Sites handel with below code
            Biz_ListView.ItemClick += (sender, e) =>
            {
                StartActivity(new Intent(this, typeof(Requestdetails)));
                OverridePendingTransition(Resource.Drawable.slide_from_right, Resource.Drawable.slide_to_left);
            };
            TextView Infocount = FindViewById<TextView>(Resource.Id.Infocount);
            TextView Infotext = FindViewById<TextView>(Resource.Id.Infotext);
            Infolayout = FindViewById<RelativeLayout>(Resource.Id.Infolayout);
            Infolayout.Click += (o, e) => PressInfoButton();
            TextView Progresscount = FindViewById<TextView>(Resource.Id.Progresscount);
            TextView Progresstext = FindViewById<TextView>(Resource.Id.Progresstext);
            Progresslayout = FindViewById<RelativeLayout>(Resource.Id.Progresslayout);
            Progresslayout.Click += (o, e) => PressProgressButton();
            TextView Completedcount = FindViewById<TextView>(Resource.Id.Completedcount);
            TextView Completedtext = FindViewById<TextView>(Resource.Id.Completedtext);
            Completedlayout = FindViewById<RelativeLayout>(Resource.Id.Completedlayout);
            Completedlayout.Click += (o, e) => PressCompletedButton();
            Infocount.SetTypeface(AppFont.GetTitle(this), TypefaceStyle.Normal);
            Infotext.SetTypeface(AppFont.GetButton(this), TypefaceStyle.Normal);
            Progresscount.SetTypeface(AppFont.GetTitle(this), TypefaceStyle.Normal);
            Progresstext.SetTypeface(AppFont.GetButton(this), TypefaceStyle.Normal);
            Completedcount.SetTypeface(AppFont.GetTitle(this), TypefaceStyle.Normal);
            Completedtext.SetTypeface(AppFont.GetButton(this), TypefaceStyle.Normal);
            TextView Filtertext = FindViewById<TextView>(Resource.Id.Filtertext);
            TextView Bizheader = FindViewById<TextView>(Resource.Id.Bizheader);
            Bizheader.SetTypeface(AppFont.GetText(this), TypefaceStyle.Normal);

            ScrollView Scrollview = FindViewById<ScrollView>(Resource.Id.Scrollview);
            Filtercontenttext = FindViewById<TextView>(Resource.Id.Filtercontenttext);
            Filtertext.SetTypeface(AppFont.GetButton(this), TypefaceStyle.Normal);
            Filtercontenttext.SetTypeface(AppFont.GetButton(this), TypefaceStyle.Normal);
            Filtercontenttext.Click += (o, e) => PressHeaderButton();
            bool Menustatus = Intent.GetBooleanExtra("menu", false);
            if (Menustatus)
            {
                Filtercontenttext.Text = Intent.GetStringExtra("menuname");
            }
            Scrollview.ScrollTo(0, 0);
        }
        // Methods call to Show and hide the content screen and Page screen on Click
        private void PressHeaderButton()
        {
            Intent objIntent = new Intent(this, typeof(Websitemenu));
            objIntent.PutExtra("screen", "Filter");
            if (!String.IsNullOrEmpty(Filtercontenttext.Text))
                objIntent.PutExtra("menuname", Filtercontenttext.Text.ToLower());
            StartActivity(objIntent);
            OverridePendingTransition(Resource.Drawable.slide_in_bottom, Resource.Drawable.slide_out_bottom);
        }
        private void PressInfoButton()
        {
            Infolayout.SetBackgroundResource(Resource.Drawable.Requestselectbg);
            Progresslayout.SetBackgroundResource(Resource.Drawable.Requestbg);
            Completedlayout.SetBackgroundResource(Resource.Drawable.Requestbg);
        }
        private void PressProgressButton()
        {
            Infolayout.SetBackgroundResource(Resource.Drawable.Requestbg);
            Progresslayout.SetBackgroundResource(Resource.Drawable.Requestselectbg);
            Completedlayout.SetBackgroundResource(Resource.Drawable.Requestbg);
        }
        private void PressCompletedButton()
        {
            Infolayout.SetBackgroundResource(Resource.Drawable.Requestbg);
            Progresslayout.SetBackgroundResource(Resource.Drawable.Requestbg);
            Completedlayout.SetBackgroundResource(Resource.Drawable.Requestselectbg);
        }
        // Click Back button Events Occurs below
        public override void OnBackPressed()
        {
            base.OnBackPressed();
            StartActivity(new Intent(this, typeof(Home)));
            OverridePendingTransition(Resource.Drawable.slide_from_left, Resource.Drawable.slide_to_right);
        }
    }
    // Adapter For Populating the Website List
    public class AdapterRequestList : BaseAdapter<Bussinessclass>
    {
        List<Bussinessclass> Listitems;
        Activity context;
        public AdapterRequestList(Activity context, List<Bussinessclass> Listitemsv) : base()
        {
            this.context = context;
            this.Listitems = Listitemsv;
        }
        // Assign position for every item
        public override long GetItemId(int position)
        {
            return position;
        }
        public override Bussinessclass this[int position]
        {
            get { return Listitems[position]; }
        }
        public override int Count
        {
            get { return Listitems.Count; }
        }
        //This method is used to bind the datas and returing View to User
        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View view = convertView;
            if (view == null)
                view = context.LayoutInflater.Inflate(Resource.Layout.Requestlist, null);
            //view.FindViewById<TextView>(Resource.Id.Bizname).Text = Listitems[position].Bussiness;
            TextView Bizname = view.FindViewById<TextView>(Resource.Id.Bizname);
            Bizname.SetTypeface(AppFont.GetText(context), TypefaceStyle.Normal);
            TextView Bizlocation = view.FindViewById<TextView>(Resource.Id.Bizlocation);
            Bizlocation.SetTypeface(AppFont.GetText(context), TypefaceStyle.Normal);
            TextView Bizwebsite = view.FindViewById<TextView>(Resource.Id.Bizwebsite);
            Bizwebsite.SetTypeface(AppFont.GetText(context), TypefaceStyle.Normal);
            TextView Bizstatus = view.FindViewById<TextView>(Resource.Id.Bizstatus);
            Bizstatus.SetTypeface(AppFont.GetText(context), TypefaceStyle.Normal);
            TextView Bizhome = view.FindViewById<TextView>(Resource.Id.Bizhome);
            Bizhome.SetTypeface(AppFont.GetText(context), TypefaceStyle.Normal);
            TextView Bizwebsite2 = view.FindViewById<TextView>(Resource.Id.Bizwebsite2);
            Bizwebsite2.SetTypeface(AppFont.GetText(context), TypefaceStyle.Normal);
            return view;
        }
    }
}