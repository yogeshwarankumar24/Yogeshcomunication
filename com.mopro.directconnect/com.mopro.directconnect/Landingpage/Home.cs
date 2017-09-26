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
using Android.Graphics;
using Android.Views.Animations;
using static Android.Views.Animations.Animation;
using Android.Animation;
using Android.Support.V4.App;
using Android.Views.InputMethods;
using Android.Content.PM;
using Android.Content.Res;
using Android.Util;
using System.Threading.Tasks;

namespace com.mopro.directconnect
{
    [Activity(Label = "Direct Connect", ScreenOrientation = ScreenOrientation.Portrait)]
    public class Home : Activity,ViewTreeObserver.IOnGlobalLayoutListener,View.IOnTouchListener
    {
        RelativeLayout IntroScreen;
        LinearLayout ContentScreen,content_frame;
       // DrawerLayout mDrawerLayout;
        EditText EditSearch;
        Button EditScreen;
        ImageView VoiceButton2;
        Sidemenu menu;
        public static Button Closemenu;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Home);
            Window.SetSoftInputMode(SoftInput.AdjustResize | SoftInput.StateUnchanged | SoftInput.StateHidden);
            IntroScreen = FindViewById<RelativeLayout>(Resource.Id.IntroScreen);
            ContentScreen = FindViewById<LinearLayout>(Resource.Id.ContentScreen);
            //Layoutbuttons = FindViewById<LinearLayout>(Resource.Id.Layoutbuttons);
            Button ButtonIntro = FindViewById<Button>(Resource.Id.ButtonIntro);
            bool Livestatus = Intent.GetBooleanExtra("status", false);
            if (Livestatus)
            {
                // Show the Intro Screens for Non-Live Clients
                IntroScreen.Visibility = ViewStates.Visible;
            }
            else
            {
                // Hide the Intro Screens for Live Clients
                IntroScreen.Visibility = ViewStates.Gone;
            }
            if (Intent.GetBooleanExtra("request", false))
                Alertpopup("Your request(s) have been submitted.");
            ButtonIntro.Click += (o, e) => PressIntroButton();
            ButtonIntro.SetTypeface(AppFont.GetButton(this), TypefaceStyle.Normal);
            ImageView Notesbutton = FindViewById<ImageView>(Resource.Id.Notesbutton);
            Notesbutton.Click += (o, e) => PressNotesButton();
            ImageView Voicebutton = FindViewById<ImageView>(Resource.Id.Voicebutton);
            VoiceButton2 = FindViewById<ImageView>(Resource.Id.VoiceButton2);
            VoiceButton2.Click += (o, e) => PresentActivity(VoiceButton2);
            Voicebutton.Click += (o, e) => PresentActivity2(Voicebutton);
            ImageView Chatbutton = FindViewById<ImageView>(Resource.Id.Chatbutton);
            Chatbutton.Click += (o, e) => PressChatButton();
            ImageView ChatButton2 = FindViewById<ImageView>(Resource.Id.ChatButton2);
            ChatButton2.Click += (o, e) => PressChatButton();
            ImageView Callbutton = FindViewById<ImageView>(Resource.Id.Callbutton);
            Callbutton.Click += (o, e) => PressCallButton();
            ImageView CallButton2 = FindViewById<ImageView>(Resource.Id.CallButton2);
            CallButton2.Click += (o, e) => PressCallButton();
            ImageView TodoButton = FindViewById<ImageView>(Resource.Id.TodoButton);
            TodoButton.Click += (o, e) => PressRequestButton();
            TextView introtxt = FindViewById<TextView>(Resource.Id.introtxt);
            ScrollView Scrollview = FindViewById<ScrollView>(Resource.Id.Scrollview);
            Scrollview.SetOnTouchListener(this);
            Scrollview.SetScrollContainer(false);
            introtxt.SetTypeface(AppFont.GetTitle(this), TypefaceStyle.Normal);
            EditSearch = FindViewById<EditText>(Resource.Id.EditSearch);
            EditScreen = FindViewById<Button>(Resource.Id.EditScreen);
            EditScreen.Visibility = ViewStates.Gone;
            EditSearch.SetTypeface(AppFont.GetText(this), TypefaceStyle.Normal);
            EditSearch.LayoutParameters = new RelativeLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, Resources.DisplayMetrics.HeightPixels);
            EditSearch.BeforeTextChanged += delegate
            {
                EditScreen.Visibility = ViewStates.Visible;
                VoiceButton2.Visibility = ViewStates.Gone;
            };
            EditSearch.RequestFocus();
            EditSearch.Click += delegate
            {
                EditScreen.Visibility = ViewStates.Visible;
                VoiceButton2.Visibility = ViewStates.Gone;
            };
            EditSearch.EditorAction += delegate
            {
                EditScreen.Visibility = ViewStates.Gone;
                VoiceButton2.Visibility = ViewStates.Visible;
                hideSoftKeyboard();
                if (!String.IsNullOrEmpty(EditSearch.Text))
                    PressSearchButton(EditSearch.Text);
            };
            EditScreen.Touch += delegate
            {
                EditScreen.Visibility = ViewStates.Gone;
                VoiceButton2.Visibility = ViewStates.Visible;
                hideSoftKeyboard();
            };
            menu = FindViewById<Sidemenu>(Resource.Id.drawer_layout);
            var menuButton = FindViewById(Resource.Id.MenuButton);
            TextView App_Version = FindViewById<TextView>(Resource.Id.App_Version);
            App_Version.Text = "App Version: v 1.2";
            ListView mDrawerList = FindViewById<ListView>(Resource.Id.list_drawer);
            LinearLayout left_drawer = FindViewById<LinearLayout>(Resource.Id.left_drawer);
            Closemenu = FindViewById<Button>(Resource.Id.Closemenu);
            mDrawerList.SetAdapter(new AdapterMenu(this, menu));
            content_frame = FindViewById<LinearLayout>(Resource.Id.content_frame);
            content_frame.ViewTreeObserver.AddOnGlobalLayoutListener(this);
            content_frame.Click += delegate { };
            IntroScreen.Click += delegate { };
            menuButton.Click += (s, e) => OpensideMenu();
            Closemenu.Click += (o, e) =>
            {
                menu.AnimatedOpened = !menu.AnimatedOpened;
            };
            if (Intent.GetBooleanExtra("search", false))
            {
                EditSearch.Text = Intent.GetStringExtra("searchtext");
                EditSearch.PerformClick();
                InputMethodManager imm = (InputMethodManager)GetSystemService(Context.InputMethodService);
                imm.ToggleSoftInput(InputMethodManager.ShowForced, 0);
            }
            StatusBarHeight();
        }
        public void PressSearchButton(String Speachtext)
        {
            if (Speachtext.ToLower().Contains("hello"))
            {
                StartActivity(new Intent(this, typeof(VoiceError)));
                OverridePendingTransition(Resource.Drawable.slide_from_right, Resource.Drawable.slide_to_left);
            }
            else
            {
                Intent objIntent = new Intent(this, typeof(Voice));
                objIntent.PutExtra("search", true);
                objIntent.PutExtra("searchtext", Speachtext);
                StartActivity(objIntent);
                OverridePendingTransition(Resource.Drawable.slide_from_right, Resource.Drawable.slide_to_left);
            }
        }
        private void StatusBarHeight()
        {
            try
            {
                if (HasNavBar() || Build.VERSION.SdkInt <= Build.VERSION_CODES.Kitkat)
                {
                    int statusBarHeight = 0;
                    //DisplayMetrics displayMetrics = new DisplayMetrics();
                   // (GetSystemService(Context.WindowService))..Metrics(displayMetrics);                   
                    switch (Resources.DisplayMetrics.DensityDpi)
                    {
                        case DisplayMetrics.DensityHigh:
                            statusBarHeight = ImageHandler.HIGH_DPI_STATUS_BAR_HEIGHT;
                            break;
                        case DisplayMetrics.DensityMedium:
                            statusBarHeight = ImageHandler.MEDIUM_DPI_STATUS_BAR_HEIGHT;
                            break;
                        case DisplayMetrics.DensityLow:
                            statusBarHeight = ImageHandler.LOW_DPI_STATUS_BAR_HEIGHT;
                            break;
                        default:
                            statusBarHeight = ImageHandler.MEDIUM_DPI_STATUS_BAR_HEIGHT;
                            break;
                    }
                    int resourceId = Resources.GetIdentifier("status_bar_height", "dimen", "android");
                    if (resourceId > 0)
                    {
                        //statusBarHeight = Resources.GetDimensionPixelSize(resourceId);
                        LinearLayout VoiceLayout = FindViewById<LinearLayout>(Resource.Id.VoiceLayout);
                        LinearLayout NotesLayout = FindViewById<LinearLayout>(Resource.Id.NotesLayout);
                        LinearLayout ChatLayout = FindViewById<LinearLayout>(Resource.Id.ChatLayout);
                        int screenheight = (Resources.DisplayMetrics.HeightPixels - AppValidation.ToPixels(this, 110 + statusBarHeight)) / 3;
                        VoiceLayout.LayoutParameters = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, screenheight);
                        NotesLayout.LayoutParameters = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, screenheight);
                        ChatLayout.LayoutParameters = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, screenheight);
                    }
                }
            }
            catch { }
        }
        public bool HasNavBar()
        {
            int resourceId = Resources.GetIdentifier("config_showNavigationBar", "bool", "android");
            return resourceId > 0 && Resources.GetBoolean(resourceId);
        }
        // Connect to Mo pro for Non-Live Clients
        private void PressIntroButton()
        {
        }
        // Connect to Mo pro for Non-Live Clients
        private void PressRequestButton()
        {
            StartActivity(new Intent(this, typeof(Requests)));
            OverridePendingTransition(Resource.Drawable.slide_from_right, Resource.Drawable.slide_to_left);
        }
        // Click Notes button Events Occurs below and Redirect to the Screen
        private void PressNotesButton()
        {
            StartActivity(new Intent(this, typeof(Website)));
            OverridePendingTransition(Resource.Drawable.slide_from_right, Resource.Drawable.slide_to_left);
        }
        public void PresentActivity(View view)
        {
            ActivityOptionsCompat options = ActivityOptionsCompat.MakeSceneTransitionAnimation(this, view, "transition");
            int revealX = (int)(view.GetX() + 50 + (view.Width / 2));
            int revealY = 250;
            Intent intent = new Intent(this, typeof(Voice));
            intent.PutExtra(Voice.REVEAL_X, revealX);
            intent.PutExtra(Voice.REVEAL_Y, revealY);
            ActivityCompat.StartActivity(this, intent, options.ToBundle());
        }
        public void PresentActivity2(View view)
        {
            ActivityOptionsCompat options = ActivityOptionsCompat.MakeSceneTransitionAnimation(this, view, "transition");
            int revealX = (Resources.DisplayMetrics.WidthPixels/4);
            int revealY = (Resources.DisplayMetrics.HeightPixels - 200);
            Intent intent = new Intent(this, typeof(Voice));
            intent.PutExtra(Voice.REVEAL_X, revealX);
            intent.PutExtra(Voice.REVEAL_Y, revealY);
            ActivityCompat.StartActivity(this, intent, options.ToBundle());
        }
        // Click Chat button Events Occurs below and Redirect to the Screen
        private void PressChatButton()
        {
            StartActivity(new Intent(this, typeof(Chatting)));
            OverridePendingTransition(Resource.Drawable.slide_from_right, Resource.Drawable.slide_to_left);
        }
        // Click Chat button Events Occurs below and Redirect to the Screen
        private void PressCallButton()
        {
            StartActivity(new Intent(this, typeof(Callview)));
            OverridePendingTransition(Resource.Drawable.slide_zoom_in, Resource.Drawable.slide_zoom_out);
        }
        // Click Back button Events Occurs below
        public override void OnBackPressed()
        {
            base.OnBackPressed();
            if(Intent.GetBooleanExtra("status", false))
                FinishAffinity();
            else
                StartActivity(new Intent(this, typeof(Bizoption)));
            OverridePendingTransition(Resource.Drawable.slide_from_right, Resource.Drawable.slide_to_left);
        }

        // Shows Alert for Inavlid User Login
        void Alertpopup(String Content)
        {
            AlertDialog.Builder alertDialog = new AlertDialog.Builder(this);
            alertDialog.SetTitle("Thank you!");
            alertDialog.SetMessage(Content);
            alertDialog.SetPositiveButton("OK", delegate
            {
                alertDialog.Dispose();
            });
            alertDialog.Show();
        }
        // Method used to hide keyboard when not in use
        public async void OpensideMenu()
        {
            if (CurrentFocus != null)
            {
                InputMethodManager inputMethodManager = (InputMethodManager)GetSystemService(Context.InputMethodService);
                inputMethodManager.HideSoftInputFromWindow(CurrentFocus.WindowToken, 0);
            }
            await Task.Delay(100);
            menu.AnimatedOpened = !menu.AnimatedOpened;
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
        private int mPreviousHeight;
        public void OnGlobalLayout()
        {
            int newHeight = content_frame.Height;
            if (mPreviousHeight != 0)
            {
                if (mPreviousHeight < newHeight)
                {
                    EditScreen.Visibility = ViewStates.Gone;
                    VoiceButton2.Visibility = ViewStates.Visible;
                    hideSoftKeyboard();
                }
            }
            mPreviousHeight = newHeight;
        }

        public bool OnTouch(View v, MotionEvent e)
        {
            return true;
        }
    }
}