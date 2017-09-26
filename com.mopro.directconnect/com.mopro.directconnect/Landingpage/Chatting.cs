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
using Android.Content.PM;
using Android.Graphics;

namespace com.mopro.directconnect
{
    [Activity(Label = "Chatting", ScreenOrientation = ScreenOrientation.Portrait)]
    public class Chatting : Activity
    {
        AdapterChatting objAdapterChatting;
        List<Chatclass> objBussinessdata;
        ListView Biz_ListView;
        EditText EditChat;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Chatting);
            Window.SetSoftInputMode(SoftInput.AdjustResize | SoftInput.StateHidden);
            // Click Back button Events Occurs below
            ImageView Backbutton = FindViewById<ImageView>(Resource.Id.Backbutton);
            Backbutton.Click += (o, e) =>
            {
                StartActivity(new Intent(this, typeof(Home)));
                OverridePendingTransition(Resource.Drawable.slide_from_left, Resource.Drawable.slide_to_right);
            };
            TextView headingtext = FindViewById<TextView>(Resource.Id.headingtext);
            headingtext.SetTypeface(AppFont.GetButton(this), TypefaceStyle.Normal);
            TextView headingtime = FindViewById<TextView>(Resource.Id.headingtime);
            headingtime.SetTypeface(AppFont.GetText(this), TypefaceStyle.Normal);
            headingtime.Text = DateTime.Now.ToString("MMM d, yyyy");
            ImageView Callbutton = FindViewById<ImageView>(Resource.Id.Callbutton);
            Callbutton.Click += (o, e) => PressCallButton();
            Biz_ListView = FindViewById<ListView>(Resource.Id.Listview);
            // Populate Default values for testing
            objBussinessdata = new List<Chatclass>();
            objBussinessdata.Add(new Chatclass() { id = "1", text="Hi, how can we help you?", isoutgoing = false, time = DateTime.Now.ToString("h:mm:ss tt") });
            objBussinessdata.Add(new Chatclass() { id = "2", text = "Hi, i would like to change the header image in my website", isoutgoing = true, time = DateTime.Now.ToString("h:mm:ss tt") });
            objAdapterChatting = new AdapterChatting(this, objBussinessdata);
            Biz_ListView.SetAdapter(objAdapterChatting);

            EditChat = FindViewById<EditText>(Resource.Id.EditChat);
            EditChat.SetImeActionLabel("Send", ImeAction.Done);
            EditChat.EditorAction += (sender, e) => PressSendButton(e);
        }
        private void PressSendButton(TextView.EditorActionEventArgs e)
        {
            if (e.ActionId == ImeAction.Done)
            {
                if (!String.IsNullOrEmpty(EditChat.Text) && !String.IsNullOrWhiteSpace(EditChat.Text))
                {
                    objBussinessdata.Add(new Chatclass() { id = objBussinessdata.Count.ToString(), text = EditChat.Text, isoutgoing = true, time = DateTime.Now.ToString("h:mm:ss tt") });
                    objAdapterChatting.NotifyDataSetChanged();
                    Biz_ListView.SmoothScrollToPosition(objBussinessdata.Count - 1);
                    EditChat.Text = "";
                }
                e.Handled = true;
            }
            else
            {
                e.Handled = false;
            }
        }
        private void PressCallButton()
        {
            StartActivity(new Intent(this, typeof(Callview)));
            OverridePendingTransition(Resource.Drawable.slide_zoom_in, Resource.Drawable.slide_zoom_out);
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
        // Shows Alert for Inavlid User Login
        void Alertpopup(String Content)
        {
            AlertDialog.Builder alertDialog = new AlertDialog.Builder(this);
            alertDialog.SetTitle("Sorry!");
            alertDialog.SetMessage(Content);
            alertDialog.SetPositiveButton("Try again", delegate
            {
                alertDialog.Dispose();
            });
            alertDialog.SetNegativeButton("Cancel", delegate
            {
                alertDialog.Dispose();
            });
            alertDialog.Show();
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
    public class AdapterChatting : BaseAdapter<Chatclass>
    {
        List<Chatclass> Listitems;
        Activity context;
        public AdapterChatting(Activity context, List<Chatclass> Listitemsv) : base()
        {
            this.context = context;
            this.Listitems = Listitemsv;
        }
        // Assign position for every item
        public override long GetItemId(int position)
        {
            return position;
        }
        public override Chatclass this[int position]
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
            ChatViewHolder vh;
            View view = convertView;
            if (view == null)
            {
                view = context.LayoutInflater.Inflate(Resource.Layout.Chattinglist, null);
                vh = new ChatViewHolder();
                vh.InitializeNotAccepted(view);
                view.Tag = vh;
            }
            var notaccepitem = Listitems[position];
            vh = (ChatViewHolder)view.Tag;
            vh.BindNotAccepted(context, Listitems[position]);
            return view;
        }
    }
    public class ChatViewHolder : Activity
    {
        Activity context;
        RelativeLayout IncomingChatlayout, OutgoingChatlayout;
        TextView OutgoingChat, IncomingChat, OutgoingChatTime, IncomingChatTime;
        public void InitializeNotAccepted(View view)
        {

            IncomingChatlayout = view.FindViewById<RelativeLayout>(Resource.Id.IncomingChatlayout);
            OutgoingChat = view.FindViewById<TextView>(Resource.Id.OutgoingChat);
            IncomingChat = view.FindViewById<TextView>(Resource.Id.IncomingChat);
            OutgoingChatTime = view.FindViewById<TextView>(Resource.Id.OutgoingChatTime);
            IncomingChatTime = view.FindViewById<TextView>(Resource.Id.IncomingChatTime);
            OutgoingChatlayout = view.FindViewById<RelativeLayout>(Resource.Id.OutgoingChatlayout);
        }
        //This method is used to Bind the data.
        public void BindNotAccepted(Activity myContext, Chatclass Listitems)
        {
            try
            {
                context = myContext;
                IncomingChat.SetTypeface(AppFont.GetText(context), TypefaceStyle.Normal);
                IncomingChatTime.SetTypeface(AppFont.GetText(context), TypefaceStyle.Normal);
                OutgoingChat.SetTypeface(AppFont.GetText(context), TypefaceStyle.Normal);
                OutgoingChatTime.SetTypeface(AppFont.GetText(context), TypefaceStyle.Normal);
                if (Listitems.isoutgoing)
                {
                    OutgoingChat.Text = Listitems.text;
                    OutgoingChatTime.Text = Listitems.time;
                    IncomingChatlayout.Visibility = ViewStates.Gone;
                    OutgoingChatlayout.Visibility = ViewStates.Visible;
                }
                else
                {
                    IncomingChat.Text = Listitems.text;
                    IncomingChatTime.Text = Listitems.time;
                    OutgoingChatlayout.Visibility = ViewStates.Gone;
                    IncomingChatlayout.Visibility = ViewStates.Visible;
                }
            }
            catch
            {
            }
        }        
    }
}

//View view = convertView;
//if (view == null)
//    view = context.LayoutInflater.Inflate(Resource.Layout.Chattinglist, null);
//if(Listitems[position].isoutgoing)
//{
//    view.FindViewById<RelativeLayout>(Resource.Id.IncomingChatlayout).Visibility = ViewStates.Gone;
//    view.FindViewById<TextView>(Resource.Id.OutgoingChat).Text = Listitems[position].text;
//}
//else
//{
//    view.FindViewById<TextView>(Resource.Id.IncomingChat).Text = Listitems[position].text;
//    view.FindViewById<RelativeLayout>(Resource.Id.OutgoingChatlayout).Visibility = ViewStates.Gone;
//}
//return view;