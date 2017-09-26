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
    [Activity(Label = "Direct Connect", ScreenOrientation = ScreenOrientation.Portrait)]
    public class Bizoption : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Bizoption);
            Window.SetSoftInputMode(SoftInput.StateHidden);
            ListView Biz_ListView = FindViewById<ListView>(Resource.Id.Listview);
            // Populate Default values for testing
            List<Bussinessclass> objBussinessdata = new List<Bussinessclass>();
            objBussinessdata.Add(new Bussinessclass() { id="1" });
            objBussinessdata.Add(new Bussinessclass() { id = "2" });
            Biz_ListView.SetAdapter(new AdapterBussinessList(this, objBussinessdata));
            // Click Listview Sites handel with below code
            Biz_ListView.ItemClick += (sender, e) =>
            {
                Biz_ListView.Selected = false;
                StartActivity(new Intent(this, typeof(Home)));
                OverridePendingTransition(Resource.Drawable.slide_from_right, Resource.Drawable.slide_to_left);
            };
            TextView headingtext = FindViewById<TextView>(Resource.Id.headingtext);
            headingtext.SetTypeface(AppFont.GetTitle(this), TypefaceStyle.Normal);
            TextView headingname = FindViewById<TextView>(Resource.Id.headingname);
            headingname.SetTypeface(AppFont.GetTitle(this), TypefaceStyle.Normal);
        }
        // Click Back button Events Occurs below
        public override void OnBackPressed()
        {
            FinishAffinity();
            base.OnBackPressed();
        }
    }
    // Adapter For Populating the Website List
    public class AdapterBussinessList : BaseAdapter<Bussinessclass>
    {
        List<Bussinessclass> Listitems;
        Activity context;
        public AdapterBussinessList(Activity context, List<Bussinessclass> Listitemsv) : base()
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
                view = context.LayoutInflater.Inflate(Resource.Layout.Bizoptionlist, null);
            //view.FindViewById<TextView>(Resource.Id.Bizname).Text = Listitems[position].Bussiness;
            TextView Bizname = view.FindViewById<TextView>(Resource.Id.Bizname);
            Bizname.SetTypeface(AppFont.GetText(context), TypefaceStyle.Bold);
            TextView Bizlocation = view.FindViewById<TextView>(Resource.Id.Bizlocation);
            Bizlocation.SetTypeface(AppFont.GetText(context), TypefaceStyle.Bold);
            TextView Bizwebsite = view.FindViewById<TextView>(Resource.Id.Bizwebsite);
            Bizwebsite.SetTypeface(AppFont.GetText(context), TypefaceStyle.Bold);
            TextView Bizlocationtxt = view.FindViewById<TextView>(Resource.Id.Bizlocationtxt);
            Bizlocationtxt.SetTypeface(AppFont.GetText(context), TypefaceStyle.Normal);
            TextView Bizwebsitetxt = view.FindViewById<TextView>(Resource.Id.Bizwebsitetxt);
            Bizwebsitetxt.SetTypeface(AppFont.GetText(context), TypefaceStyle.Normal);
            return view;
        }
    }
}