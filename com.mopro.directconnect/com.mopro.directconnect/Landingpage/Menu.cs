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
using Android.Support.V4.Widget;

namespace com.mopro.directconnect
{
    public class AdapterMenu : BaseAdapter<int>
    {
        List<int> Listitems;
        Activity context;
        bool EditStatus;
        Sidemenu mDrawerLayout;
        public AdapterMenu(Activity context, Sidemenu mDrawerLayoutv) : base()
        {
            mDrawerLayout = mDrawerLayoutv;
            this.context = context;
            Listitems = new List<int>();
            Listitems.Add(1);
        }
        public override long GetItemId(int position)
        {
            return position;
        }
        public override int this[int position]
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
            MenuViewHolder vh;
            View view = convertView;
            if (view == null)
            {
                view = context.LayoutInflater.Inflate(Resource.Layout.Menu, null);
                vh = new MenuViewHolder();
                vh.InitializeNotAccepted(view);
                view.Tag = vh;
            }
            var notaccepitem = Listitems[position];
            vh = (MenuViewHolder)view.Tag;
            vh.BindNotAccepted(context, mDrawerLayout);
            return view;
        }
    }
    public class MenuViewHolder : Activity
    {
        Activity context;
         Sidemenu mDrawerLayout;
        public void InitializeNotAccepted(View view)
        {
            TextView Changetext = view.FindViewById<TextView>(Resource.Id.Changetext);
            ImageView Changeimage = view.FindViewById<ImageView>(Resource.Id.Changeimage);
            Changetext.Click += new EventHandler(this.PressChangesiteButton);
            Changeimage.Click += new EventHandler(this.PressChangesiteButton);
            TextView Bizname = view.FindViewById<TextView>(Resource.Id.Bizname);
            TextView Bizlocation = view.FindViewById<TextView>(Resource.Id.Bizlocation);
            TextView Bizwebsite = view.FindViewById<TextView>(Resource.Id.Bizwebsite);

            TextView Pendingtext = view.FindViewById<TextView>(Resource.Id.Pendingtext);
            ImageView Pendingimage = view.FindViewById<ImageView>(Resource.Id.Pendingimage);
            Pendingtext.Click += new EventHandler(this.PressRequestsButton);
            Pendingimage.Click += new EventHandler(this.PressRequestsButton);

            TextView Faqtext = view.FindViewById<TextView>(Resource.Id.Faqtext);
            ImageView Faqimage = view.FindViewById<ImageView>(Resource.Id.Faqimage);
            Faqtext.Click += new EventHandler(this.PressFaqButton);
            Faqimage.Click += new EventHandler(this.PressFaqButton);

            TextView Privacytext = view.FindViewById<TextView>(Resource.Id.Privacytext);
            ImageView Privacyimage = view.FindViewById<ImageView>(Resource.Id.Privacyimage);
            Privacytext.Click += new EventHandler(this.PressPrivacyButton);
            Privacyimage.Click += new EventHandler(this.PressPrivacyButton);

            TextView Termstext = view.FindViewById<TextView>(Resource.Id.Termstext);
            ImageView Termsimage = view.FindViewById<ImageView>(Resource.Id.Termsimage);
            Termstext.Click += new EventHandler(this.PressTermsButton);
            Termsimage.Click += new EventHandler(this.PressTermsButton);

            TextView Signouttext = view.FindViewById<TextView>(Resource.Id.Signouttext);
            ImageView Signoutimage = view.FindViewById<ImageView>(Resource.Id.Signoutimage);
            Signouttext.Click += new EventHandler(this.PressSignoutButton);
            Signoutimage.Click += new EventHandler(this.PressSignoutButton);
        }
        //This method is used to Bind the data.
        public void BindNotAccepted(Activity myContext, Sidemenu mDrawerLayoutv)
        {
            try
            {
                mDrawerLayout = mDrawerLayoutv;
                context = myContext;
            }
            catch
            {
            }
        }
        void PressButton(Object sender, EventArgs e)
        {
        }
        void PressRequestsButton(Object sender, EventArgs e)
        {
           // mDrawerLayout.CloseDrawers();
            context.StartActivity(new Intent(context, typeof(Requests)));
            context.OverridePendingTransition(Resource.Drawable.slide_from_right, Resource.Drawable.slide_to_left);
        }
        void PressFaqButton(Object sender, EventArgs e)
        {
           // mDrawerLayout.CloseDrawers();
            context.StartActivity(new Intent(context, typeof(Faq)));
            context.OverridePendingTransition(Resource.Drawable.slide_from_right, Resource.Drawable.slide_to_left);
        }
        void PressPrivacyButton(Object sender, EventArgs e)
        {
          //  mDrawerLayout.CloseDrawers();
            context.StartActivity(new Intent(context, typeof(Privacypolicy)));
            context.OverridePendingTransition(Resource.Drawable.slide_from_right, Resource.Drawable.slide_to_left);
        }
        void PressTermsButton(Object sender, EventArgs e)
        {
          //  mDrawerLayout.CloseDrawers();
            context.StartActivity(new Intent(context, typeof(Termsandconditions)));
            context.OverridePendingTransition(Resource.Drawable.slide_from_right, Resource.Drawable.slide_to_left);
        }
        void PressChangesiteButton(Object sender, EventArgs e)
        {
          //  mDrawerLayout.CloseDrawers();
            context.StartActivity(new Intent(context, typeof(Bizoption)));
            context.OverridePendingTransition(Resource.Drawable.slide_from_right, Resource.Drawable.slide_to_left);
        }
        void PressSignoutButton(Object sendere, EventArgs ee)
        {
            var button = (sendere as View);
            mDrawerLayout.AnimatedOpened = !mDrawerLayout.AnimatedOpened;
            AlertDialog.Builder dialog = new AlertDialog.Builder(context);
            dialog.SetTitle("Sign Out");
            dialog.SetMessage("Are you sure you want to sign out?");
            dialog.SetPositiveButton("Sign Out", (sender, e) =>
            {
                AppPreferences objAppPreferences = new AppPreferences(context);
                objAppPreferences.SaveUserdetails("","");
                context.StartActivity(new Intent(context, typeof(Login)));
                context.OverridePendingTransition(Resource.Drawable.slide_from_left, Resource.Drawable.slide_to_right);
            });
            dialog.SetNegativeButton("Cancel", (sender, e) =>
            {
                dialog.Dispose();
            });

            dialog.Create().Show();
        }
    }
}