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
using Android.Webkit;
using Android.Graphics;
using Android.Speech;
using Android.Support.V4.App;
using Android;
using Android.Content.PM;
using Android.Animation;
using Android.Views.Animations;
using Android.Graphics.Drawables;
using System.Threading.Tasks;
using Android.Transitions;

namespace com.mopro.directconnect
{
    [Activity(Theme = "@style/AppThemeTransp", Label = "Voice", ScreenOrientation = ScreenOrientation.Portrait)]
    public class Voice : Activity, IRecognitionListener, ViewTreeObserver.IOnGlobalLayoutListener
    {
        int VOICE = 1004;
        TextView Speachtext;
        ImageView Callbutton;
        RelativeLayout SpeachScreen;
        LinearLayout ResultScreen;
        Button ButtonEditquestion;
        private SpeechRecognizer speech = null;
        private Intent recognizerIntent;
        public static String REVEAL_X = "REVEAL_X";
        public static String REVEAL_Y = "REVEAL_Y";
        LinearLayout rootLayout;
        private int revealX;
        private int revealY;
        WebView web_view;
        public static String Searchtextstr;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Voice);
            rootLayout = FindViewById<LinearLayout>(Resource.Id.root_layout);
            if (savedInstanceState == null && Build.VERSION.SdkInt >= Build.VERSION_CODES.Lollipop && Intent.HasExtra(REVEAL_X) && Intent.HasExtra(REVEAL_Y))
            {
                Window.SharedElementEnterTransition = null;
                Window.SharedElementReenterTransition = null;
                Window.SharedElementExitTransition = null;
                Fade fade = new Fade();
                fade.ExcludeTarget(Android.Resource.Id.StatusBarBackground, true);
                Window.ExitTransition = fade;
                Window.EnterTransition = fade;
                Window.AllowEnterTransitionOverlap = false;
                Window.AllowReturnTransitionOverlap = false;
                rootLayout.Visibility = (ViewStates.Invisible);
                revealX = Intent.GetIntExtra(REVEAL_X, 0);
                revealY = Intent.GetIntExtra(REVEAL_Y, 0);
                ViewTreeObserver viewTreeObserver = rootLayout.ViewTreeObserver;
                if (viewTreeObserver.IsAlive)
                {
                    viewTreeObserver.AddOnGlobalLayoutListener(this);
                }
            }
            else
            {
                rootLayout.Visibility = (ViewStates.Visible);
            }
            Window.SetSoftInputMode(SoftInput.AdjustResize | SoftInput.StateHidden);
            SpeachScreen = FindViewById<RelativeLayout>(Resource.Id.SpeachScreen);
            ResultScreen = FindViewById<LinearLayout>(Resource.Id.ResultScreen);
            // Click Back button Events Occurs below
            ImageView Backbutton = FindViewById<ImageView>(Resource.Id.Backbutton);
            Backbutton.Click += (o, e) =>
            {
                if (ResultScreen.Visibility == ViewStates.Visible)
                {
                    StartActivity(new Intent(this, typeof(Home)));
                    OverridePendingTransition(Resource.Drawable.slide_from_left, Resource.Drawable.slide_to_right);
                }
                else
                {
                    unRevealActivity();
                }
            };
            Callbutton = FindViewById<ImageView>(Resource.Id.Callbutton);
            ImageView Callimage = FindViewById<ImageView>(Resource.Id.Callimage);
            TextView Calltext = FindViewById<TextView>(Resource.Id.Calltext);
            Calltext.SetTypeface(AppFont.GetButton(this), TypefaceStyle.Normal);
            RelativeLayout Calllayout = FindViewById<RelativeLayout>(Resource.Id.Calllayout);
            Callbutton.Click += (o, e) => PressCallButton();
            Callimage.Click += (o, e) => PressCallButton();
            Calltext.Click += (o, e) => PressCallButton();
            Calllayout.Click += (o, e) => PressCallButton();
            ButtonEditquestion = FindViewById<Button>(Resource.Id.ButtonEditquestion);
            ButtonEditquestion.Click += (o, e) => PressEditquestionButton();
            Callbutton.Visibility = ViewStates.Gone;
            Speachtext = FindViewById<TextView>(Resource.Id.Speachtext);
            Speachtext.SetTypeface(AppFont.GetText(this), TypefaceStyle.Normal);
            TextView headertext = FindViewById<TextView>(Resource.Id.headertext);
            headertext.SetTypeface(AppFont.GetText(this), TypefaceStyle.Normal);
            TextView headertext2 = FindViewById<TextView>(Resource.Id.headertext2);
            headertext2.SetTypeface(AppFont.GetText(this), TypefaceStyle.Normal);
            TextView headertextfound = FindViewById<TextView>(Resource.Id.headertextfound);
            headertextfound.SetTypeface(AppFont.GetButton(this), TypefaceStyle.Normal);
            Button Webviewbut = FindViewById<Button>(Resource.Id.Webviewbut);
            Webviewbut.Click += (o, e) => { };
            ListView Listview = FindViewById<ListView>(Resource.Id.Listview);
            // Populate Default values for testing
            List<Bussinessclass> objBussinessdata = new List<Bussinessclass>();
            objBussinessdata.Add(new Bussinessclass() { id = "1", name = "How do I use A.I. Editor to make changes to my sites's images/photos?" });
            objBussinessdata.Add(new Bussinessclass() { id = "2", name = "Send in a Request, for us to update the image?" });
            objBussinessdata.Add(new Bussinessclass() { id = "2", name = "How to Add Photos & Video to Your Site ?" });
            objBussinessdata.Add(new Bussinessclass() { id = "2", name = "How to add a gallery Block in AI ?" });
            objBussinessdata.Add(new Bussinessclass() { id = "5", name = "Don't see what you are looking for ? \nSend us a note" });
            AdapterVoiceList objAdapterVoiceList = new AdapterVoiceList(this, objBussinessdata);
            Listview.SetAdapter(objAdapterVoiceList);
            // Create and Show Wave while speaking
            web_view = FindViewById<WebView>(Resource.Id.Webview);
            web_view.Settings.JavaScriptEnabled = true;
            web_view.Enabled = false;
            web_view.VerticalScrollBarEnabled = false;
            web_view.HorizontalScrollBarEnabled = false;
            web_view.SetScrollContainer(false);
            //web_view.SetInitialScale(1);
            //web_view.Settings.LoadWithOverviewMode = true;
            //web_view.Settings.UseWideViewPort = true;
            //web_view.Settings.DefaultZoom = WebSettings.ZoomDensity.Far;
            web_view.LoadUrl("file:///android_asset/test.html");
            web_view.SetBackgroundColor(Resources.GetColor(Resource.Color.Transparent));
            if (Build.VERSION.SdkInt >= BuildVersionCodes.IceCreamSandwich)
            {
                web_view.SetLayerType(LayerType.Software, null);
            }
            //loading xml from anim folder
            Animation localAnimation = AnimationUtils.LoadAnimation(this, Resource.Drawable.voicezoomin);
            //You can now apply the animation to a view
            RelativeLayout header = FindViewById<RelativeLayout>(Resource.Id.header);
            header.StartAnimation(localAnimation);
            Listview.ItemClick += (sender, e) =>
            {
                if (objBussinessdata[e.Position].id == "5")
                {
                    Searchtextstr = Speachtext.Text;
                    StartActivity(new Intent(this, typeof(SupportRequest)));
                    OverridePendingTransition(Resource.Drawable.slide_from_right, Resource.Drawable.slide_to_left);
                }
                else
                {
                    StartActivity(new Intent(this, typeof(CTA)));
                    OverridePendingTransition(Resource.Drawable.slide_from_right, Resource.Drawable.slide_to_left);
                }
            };
            if (Intent.GetBooleanExtra("search", false))
            {
                Speachtext.Text = Intent.GetStringExtra("searchtext");
                SpeachScreen.Visibility = ViewStates.Gone;
                ResultScreen.Visibility = ViewStates.Visible;
                Callbutton.Visibility = ViewStates.Visible;
            }
            else
            {
                if (Build.VERSION.SdkInt >= Build.VERSION_CODES.M)
                {
                    if (Android.Support.V4.Content.ContextCompat.CheckSelfPermission(this, Manifest.Permission.RecordAudio) != (int)Permission.Granted)
                    {
                        // Permission has never been accepted
                        // So, I ask the user for permission
                        ActivityCompat.RequestPermissions(this, new String[] { Manifest.Permission.RecordAudio }, 100);
                    }
                    else     // Permission has already been accepted previously
                        startVoiceRecognitionActivity();
                }
                else
                {
                    // Permission has been accepted
                    startVoiceRecognitionActivity();
                }
            }
        }
        public void PressEditquestionButton()
        {
            Intent objIntent = new Intent(this, typeof(Home));
            objIntent.PutExtra("search", true);
            objIntent.PutExtra("searchtext", Speachtext.Text);
            StartActivity(objIntent);
            OverridePendingTransition(Resource.Drawable.slide_from_left, Resource.Drawable.slide_to_right);
        }
        public void OnGlobalLayout()
        {
            revealActivity(revealX, revealY);
            rootLayout.ViewTreeObserver.RemoveOnGlobalLayoutListener(this);
        }
        protected void revealActivity(int x, int y)
        {
            EnableBorder();
            if (Build.VERSION.SdkInt >= Build.VERSION_CODES.Lollipop)
            {
                float finalRadius = (float)(Math.Max(rootLayout.Width, rootLayout.Height) * 1.1);

                // create the animator for this view (the start radius is zero)
                Animator circularReveal = ViewAnimationUtils.CreateCircularReveal(rootLayout, x, y, 0, finalRadius);
                circularReveal.SetDuration(400);
                
                circularReveal.SetInterpolator(new AccelerateInterpolator());
                circularReveal.AnimationEnd += delegate
                {
                    rootLayout.SetBackgroundDrawable(Resources.GetDrawable(Resource.Drawable.background));
                };
                // make the view visible and start the animation
                rootLayout.Visibility = (ViewStates.Visible);
                circularReveal.Start();
            }
            else
            {
                Intent objIntent = new Intent(this, typeof(Home));
                objIntent.SetFlags(ActivityFlags.NoAnimation);
                StartActivity(objIntent);
                OverridePendingTransition(0, 0);
            }
        }
        void EnableBorder()
        {
            //rootLayout.SetBackgroundDrawable(Resources.GetDrawable(Resource.Drawable.bgborder));
            ////use a GradientDrawable with only one color set, to make it a solid color
            GradientDrawable border = new GradientDrawable(GradientDrawable.Orientation.TlBr,
            new int[] { Color.ParseColor("#734aa6"),Color.ParseColor("#1a8cd5"), Color.ParseColor("#1a8cd5")  });
            border.SetStroke(10, Color.White); //black border with full opacity
            if (Build.VERSION.SdkInt < Build.VERSION_CODES.JellyBean)
            {
                rootLayout.SetBackgroundDrawable(border);
            }
            else { rootLayout.SetBackgroundDrawable(border); }
        }        
        protected void unRevealActivity()
        {
            EnableBorder();
            if (Build.VERSION.SdkInt >= Build.VERSION_CODES.Lollipop)
            {
                web_view.Visibility = ViewStates.Gone;
                float finalRadius = (float)(Math.Max(rootLayout.Width, rootLayout.Height) * 1.1);
                Animator circularReveal = ViewAnimationUtils.CreateCircularReveal(rootLayout, revealX, revealY, finalRadius, 0);
                circularReveal.SetDuration(400);
                //circularReveal.excludeTarget(android.R.id.statusBarBackground, true);
                circularReveal.AnimationEnd += delegate
                {
                    rootLayout.Visibility = ViewStates.Invisible;
                    Intent objIntent = new Intent(this, typeof(Home));
                    objIntent.SetFlags(ActivityFlags.NoAnimation);
                    StartActivity(objIntent);
                    OverridePendingTransition(0, 0);
                };
                circularReveal.Start();
            } else
            {
                Intent objIntent = new Intent(this, typeof(Home));
                objIntent.SetFlags(ActivityFlags.NoAnimation);
                StartActivity(objIntent);
                OverridePendingTransition(0, 0);
            }
        }
        private void PressCallButton()
        {
            StartActivity(new Intent(this, typeof(Callview)));
            OverridePendingTransition(Resource.Drawable.slide_zoom_in, Resource.Drawable.slide_zoom_out);
        }
        private void startVoiceRecognitionActivity()
        {
            SpeachScreen.Visibility = ViewStates.Visible;
            ResultScreen.Visibility = ViewStates.Gone;
            Callbutton.Visibility = ViewStates.Gone;
            string rec = Android.Content.PM.PackageManager.FeatureMicrophone;
            if (rec != "android.hardware.microphone")
            {
                // Give Alert to the user if the Headset Not Connect with Microphone
                var alert = new AlertDialog.Builder(this);
                alert.SetTitle("You don't seem to have a microphone to record with");
                alert.SetPositiveButton("OK", (sender, e) =>
                {
                    return;
                });
                alert.Show();
            }
            else
            {
                // create the intent and start the activity
                speech = SpeechRecognizer.CreateSpeechRecognizer(this);
                speech.SetRecognitionListener(this);
                //// put a message on the modal dialog
                recognizerIntent = new Intent(RecognizerIntent.ActionRecognizeSpeech);
                //// you can specify other languages recognised here, for example
                recognizerIntent.PutExtra(RecognizerIntent.ExtraLanguagePreference, "en");
                recognizerIntent.PutExtra(RecognizerIntent.ExtraLanguageModel, RecognizerIntent.LanguageModelFreeForm);
                recognizerIntent.PutExtra(RecognizerIntent.ExtraCallingPackage, Application.PackageName);
                recognizerIntent.PutExtra(RecognizerIntent.ExtraMaxResults, 2);
                recognizerIntent.PutExtra(RecognizerIntent.ExtraPartialResults, true);
                //// if there is more then 1.5s of silence, consider the speech over
                recognizerIntent.PutExtra(RecognizerIntent.ExtraSpeechInputCompleteSilenceLengthMillis, 1500);
                recognizerIntent.PutExtra(RecognizerIntent.ExtraSpeechInputPossiblyCompleteSilenceLengthMillis, 1500);
                recognizerIntent.PutExtra(RecognizerIntent.ExtraSpeechInputMinimumLengthMillis, 15000);
                recognizerIntent.PutExtra(RecognizerIntent.ExtraLanguage, Java.Util.Locale.Default);
                speech.StartListening(recognizerIntent);
            }
        }
        // Getting Data After the user Speech
        protected override void OnActivityResult(int requestCode, Result resultVal, Intent data)
        {
            if (requestCode == 100)
            {
                // Permission has been accepted
                startVoiceRecognitionActivity();
            }
            base.OnActivityResult(requestCode, resultVal, data);
        }
        // Click Back button Events Occurs below
        public override void OnBackPressed()
        {
           // base.OnBackPressed();
            unRevealActivity();
        }

        public void OnBeginningOfSpeech()
        {

        }

        public void OnBufferReceived(byte[] buffer)
        {

        }

        public void OnEndOfSpeech()
        {

        }

        public void OnError([GeneratedEnum] SpeechRecognizerError error)
        {
            String message;
            switch (error)
            {
                case SpeechRecognizer.ErrorAudio:
                    message = "Audio recording error";
                    break;
                case SpeechRecognizer.ErrorClient:
                    message = "Client side error";
                    break;
                case SpeechRecognizer.ErrorInsufficientPermissions:
                    message = "Insufficient permissions";
                    break;
                case SpeechRecognizer.ErrorNetwork:
                    message = "Network error";
                    break;
                case SpeechRecognizer.ErrorNetworkTimeout:
                    message = "Network timeout";
                    break;
                case SpeechRecognizer.ErrorNoMatch:
                    message = "No match";
                    break;
                case SpeechRecognizer.ErrorRecognizerBusy:
                    message = "RecognitionService busy";
                    break;
                case SpeechRecognizer.ErrorServer:
                    message = "error from server";
                    break;
                case SpeechRecognizer.ErrorSpeechTimeout:
                    message = "No speech input";
                    break;
                default:
                    message = "Didn't understand, please try again.";
                    break;
            }
            message = "Didn't understand, please try again.";
            unRevealActivity();
        }
        // Shows Alert for Inavlid User Login
        void Alertpopup(String Content)
        {
            AlertDialog.Builder alertDialog = new AlertDialog.Builder(this);
            alertDialog.SetTitle(Resources.GetString(Resource.String.error));
            alertDialog.SetMessage(Content);
            alertDialog.SetPositiveButton("Try again", delegate
            {
                alertDialog.Dispose();
                startVoiceRecognitionActivity();
            });
            alertDialog.SetNegativeButton("Cancel", delegate
            {
                alertDialog.Dispose();
                //StartActivity(new Intent(this, typeof(Home)));
            });
            alertDialog.Show();
        }
        public void OnEvent(int eventType, Bundle @params)
        {
        }

        public void OnPartialResults(Bundle partialResults)
        {
        }

        public void OnReadyForSpeech(Bundle @params)
        {
        }

        public void OnResults(Bundle results)
        {
            var matches = results.GetStringArrayList(SpeechRecognizer.ResultsRecognition);
            if (matches.Count != 0)
            {
                string textInput = matches[0];
                // limit the output to 500 characters
                if (textInput.Length > 500)
                    textInput = textInput.Substring(0, 500);
                Speachtext.Text = textInput;
                if (textInput.ToLower().Contains("hello"))
                {
                    StartActivity(new Intent(this, typeof(VoiceError)));
                    OverridePendingTransition(Resource.Drawable.slide_from_right, Resource.Drawable.slide_to_left);
                }
                else
                {
                    SpeachScreen.Visibility = ViewStates.Gone;
                    ResultScreen.Visibility = ViewStates.Visible;
                    Callbutton.Visibility = ViewStates.Visible;
                }
            }
            else
                Alertpopup("No speech was recognised");
            //List<String> matches = results.GetStringArrayList(SpeechRecognizer.ResultsRecognition).ToList();
            //String text = "";
            // foreach (String result in matches)
            //    text += result + "\n";
            speech.StopListening();
            speech.Cancel();
            speech.Destroy();
        }
        // Adapter For Populating the Website List
        public class AdapterVoiceList : BaseAdapter<Bussinessclass>
        {
            List<Bussinessclass> Listitems;
            Activity context;
            public AdapterVoiceList(Activity context, List<Bussinessclass> Listitemsv) : base()
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
                    view = context.LayoutInflater.Inflate(Resource.Layout.Voicelist, null);
                //view.FindViewById<TextView>(Resource.Id.Bizname).Text = Listitems[position].Bussiness;
                TextView Searchtext = view.FindViewById<TextView>(Resource.Id.Searchtext);
                Searchtext.SetTypeface(AppFont.GetText(context), TypefaceStyle.Normal);
                Searchtext.Text = Listitems[position].name;
                //ImageView Searchnav = view.FindViewById<ImageView>(Resource.Id.Searchnav);
                if (Listitems[position].id == "5")
                {
                    view.FindViewById<ImageView>(Resource.Id.Searchnav).Visibility = ViewStates.Gone;
                    view.FindViewById<ImageView>(Resource.Id.Helpnav).Visibility = ViewStates.Visible; 
                } else
                {
                    view.FindViewById<ImageView>(Resource.Id.Searchnav).Visibility = ViewStates.Visible;
                    view.FindViewById<ImageView>(Resource.Id.Helpnav).Visibility = ViewStates.Gone;
                   // Searchnav.SetColorFilter(context.Resources.GetColor(Resource.Color.textcolor));
                }
                return view;
            }
        }
        public void OnRmsChanged(float rmsdB)
        {
        }
        protected override void OnPause()
        {
            base.OnPause();
            if (speech != null)
            {
                speech.StopListening();
                speech.Cancel();
                //speech.Destroy();
            }
        }
    }
}