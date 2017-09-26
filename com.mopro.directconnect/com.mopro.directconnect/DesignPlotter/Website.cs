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
using Android.Util;
using Android.Views.Animations;
using System.Threading.Tasks;
using Android.Graphics;
using Android.Content.PM;
using System.IO;
using Java.Interop;
using Android.Support.V4.View;

namespace com.mopro.directconnect
{
    [Activity(Label = "Direct Connect", ScreenOrientation = ScreenOrientation.Portrait)]
    public class Website : Activity, View.IOnTouchListener,ScaleGestureDetector.IOnScaleGestureListener, ObservableWebView.OnScrollChangeListener, OnScaleChangeListener
    {
        private enum Mode
        {
            NONE,
            DRAG,
            ZOOM
        }
        int WebviewScrollx, WebviewScrollY;
        private Mode mode = Mode.NONE;
        bool ClickScale, Startscale;
        private float StartScale,ImageScale, ClickScalev, oldscale;
        public static String ScreenName;
        private int CLICK_ACTION_THRESHOLD = 130;
        private long lastTouchDown;
        int SelectedX, SelectedY, PerSelectedX, PerSelectedY;
        int TouchedX, TouchedY, TouchedXx, TouchedYy, CurrentWidth, CurrentHeight;
        public static List<Buttonclass> ObjButtonlist = new List<Buttonclass>();
        List<RelativeLayout> objLayoutlist = new List<RelativeLayout>();
        Button Submitbutton;
        ObservableWebView objwebview;
        TextView headingtext;
        ScaleGestureDetector GestureDetector;
        RelativeLayout Scrollviewlayout, Submitlayout;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Website);
            Window.SetSoftInputMode(SoftInput.StateHidden);
            headingtext = FindViewById<TextView>(Resource.Id.headingtext);
            TextView headingtext2 = FindViewById<TextView>(Resource.Id.headingtext2);
            headingtext.SetTypeface(AppFont.GetTitle(this), Android.Graphics.TypefaceStyle.Normal);
            headingtext2.SetTypeface(AppFont.GetText(this), Android.Graphics.TypefaceStyle.Normal);
            // Click Back button Events Occurs below
            ImageView Backbutton = FindViewById<ImageView>(Resource.Id.Backbutton);
            Backbutton.Click += (o, e) => PressBackButton();
            LoadPlotterDesign();
            objwebview = FindViewById<ObservableWebView>(Resource.Id.webview);
            Submitlayout = FindViewById<RelativeLayout>(Resource.Id.Submitlayout);
            Submitbutton = FindViewById<Button>(Resource.Id.Submitbutton);
            Submitbutton.SetTypeface(AppFont.GetButton(this), TypefaceStyle.Normal);
            Scrollviewlayout = FindViewById<RelativeLayout>(Resource.Id.Scrollviewlayout);
            // Scrollviewlayout.ViewTreeObserver.AddOnScrollChangedListener(this);
            CookieManager cm = CookieManager.Instance;
            cm.SetAcceptCookie(true);
            objwebview.Settings.JavaScriptEnabled = true;
            objwebview.Settings.BuiltInZoomControls = true;
            objwebview.Settings.DisplayZoomControls = false;
            objwebview.Settings.SetSupportZoom(true);
            objwebview.SetInitialScale(3);
            objwebview.Settings.LoadWithOverviewMode = true;
            objwebview.Settings.UseWideViewPort = true;
            objwebview.Settings.DefaultZoom = WebSettings.ZoomDensity.Far;
            String Htmldata = "";
            using (StreamReader sr = new StreamReader(Assets.Open("Designplotter.html")))
            {
                Htmldata = sr.ReadToEnd();
            }
            Htmldata = Htmldata.Replace("$$image$$", "webpage.jpg");
            objwebview.LoadDataWithBaseURL("file:///android_asset/", Htmldata, "text/html", "utf-8", null);
            //objwebview.LoadData(Htmldata, "text/html", "utf-8");
            objwebview.SetBackgroundColor(Color.White);
            objwebview.ScrollBarStyle = ScrollbarStyles.InsideOverlay;
            objwebview.setOnScrollChangeListener(this);
            objwebview.SetWebViewClient(new Webviewclient(this));
            objwebview.AddJavascriptInterface(this, "app");
            Submitlayout.Visibility = ViewStates.Gone;
            GestureDetector = new ScaleGestureDetector(this, this);
            objwebview.SetOnTouchListener(this);
            //Scrollviewlayout.SetBackgroundColor(Color.ParseColor("#50FFFFFF"));
            RelativeLayout headingview2 = FindViewById<RelativeLayout>(Resource.Id.headingview2);
            Submitbutton.Click += (o, e) => PressSubmitButton();
            headingview2.Click += (o, e) => PressHeaderButton();
            if (Build.VERSION.SdkInt < Build.VERSION_CODES.Kitkat)
            {
                LoadIntentdatas();
            }
        }
        [JavascriptInterface]
        [Export]
        // to become consistent with Java/JS interop convention, the argument cannot be System.String.
        public void size(int width, int height)
        {
            this.RunOnUiThread(() => {CurrentWidth = width;CurrentHeight = height; });
        }
        public class Webviewclient : WebViewClient
        {
            public Activity mActivity;
            private OnScaleChangeListener OnScaleChangeListener;
            public Webviewclient(Activity mActivity)
            {
                this.mActivity = mActivity;
                this.OnScaleChangeListener = ((OnScaleChangeListener)mActivity);
            }
            public override void OnScaleChanged(WebView view, float oldScale, float newScale)
            {
                base.OnScaleChanged(view, oldScale, newScale);
                OnScaleChangeListener.onScaleChange(view, oldScale, newScale);
            }            
            public override void OnPageStarted(WebView view, string url, Bitmap favicon)
            {
                base.OnPageStarted(view, url, favicon);
            }
            public override void OnPageFinished(WebView view, string url)
            {
                //view.ClearHistory();
                //view.ClearCache(true);
                base.OnPageFinished(view, url);
                OnScaleChangeListener.onLoadfinished(view);
            }
            public override bool ShouldOverrideUrlLoading(WebView view, string url)
            {
                return true;
            }
        }
        private async void LoadIntentdatas()
        {
            WebView.LayoutParams objlayout = new AbsoluteLayout.LayoutParams(Resources.DisplayMetrics.WidthPixels + 10000, Resources.DisplayMetrics.HeightPixels + 100000, 0, 0);
            Scrollviewlayout.LayoutParameters = objlayout;
            bool Livestatus = Intent.GetBooleanExtra("status", false);
            //Log.Debug("Livestatus", "Livestatus: " + Livestatus.ToString());
            if (Livestatus && ObjButtonlist.Count > 0 && ObjButtonlist.Count(s => s.status == true) > 0)
            {
                headingtext.Text = ScreenName;
                List<Buttonclass> ObjButtonlisttemp = new List<Buttonclass>();
                ObjButtonlisttemp = ObjButtonlist;
                foreach (var values in ObjButtonlist.Where(a => a.status == false).ToList())
                    ObjButtonlisttemp.Remove(values);
                ObjButtonlist = ObjButtonlisttemp;
                foreach (var values in ObjButtonlist.Where(a => a.status == true).ToList())
                    AddContent(values.id, values.name, values.SelectedX, values.SelectedY, false);
                Submitlayout.Visibility = ViewStates.Visible;
                RelativeLayout.LayoutParams layoutParams = new RelativeLayout.LayoutParams(objwebview.Width, objwebview.Height);
                layoutParams.BottomMargin = AppValidation.ToPixels(this, 72);
                objwebview.LayoutParameters = layoutParams;
                Submitbutton.Text = "Submit " + ObjButtonlist.Count(a => a.status == true).ToString() + " request(s)";
            }
            else
            {
                Submitlayout.Visibility = ViewStates.Gone;
                ObjButtonlist = new List<Buttonclass>();
            }

            bool Menustatus = Intent.GetBooleanExtra("menu", false);
            if (Menustatus)
            {
                headingtext.Text = Intent.GetStringExtra("menuname").ToUpper();
            }
            ScreenName = headingtext.Text;
        }
        // Methods call to Show and hide the content screen and Page screen on Click
        private void PressFlatButton(int Value)
        {
            AddContent(Value, (ObjButtonlist.Count(s => s.status == true) + 1).ToString(), SelectedX, SelectedY, true);
            CloseFlatoptions();
        }
        // Methods call to Show and hide the content screen and Page screen on Click
        private void PressFlatButton() { }
        private void PressBackButton()
        {
            StartActivity(new Intent(this, typeof(Home)));
            OverridePendingTransition(Resource.Drawable.slide_from_left, Resource.Drawable.slide_to_right);
        }
        private void CloseFlatoptions()
        {
            menuLeftCenter.Visibility = ViewStates.Gone;
            menuRightCenter.Visibility = ViewStates.Gone;
            menuTopCenter.Visibility = ViewStates.Gone;
            menuTopLeft.Visibility = ViewStates.Gone;
            menuTopRight.Visibility = ViewStates.Gone;
            menuBottomLeft.Visibility = ViewStates.Gone;
            menuBottomRight.Visibility = ViewStates.Gone;
            menuBottomCenter.Visibility = ViewStates.Gone;
        }        
        private void PressSubmitButton()
        {
            Intent objIntent = new Intent(this, typeof(Home));
            objIntent.PutExtra("request", true);
            StartActivity(objIntent);
            OverridePendingTransition(Resource.Drawable.slide_from_left, Resource.Drawable.slide_to_right);
        }
        private async void AddContent(int Value, String Textname, int XLeftMargin, int XTopMargin, bool Update)
        {
            try
            {
                RelativeLayout NewRelativeLayout = new RelativeLayout(this);
                RelativeLayout.LayoutParams RelativelayoutParams = new RelativeLayout.LayoutParams(AppValidation.ToPixels(this,70), AppValidation.ToPixels(this, 70));
                RelativelayoutParams.LeftMargin = XLeftMargin - AppValidation.ToPixels(this, 30);
                RelativelayoutParams.TopMargin = XTopMargin - AppValidation.ToPixels(this, 30); 
                ImageView NewImageView = new ImageView(this);
                RelativeLayout.LayoutParams ImageViewlayoutParams = new RelativeLayout.LayoutParams(AppValidation.ToPixels(this, 25), AppValidation.ToPixels(this, 25));
                ImageViewlayoutParams.LeftMargin = AppValidation.ToPixels(this, 5);
                ImageViewlayoutParams.TopMargin = AppValidation.ToPixels(this, 5);
                NewImageView.SetBackgroundResource(Resource.Drawable.Closeflat);
                NewImageView.Tag = Textname;
                NewImageView.Click += NewImageView_Click;
                ImageView NewImageView2 = new ImageView(this);
                RelativeLayout.LayoutParams ImageViewlayoutParams2 = new RelativeLayout.LayoutParams(AppValidation.ToPixels(this, 30), AppValidation.ToPixels(this, 30));
                ImageViewlayoutParams2.LeftMargin = AppValidation.ToPixels(this, 38);
                ImageViewlayoutParams2.TopMargin = AppValidation.ToPixels(this, 38);
                switch (Value)
                {
                    case 1:
                        {
                            NewImageView2.SetBackgroundResource(Resource.Drawable.Imageflat);
                            break;
                        }
                    case 2:
                        {
                            NewImageView2.SetBackgroundResource(Resource.Drawable.Textflat);
                            break;
                        }
                    case 3:
                        {
                            NewImageView2.SetBackgroundResource(Resource.Drawable.Videoflat);
                            break;
                        }
                    case 4:
                        {
                            NewImageView2.SetBackgroundResource(Resource.Drawable.Linkflat);
                            break;
                        }
                    case 5:
                        {
                            NewImageView2.SetBackgroundResource(Resource.Drawable.Designflat);
                            break;
                        }
                }
                TextView NewTextView = new TextView(this);
                RelativeLayout.LayoutParams layoutParams = new RelativeLayout.LayoutParams(AppValidation.ToPixels(this, 50), AppValidation.ToPixels(this, 50));
                layoutParams.AddRule(LayoutRules.CenterInParent);
                NewTextView.Text = Textname;
                NewTextView.SetTextColor(Resources.GetColor(Resource.Color.white));
                NewTextView.TextSize = 20;
                NewTextView.Tag = Textname;
                NewTextView.Gravity = GravityFlags.Center;
                NewTextView.SetBackgroundResource(Resource.Drawable.Buttonflat);
                NewTextView.Click += delegate { };
                NewRelativeLayout.Click += delegate { };
                NewRelativeLayout.AddView(NewTextView, layoutParams);
                NewRelativeLayout.AddView(NewImageView, ImageViewlayoutParams);
                NewRelativeLayout.AddView(NewImageView2, ImageViewlayoutParams2);
                Scrollviewlayout.AddView(NewRelativeLayout, RelativelayoutParams);
                Scrollviewlayout.BringChildToFront(NewRelativeLayout);
                Scrollviewlayout.BringChildToFront(menuBottomCenter);
                Scrollviewlayout.BringChildToFront(menuBottomLeft);
                Scrollviewlayout.BringChildToFront(menuBottomRight);
                Scrollviewlayout.BringChildToFront(menuTopCenter);
                Scrollviewlayout.BringChildToFront(menuTopLeft);
                Scrollviewlayout.BringChildToFront(menuTopRight);
                Scrollviewlayout.BringChildToFront(menuLeftCenter);
                Scrollviewlayout.BringChildToFront(menuRightCenter);
                objLayoutlist.Add(NewRelativeLayout);
                if (Update)
                {
                    if (ClickScale)
                    {
                        Matrix matrix = objwebview.Matrix;
                        float[] pts = { 0, 0 };
                        matrix.MapPoints(pts);
                        XLeftMargin = (int)((TouchedX * (1 / ClickScalev)) - pts[0]);
                        XTopMargin = (int)((TouchedY * (1 / ClickScalev)) - pts[1]);
                    }
                    ObjButtonlist.Add(new Buttonclass() { id = Value, name = Textname, SelectedX = XLeftMargin, SelectedY = XTopMargin, status = false });
                    // Splash Screen is visible upto 0.5 secounds
                    await Task.Delay(300);
                    switch (Value)
                    {
                        case 1:
                            {
                                StartActivity(new Intent(this, typeof(Imageupdates)));
                                OverridePendingTransition(Resource.Drawable.slide_from_right, Resource.Drawable.slide_to_left);
                                break;
                            }
                        case 2:
                            {
                                StartActivity(new Intent(this, typeof(TextRevision)));
                                OverridePendingTransition(Resource.Drawable.slide_from_right, Resource.Drawable.slide_to_left);
                                break;
                            }
                        case 3:
                            {
                                StartActivity(new Intent(this, typeof(Videoupdates)));
                                OverridePendingTransition(Resource.Drawable.slide_from_right, Resource.Drawable.slide_to_left);
                                break;
                            }
                        case 4:
                            {
                                StartActivity(new Intent(this, typeof(Linkchanges)));
                                OverridePendingTransition(Resource.Drawable.slide_from_right, Resource.Drawable.slide_to_left);
                                break;
                            }
                        case 5:
                            {
                                StartActivity(new Intent(this, typeof(Designchanges)));
                                OverridePendingTransition(Resource.Drawable.slide_from_right, Resource.Drawable.slide_to_left);
                                break;
                            }
                    }
                }
            }
            catch (Exception ex) { }
        }
        private void NewImageView_Click(object send, EventArgs eve)
        {
            ImageView objimage = (send as ImageView);
            AlertDialog.Builder dialog = new AlertDialog.Builder(this);
            dialog.SetTitle("Delete Request!");
            dialog.SetMessage("Are you sure you want to delete this request?");
            dialog.SetPositiveButton("Delete", (sender, e) =>
            {
                foreach (var Layout in objLayoutlist)
                    Layout.Visibility = ViewStates.Gone;
                ObjButtonlist.Remove(ObjButtonlist.FirstOrDefault(d => d.name == objimage.Tag.ToString()));
                int i = 1;
                foreach (var values in ObjButtonlist.Where(a => a.status == true).ToList())
                {
                    ObjButtonlist.FirstOrDefault(a => a.name == values.name).name = i.ToString();
                    AddContent(values.id, i.ToString(), values.SelectedX, values.SelectedY, false);
                    i++;
                }
                Submitlayout.Visibility = ObjButtonlist.Count(a => a.status == true) == 0 ? ViewStates.Gone : ViewStates.Visible;
                Submitbutton.Text = "Submit " + ObjButtonlist.Count(a => a.status == true).ToString() + " request(s)";
                Textmenu1.Text = (ObjButtonlist.Count(s => s.status == true) + 1).ToString();
                Textmenu2.Text = (ObjButtonlist.Count(s => s.status == true) + 1).ToString();
            });
            dialog.SetNegativeButton("Cancel", (sender, e) =>
            {
                dialog.Dispose();
            });
            dialog.Create().Show();
            objimage.Tag.ToString();
        }
        // Methods call to Show and hide the content screen and Page screen on Click
        private void PressHeaderButton()
        {
            if (menuLeftCenter.Visibility == ViewStates.Visible || menuRightCenter.Visibility == ViewStates.Visible || menuTopCenter.Visibility == ViewStates.Visible || menuTopLeft.Visibility == ViewStates.Visible || menuTopRight.Visibility == ViewStates.Visible || menuBottomLeft.Visibility == ViewStates.Visible || menuBottomRight.Visibility == ViewStates.Visible || menuBottomCenter.Visibility == ViewStates.Visible || ObjButtonlist.Count(a => a.status == true) > 0)
            {
                AlertDialog.Builder alertDialog = new AlertDialog.Builder(this);
                alertDialog.SetTitle("Confirmation");
                alertDialog.SetMessage("Are you sure to leave without submitting the ticket(s)?");
                alertDialog.SetPositiveButton("YES", (sender, e) =>
                {
                    Intent objIntent = new Intent(this, typeof(Websitemenu));
                    objIntent.PutExtra("menuname", headingtext.Text.ToLower());
                    StartActivity(objIntent);
                    OverridePendingTransition(Resource.Drawable.slide_in_bottom, Resource.Drawable.slide_out_bottom);
                });
                alertDialog.SetNegativeButton("NO", (sender, e) =>
                {
                    alertDialog.Dispose();
                });
                alertDialog.Show();
            }
            else
            {
                Intent objIntent = new Intent(this, typeof(Websitemenu));
                objIntent.PutExtra("menuname", headingtext.Text.ToLower());
                StartActivity(objIntent);
                OverridePendingTransition(Resource.Drawable.slide_in_bottom, Resource.Drawable.slide_out_bottom);
            }
        }
        // Click Back button Events Occurs below
        public override void OnBackPressed()
        {
            base.OnBackPressed();
            PressBackButton();
        }
        long time()
        {
            TimeSpan ts = (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc));
            return(long)ts.TotalMilliseconds;
        }
        public bool OnTouch(View v, MotionEvent e)
        {
            if(!Startscale)
            {
                Startscale = true;
                StartScale = objwebview.Scale;
            }
            //Log.Debug("onScale Change", "ImageScale: " + objwebview.Scale);
            GestureDetector.OnTouchEvent(e);
            mode = Mode.NONE;
            switch (e.Action & MotionEventActions.Mask)
            {
                case MotionEventActions.Down:
                    mode = Mode.NONE;
                    break;
                case MotionEventActions.Up:
                    mode = Mode.NONE;
                    break;
                case MotionEventActions.Move:
                    mode = Mode.DRAG;
                    break;
            }
            if (e.PointerCount == 1 && mode != Mode.DRAG && mode != Mode.ZOOM)
            {
                switch (e.Action & MotionEventActions.Mask)
                {
                    case MotionEventActions.Down:
                        lastTouchDown = time();
                        break;
                    case MotionEventActions.Up:
                        Log.Debug("OnTouch", "Time: " + (time() - lastTouchDown).ToString());
                        if (50 < time() - lastTouchDown && time() - lastTouchDown < CLICK_ACTION_THRESHOLD)
                        {
                            TouchedX = (int)Math.Ceiling(e.GetX()) + WebviewScrollx;
                            TouchedY = (int)Math.Ceiling(e.GetY()) + WebviewScrollY;
                            TouchedXx = (int)Math.Ceiling(e.GetX());
                            TouchedYy = (int)Math.Ceiling(e.GetY());
                            ClickScale = ImageScale<=1?false:true;
                            ClickScalev = ImageScale;
                            Matrix m = new Matrix();
                            objwebview.Matrix.Invert(m);
                            float[] pts = { e.GetX(), e.GetY() };
                            m.MapPoints(pts);
                            //Log.Debug("OnTouch", "LeftMargin: " + TouchedX + ", TopMargin " + TouchedY + " ImageScale = " + ImageScale);
                            OpenFalticons(TouchedXx, TouchedYy, TouchedX, TouchedY, true);
                        }
                        break;
                }
            }
            return base.OnTouchEvent(e);
        }
        public void CheckScale()
        {
            if (ImageScale != 0)
            {
                Matrix matrix = objwebview.Matrix;
                float[] pts = { 0, 0 };
                matrix.MapPoints(pts);
                TouchedX = (int)((TouchedX * ImageScale) + pts[0]);
                TouchedY = (int)((TouchedY * ImageScale) + pts[1]);
            }
        }
        public void OpenFalticons(int TouchedLeft, int TouchedTop, int XLeftMargin, int XTopMargin, bool animation)
        {
            SelectedY = XTopMargin;
            SelectedX = XLeftMargin;
            if (TouchedTop < AppValidation.ToPixels(this, 120))
            {
                if (TouchedLeft < AppValidation.ToPixels(this, 100))
                {
                    Textmenu6.Text = (ObjButtonlist.Count(s => s.status == true) + 1).ToString();
                    RelativeLayout.LayoutParams layoutParams = new RelativeLayout.LayoutParams(menuTopLeft.Width, menuTopLeft.Height);
                    //SelectedY = (XTopMargin - 50);
                    //SelectedX = (XLeftMargin - 75);
                    PerSelectedY = (XTopMargin - AppValidation.ToPixels(this, 28));
                    PerSelectedX = (XLeftMargin - AppValidation.ToPixels(this, 28));
                    layoutParams.TopMargin = PerSelectedY;
                    layoutParams.LeftMargin = PerSelectedX;
                    menuTopLeft.LayoutParameters = layoutParams;
                    //Log.Debug("menuTopLeft", "Width: " + menuTopLeft.Width + ", Height " + menuTopLeft.Height);
                    menuLeftCenter.Visibility = ViewStates.Gone;
                    menuRightCenter.Visibility = ViewStates.Gone;
                    menuTopCenter.Visibility = ViewStates.Gone;
                    menuTopLeft.Visibility = ViewStates.Visible;
                    menuTopRight.Visibility = ViewStates.Gone;
                    menuBottomLeft.Visibility = ViewStates.Gone;
                    menuBottomRight.Visibility = ViewStates.Gone;
                    menuBottomCenter.Visibility = ViewStates.Gone;
                    if (animation)
                    {
                        Linearlayout26.StartAnimation(AnimationUtils1);
                        Linearlayout27.StartAnimation(AnimationUtils2);
                        Linearlayout28.StartAnimation(AnimationUtils3);
                        Linearlayout29.StartAnimation(AnimationUtils4);
                        Linearlayout30.StartAnimation(AnimationUtils5);
                    }
                }
                else if (TouchedLeft > (objwebview.Width - AppValidation.ToPixels(this, 100)))
                {
                    Textmenu5.Text = (ObjButtonlist.Count(s => s.status == true) + 1).ToString();
                    RelativeLayout.LayoutParams layoutParams = new RelativeLayout.LayoutParams(menuTopRight.Width, menuTopRight.Height);
                    //SelectedY = (XTopMargin - 50);
                    //SelectedX = (XLeftMargin - 75);
                    PerSelectedY = (XTopMargin - AppValidation.ToPixels(this, 28));
                    PerSelectedX = (XLeftMargin - (menuTopRight.Width - AppValidation.ToPixels(this, 28)));
                    layoutParams.TopMargin = PerSelectedY;
                    layoutParams.LeftMargin = PerSelectedX;
                    menuTopRight.LayoutParameters = layoutParams;
                    //Log.Debug("menuTopRight", "Width: " + menuTopRight.Width + ", Height " + menuTopRight.Height);
                    menuLeftCenter.Visibility = ViewStates.Gone;
                    menuRightCenter.Visibility = ViewStates.Gone;
                    menuTopCenter.Visibility = ViewStates.Gone;
                    menuTopLeft.Visibility = ViewStates.Gone;
                    menuTopRight.Visibility = ViewStates.Visible;
                    menuBottomLeft.Visibility = ViewStates.Gone;
                    menuBottomRight.Visibility = ViewStates.Gone;
                    menuBottomCenter.Visibility = ViewStates.Gone;
                    if (animation)
                    {
                        Linearlayout21.StartAnimation(AnimationUtils1);
                        Linearlayout22.StartAnimation(AnimationUtils2);
                        Linearlayout23.StartAnimation(AnimationUtils3);
                        Linearlayout24.StartAnimation(AnimationUtils4);
                        Linearlayout25.StartAnimation(AnimationUtils5);
                    }

                }
                else
                {
                    Textmenu3.Text = (ObjButtonlist.Count(s => s.status == true) + 1).ToString();
                    RelativeLayout.LayoutParams layoutParams = new RelativeLayout.LayoutParams(menuTopCenter.Width, menuTopCenter.Height);
                    //SelectedY = (XTopMargin - 50);
                    //SelectedX = (XLeftMargin - 75);
                    PerSelectedY = (XTopMargin - AppValidation.ToPixels(this, 28));
                    PerSelectedX = (XLeftMargin - (menuTopCenter.Width / 2));
                    layoutParams.TopMargin = PerSelectedY;
                    layoutParams.LeftMargin = PerSelectedX;
                    menuTopCenter.LayoutParameters = layoutParams;
                  //  Log.Debug("menuTopCenter", "Width: " + menuTopCenter.Width + ", Height " + menuTopCenter.Height);
                    menuLeftCenter.Visibility = ViewStates.Gone;
                    menuRightCenter.Visibility = ViewStates.Gone;
                    menuTopCenter.Visibility = ViewStates.Visible;
                    menuTopLeft.Visibility = ViewStates.Gone;
                    menuTopRight.Visibility = ViewStates.Gone;
                    menuBottomLeft.Visibility = ViewStates.Gone;
                    menuBottomRight.Visibility = ViewStates.Gone;
                    menuBottomCenter.Visibility = ViewStates.Gone;
                    if (animation)
                    {
                        Linearlayout11.StartAnimation(AnimationUtils1);
                        Linearlayout12.StartAnimation(AnimationUtils2);
                        Linearlayout13.StartAnimation(AnimationUtils3);
                        Linearlayout14.StartAnimation(AnimationUtils4);
                        Linearlayout15.StartAnimation(AnimationUtils5);
                    }
                }
            }
            else if (TouchedTop > (objwebview.Height - AppValidation.ToPixels(this, 150)))
            {
                if (TouchedLeft < AppValidation.ToPixels(this, 100))
                {
                    Textmenu7.Text = (ObjButtonlist.Count(s => s.status == true) + 1).ToString(); menuLeftCenter.Visibility = ViewStates.Gone;
                    RelativeLayout.LayoutParams layoutParams = new RelativeLayout.LayoutParams(menuBottomLeft.Width, menuBottomLeft.Height);
                    //SelectedY = (XTopMargin - 50);
                    //SelectedX = (XLeftMargin - 75);
                    PerSelectedY = (XTopMargin - (menuBottomLeft.Height - AppValidation.ToPixels(this, 28)));
                    PerSelectedX = (XLeftMargin - AppValidation.ToPixels(this, 28));
                    layoutParams.TopMargin = PerSelectedY;
                    layoutParams.LeftMargin = PerSelectedX;
                    menuBottomLeft.LayoutParameters = layoutParams;
                    //Log.Debug("menuBottomLeft", "Width: " + menuBottomLeft.Width + ", Height " + menuBottomLeft.Height);
                    menuRightCenter.Visibility = ViewStates.Gone;
                    menuTopCenter.Visibility = ViewStates.Gone;
                    menuTopLeft.Visibility = ViewStates.Gone;
                    menuTopRight.Visibility = ViewStates.Gone;
                    menuBottomLeft.Visibility = ViewStates.Visible;
                    menuBottomRight.Visibility = ViewStates.Gone;
                    menuBottomCenter.Visibility = ViewStates.Gone;
                    if (animation)
                    {
                        Linearlayout36.StartAnimation(AnimationUtils1);
                        Linearlayout37.StartAnimation(AnimationUtils2);
                        Linearlayout38.StartAnimation(AnimationUtils3);
                        Linearlayout39.StartAnimation(AnimationUtils4);
                        Linearlayout40.StartAnimation(AnimationUtils5);
                    }
                }
                else if (TouchedLeft > (objwebview.Width - AppValidation.ToPixels(this, 100)))
                {
                    Textmenu8.Text = (ObjButtonlist.Count(s => s.status == true) + 1).ToString();
                    RelativeLayout.LayoutParams layoutParams = new RelativeLayout.LayoutParams(menuBottomRight.Width, menuBottomRight.Height);
                    //SelectedY = (XTopMargin - 50);
                    //SelectedX = (XLeftMargin - 75);
                    PerSelectedY = (XTopMargin - (menuBottomRight.Height - AppValidation.ToPixels(this, 28)));
                    PerSelectedX = (XLeftMargin- (menuBottomRight.Width - AppValidation.ToPixels(this, 28)));
                   layoutParams.TopMargin = PerSelectedY;
                    layoutParams.LeftMargin = PerSelectedX;
                    menuBottomRight.LayoutParameters = layoutParams;
                    //Log.Debug("menuBottomRight", "Width: " + menuBottomRight.Width + ", Height " + menuBottomRight.Height);
                    menuLeftCenter.Visibility = ViewStates.Gone;
                    menuRightCenter.Visibility = ViewStates.Gone;
                    menuTopCenter.Visibility = ViewStates.Gone;
                    menuTopLeft.Visibility = ViewStates.Gone;
                    menuTopRight.Visibility = ViewStates.Gone;
                    menuBottomLeft.Visibility = ViewStates.Gone;
                    menuBottomRight.Visibility = ViewStates.Visible;
                    menuBottomCenter.Visibility = ViewStates.Gone;
                    if (animation)
                    {
                        Linearlayout31.StartAnimation(AnimationUtils1);
                        Linearlayout32.StartAnimation(AnimationUtils2);
                        Linearlayout33.StartAnimation(AnimationUtils3);
                        Linearlayout34.StartAnimation(AnimationUtils4);
                        Linearlayout35.StartAnimation(AnimationUtils5);
                    }
                }
                else
                {
                    Textmenu4.Text = (ObjButtonlist.Count(s => s.status == true) + 1).ToString();
                    RelativeLayout.LayoutParams layoutParams = new RelativeLayout.LayoutParams(menuBottomCenter.Width, menuBottomCenter.Height);
                    //SelectedY = (XTopMargin - 50);
                    //SelectedX = (XLeftMargin - 75);
                    PerSelectedY = (XTopMargin - (menuBottomCenter.Height - AppValidation.ToPixels(this, 28)));
                    PerSelectedX = (XLeftMargin - (menuBottomCenter.Width/2));
                    layoutParams.TopMargin = PerSelectedY;
                    layoutParams.LeftMargin = PerSelectedX;
                    menuBottomCenter.LayoutParameters = layoutParams;
                    //Log.Debug("menuBottomCenter", "Width: " + menuBottomCenter.Width + ", Height " + menuBottomCenter.Height);
                    menuLeftCenter.Visibility = ViewStates.Gone;
                    menuRightCenter.Visibility = ViewStates.Gone;
                    menuTopCenter.Visibility = ViewStates.Gone;
                    menuTopLeft.Visibility = ViewStates.Gone;
                    menuTopRight.Visibility = ViewStates.Gone;
                    menuBottomLeft.Visibility = ViewStates.Gone;
                    menuBottomRight.Visibility = ViewStates.Gone;
                    menuBottomCenter.Visibility = ViewStates.Visible;
                    if (animation)
                    {
                        Linearlayout16.StartAnimation(AnimationUtils1);
                        Linearlayout17.StartAnimation(AnimationUtils2);
                        Linearlayout18.StartAnimation(AnimationUtils3);
                        Linearlayout19.StartAnimation(AnimationUtils4);
                        Linearlayout20.StartAnimation(AnimationUtils5);
                    }
                }
            }
            else if (TouchedLeft > (Resources.DisplayMetrics.WidthPixels/2))
            {
                Textmenu2.Text = (ObjButtonlist.Count(s => s.status == true) + 1).ToString();
                RelativeLayout.LayoutParams layoutParams = new RelativeLayout.LayoutParams(menuRightCenter.Width, menuRightCenter.Height);
                //SelectedY = (XTopMargin - 100);
                //SelectedX = (XLeftMargin - 150);
                PerSelectedY = (XTopMargin - (menuRightCenter.Height / 2));
                PerSelectedX = (XLeftMargin - (menuRightCenter.Width - AppValidation.ToPixels(this, 28)));
                layoutParams.TopMargin = PerSelectedY;
                layoutParams.LeftMargin = PerSelectedX;
                menuRightCenter.LayoutParameters = layoutParams;
                //Log.Debug("menuRightCenter", "Width: " + menuRightCenter.Width + ", Height " + menuRightCenter.Height);
                menuLeftCenter.Visibility = ViewStates.Gone;
                menuRightCenter.Visibility = ViewStates.Visible;
                menuTopCenter.Visibility = ViewStates.Gone;
                menuTopLeft.Visibility = ViewStates.Gone;
                menuTopRight.Visibility = ViewStates.Gone;
                menuBottomLeft.Visibility = ViewStates.Gone;
                menuBottomRight.Visibility = ViewStates.Gone;
                menuBottomCenter.Visibility = ViewStates.Gone;
                if (animation)
                {
                    Linearlayout6.StartAnimation(AnimationUtils6);
                    Linearlayout7.StartAnimation(AnimationUtils7);
                    Linearlayout8.StartAnimation(AnimationUtils8);
                    Linearlayout9.StartAnimation(AnimationUtils9);
                    Linearlayout10.StartAnimation(AnimationUtils10);
                }
            }
            else
            {
                Textmenu1.Text = (ObjButtonlist.Count(s => s.status == true) + 1).ToString();
                RelativeLayout.LayoutParams layoutParams = new RelativeLayout.LayoutParams(menuLeftCenter.Width, menuLeftCenter.Height);
                //SelectedY = (XTopMargin - 100);
                //SelectedX = (XLeftMargin);
                PerSelectedY = (XTopMargin - (menuLeftCenter.Height / 2));
                PerSelectedX = (XLeftMargin - 60);
                layoutParams.TopMargin = PerSelectedY;
                layoutParams.LeftMargin = PerSelectedX;
                menuLeftCenter.LayoutParameters = layoutParams;
                //Log.Debug("menuLeftCenter", "Width: " + menuLeftCenter.Width + ", Height " + menuLeftCenter.Height);
                menuLeftCenter.Visibility = ViewStates.Visible;
                menuRightCenter.Visibility = ViewStates.Gone;
                menuTopCenter.Visibility = ViewStates.Gone;
                menuTopLeft.Visibility = ViewStates.Gone;
                menuTopRight.Visibility = ViewStates.Gone;
                menuBottomLeft.Visibility = ViewStates.Gone;
                menuBottomRight.Visibility = ViewStates.Gone;
                menuBottomCenter.Visibility = ViewStates.Gone;
                if (animation)
                {
                    Linearlayout1.StartAnimation(AnimationUtils1);
                    Linearlayout2.StartAnimation(AnimationUtils2);
                    Linearlayout3.StartAnimation(AnimationUtils3);
                    Linearlayout4.StartAnimation(AnimationUtils4);
                    Linearlayout5.StartAnimation(AnimationUtils5);
                }
            }
           // Log.Debug("scale", "X: " + TouchedX + ", Y " + TouchedY +" ImageScale = " + ImageScale);

        }
        private async void CheckFlatoptions(float Scale)
        {
            Task.Delay(300);
            this.RunOnUiThread(() => { 
                Matrix matrix = objwebview.Matrix;
                float[] pts = { 0, 0 };
                matrix.MapPoints(pts);
                int XLeftMargin, XTopMargin;
                if (ClickScale)
                {
                    XLeftMargin = (int)((TouchedX * (Scale/ ClickScalev)) - pts[0]);
                    XTopMargin = (int)((TouchedY * (Scale/ ClickScalev)) - pts[1]);
                }
                else
                {
                    XLeftMargin = (int)((TouchedX * Scale) + pts[0]);
                    XTopMargin = (int)((TouchedY * Scale) + pts[1]);
                }
                //Log.Debug("Reset Flatoption", "LeftMargin: " + XLeftMargin + ", TopMargin " + XTopMargin + " ImageScale = " + Scale);
                if (menuLeftCenter.Visibility == ViewStates.Visible)
                {
                    RelativeLayout.LayoutParams layoutParams = (RelativeLayout.LayoutParams)menuLeftCenter.LayoutParameters;
                    //RelativeLayout.LayoutParams layoutParams = new RelativeLayout.LayoutParams(menuLeftCenter.Width, menuLeftCenter.Height);
                    layoutParams.TopMargin = XTopMargin - (menuLeftCenter.Height / 2);
                    //layoutParams.TopMargin = XTopMargin;
                    layoutParams.LeftMargin = XLeftMargin - 60;
                    menuLeftCenter.LayoutParameters = layoutParams;
                }
                else if (menuRightCenter.Visibility == ViewStates.Visible)
                {
                    //RelativeLayout.LayoutParams layoutParams = new RelativeLayout.LayoutParams(menuRightCenter.Width, menuRightCenter.Height);
                    RelativeLayout.LayoutParams layoutParams = (RelativeLayout.LayoutParams)menuRightCenter.LayoutParameters;
                    layoutParams.TopMargin = XTopMargin - (menuRightCenter.Height / 2);
                    layoutParams.LeftMargin = XLeftMargin - (menuRightCenter.Width - AppValidation.ToPixels(this, 18));
                    //layoutParams.TopMargin = XTopMargin;
                    //layoutParams.LeftMargin = XLeftMargin;
                    menuRightCenter.LayoutParameters = layoutParams;
                }
                else if (menuTopCenter.Visibility == ViewStates.Visible)
                {
                    //RelativeLayout.LayoutParams layoutParams = new RelativeLayout.LayoutParams(menuTopCenter.Width, menuTopCenter.Height);
                    RelativeLayout.LayoutParams layoutParams = (RelativeLayout.LayoutParams)menuTopCenter.LayoutParameters;
                    layoutParams.TopMargin = (XTopMargin - AppValidation.ToPixels(this, 18));
                    layoutParams.LeftMargin = (XLeftMargin - (menuTopCenter.Width / 2));
                    //layoutParams.TopMargin = XTopMargin;
                    //layoutParams.LeftMargin = XLeftMargin;
                    menuTopCenter.LayoutParameters = layoutParams;
                }
                else if (menuTopLeft.Visibility == ViewStates.Visible)
                {
                    //RelativeLayout.LayoutParams layoutParams = new RelativeLayout.LayoutParams(menuTopLeft.Width, menuTopLeft.Height);
                    RelativeLayout.LayoutParams layoutParams = (RelativeLayout.LayoutParams)menuTopLeft.LayoutParameters;
                    layoutParams.TopMargin = (XTopMargin - AppValidation.ToPixels(this, 18));
                    layoutParams.LeftMargin = (XLeftMargin - AppValidation.ToPixels(this, 18));
                    //layoutParams.TopMargin = XTopMargin;
                    //layoutParams.LeftMargin = XLeftMargin;
                    menuTopLeft.LayoutParameters = layoutParams;
                }
                else if (menuTopRight.Visibility == ViewStates.Visible)
                {
                    //RelativeLayout.LayoutParams layoutParams = new RelativeLayout.LayoutParams(menuTopRight.Width, menuTopRight.Height);
                    RelativeLayout.LayoutParams layoutParams = (RelativeLayout.LayoutParams)menuTopRight.LayoutParameters;
                    layoutParams.TopMargin = (XTopMargin - AppValidation.ToPixels(this, 18));
                    layoutParams.LeftMargin = (XLeftMargin - (menuTopRight.Width - AppValidation.ToPixels(this, 18)));
                    //layoutParams.TopMargin = XTopMargin;
                    //layoutParams.LeftMargin = XLeftMargin;
                    menuTopRight.LayoutParameters = layoutParams;
                }
                else if (menuBottomLeft.Visibility == ViewStates.Visible)
                {
                    //RelativeLayout.LayoutParams layoutParams = new RelativeLayout.LayoutParams(menuBottomLeft.Width, menuBottomLeft.Height);
                    RelativeLayout.LayoutParams layoutParams = (RelativeLayout.LayoutParams)menuBottomLeft.LayoutParameters;
                    layoutParams.TopMargin = (XTopMargin - menuBottomLeft.Height);
                    layoutParams.LeftMargin = (XLeftMargin - AppValidation.ToPixels(this, 18));
                    //layoutParams.TopMargin = XTopMargin;
                    //layoutParams.LeftMargin = XLeftMargin;
                    menuBottomLeft.LayoutParameters = layoutParams;
                }
                else if (menuBottomRight.Visibility == ViewStates.Visible)
                {
                    //RelativeLayout.LayoutParams layoutParams = new RelativeLayout.LayoutParams(menuBottomRight.Width, menuBottomRight.Height);
                    RelativeLayout.LayoutParams layoutParams = (RelativeLayout.LayoutParams)menuBottomRight.LayoutParameters;
                    layoutParams.TopMargin = (XTopMargin - menuBottomRight.Height);
                    layoutParams.LeftMargin = (XLeftMargin - (menuBottomRight.Width - AppValidation.ToPixels(this, 18)));
                    //layoutParams.TopMargin = XTopMargin;
                    //layoutParams.LeftMargin = XLeftMargin;
                    menuBottomRight.LayoutParameters = layoutParams;
                }
                else if (menuBottomCenter.Visibility == ViewStates.Visible)
                {
                    //RelativeLayout.LayoutParams layoutParams = new RelativeLayout.LayoutParams(menuBottomCenter.Width, menuBottomCenter.Height);
                    RelativeLayout.LayoutParams layoutParams = (RelativeLayout.LayoutParams)menuBottomCenter.LayoutParameters;
                    layoutParams.TopMargin = (XTopMargin - (menuBottomCenter.Height - AppValidation.ToPixels(this, 18)));
                    layoutParams.LeftMargin = (XLeftMargin - (menuBottomCenter.Width / 2));
                    //layoutParams.TopMargin = XTopMargin;
                    //layoutParams.LeftMargin = XLeftMargin;
                    menuBottomCenter.LayoutParameters = layoutParams;
                }

                for (int i = 0; i < ObjButtonlist.Count; i++)
                {
                    //RelativeLayout.LayoutParams layoutParams = new RelativeLayout.LayoutParams(objLayoutlist[i].Width, objLayoutlist[i].Height);
                    RelativeLayout.LayoutParams layoutParams = (RelativeLayout.LayoutParams)objLayoutlist[i].LayoutParameters;
                    layoutParams.TopMargin = ((int)((ObjButtonlist[i].SelectedY * Scale) + pts[0])) - AppValidation.ToPixels(this, 30);
                    layoutParams.LeftMargin = ((int)((ObjButtonlist[i].SelectedX * Scale) + pts[1])) - AppValidation.ToPixels(this, 30);
                    objLayoutlist[i].LayoutParameters = layoutParams;
                }
            });
        }        
        public void onScrollChange(WebView v, int scrollX, int scrollY, int oldScrollX, int oldScrollY)
        {
            WebviewScrollx = scrollX;
            WebviewScrollY = scrollY;
        }
        public void onScaleChange(WebView v, float oldScale, float newScale)
        {
            if (Startscale)
            {
                if (oldscale != newScale)
                {
                    ImageScale = newScale / StartScale;
                    CheckFlatoptions(ImageScale);
                }
            }

            // Log.Debug("Scale GestureDetector", "ScaleFactor: " + newScale);
            //Log.Debug("onScale Change", "ImageScale: " + oldscale + " ImageScale: " + newScale);
        }
        TextView Textmenu1, Textmenu2, Textmenu3, Textmenu4, Textmenu5, Textmenu6, Textmenu7, Textmenu8;
        RelativeLayout menuLeftCenter, menuRightCenter, menuBottomLeft, menuBottomRight, menuBottomCenter, menuTopLeft, menuTopRight, menuTopCenter;
        Animation AnimationUtils1, AnimationUtils2, AnimationUtils3, AnimationUtils4, AnimationUtils5, AnimationUtils6, AnimationUtils7, AnimationUtils8, AnimationUtils9, AnimationUtils10;
        TextView Textview1, Textview2, Textview3, Textview4, Textview5, Textview6, Textview7, Textview8, Textview9, Textview10;
        LinearLayout Linearlayout1, Linearlayout2, Linearlayout3, Linearlayout4, Linearlayout5, Linearlayout6, Linearlayout7, Linearlayout8, Linearlayout9, Linearlayout10;
        TextView Textview11, Textview12, Textview13, Textview14, Textview15, Textview16, Textview17, Textview18, Textview19, Textview20;
        LinearLayout Linearlayout11, Linearlayout12, Linearlayout13, Linearlayout14, Linearlayout15, Linearlayout16, Linearlayout17, Linearlayout18, Linearlayout19, Linearlayout20;
        TextView Textview21, Textview22, Textview23, Textview24, Textview25, Textview26, Textview27, Textview28, Textview29, Textview30;
        LinearLayout Linearlayout21, Linearlayout22, Linearlayout23, Linearlayout24, Linearlayout25, Linearlayout26, Linearlayout27, Linearlayout28, Linearlayout29, Linearlayout30;
        TextView Textview31, Textview32, Textview33, Textview34, Textview35, Textview36, Textview37, Textview38, Textview39, Textview40;
        LinearLayout Linearlayout31, Linearlayout32, Linearlayout33, Linearlayout34, Linearlayout35, Linearlayout36, Linearlayout37, Linearlayout38, Linearlayout39, Linearlayout40;
        private void LoadPlotterDesign()
        {

            Textmenu1 = FindViewById<TextView>(Resource.Id.Textmenu);
            Textmenu2 = FindViewById<TextView>(Resource.Id.Textmenu2);
            Textmenu3 = FindViewById<TextView>(Resource.Id.Textmenu3);
            Textmenu4 = FindViewById<TextView>(Resource.Id.Textmenu4);
            Textmenu5 = FindViewById<TextView>(Resource.Id.Textmenu5);
            Textmenu6 = FindViewById<TextView>(Resource.Id.Textmenu6);
            Textmenu7 = FindViewById<TextView>(Resource.Id.Textmenu7);
            Textmenu8 = FindViewById<TextView>(Resource.Id.Textmenu8);

            menuLeftCenter = FindViewById<RelativeLayout>(Resource.Id.menuLeftCenter);
            menuRightCenter = FindViewById<RelativeLayout>(Resource.Id.menuRightCenter);
            menuTopCenter = FindViewById<RelativeLayout>(Resource.Id.menuTopCenter);
            menuTopLeft = FindViewById<RelativeLayout>(Resource.Id.menuTopLeft);
            menuTopRight = FindViewById<RelativeLayout>(Resource.Id.menuTopRight);
            menuBottomLeft = FindViewById<RelativeLayout>(Resource.Id.menuBottomLeft);
            menuBottomRight = FindViewById<RelativeLayout>(Resource.Id.menuBottomRight);
            menuBottomCenter = FindViewById<RelativeLayout>(Resource.Id.menuBottomCenter);

            menuLeftCenter.Visibility = ViewStates.Invisible;
            menuRightCenter.Visibility = ViewStates.Invisible;
            menuTopCenter.Visibility = ViewStates.Invisible;
            menuTopLeft.Visibility = ViewStates.Invisible;
            menuTopRight.Visibility = ViewStates.Invisible;
            menuBottomLeft.Visibility = ViewStates.Invisible;
            menuBottomRight.Visibility = ViewStates.Invisible;
            menuBottomCenter.Visibility = ViewStates.Invisible;

            Linearlayout1 = FindViewById<LinearLayout>(Resource.Id.Linearlayout1);
            Linearlayout2 = FindViewById<LinearLayout>(Resource.Id.Linearlayout2);
            Linearlayout3 = FindViewById<LinearLayout>(Resource.Id.Linearlayout3);
            Linearlayout4 = FindViewById<LinearLayout>(Resource.Id.Linearlayout4);
            Linearlayout5 = FindViewById<LinearLayout>(Resource.Id.Linearlayout5);
            Linearlayout6 = FindViewById<LinearLayout>(Resource.Id.Linearlayout6);
            Linearlayout7 = FindViewById<LinearLayout>(Resource.Id.Linearlayout7);
            Linearlayout8 = FindViewById<LinearLayout>(Resource.Id.Linearlayout8);
            Linearlayout9 = FindViewById<LinearLayout>(Resource.Id.Linearlayout9);
            Linearlayout10 = FindViewById<LinearLayout>(Resource.Id.Linearlayout10);
            Linearlayout11 = FindViewById<LinearLayout>(Resource.Id.Linearlayout11);
            Linearlayout12 = FindViewById<LinearLayout>(Resource.Id.Linearlayout12);
            Linearlayout13 = FindViewById<LinearLayout>(Resource.Id.Linearlayout13);
            Linearlayout14 = FindViewById<LinearLayout>(Resource.Id.Linearlayout14);
            Linearlayout15 = FindViewById<LinearLayout>(Resource.Id.Linearlayout15);
            Linearlayout16 = FindViewById<LinearLayout>(Resource.Id.Linearlayout16);
            Linearlayout17 = FindViewById<LinearLayout>(Resource.Id.Linearlayout17);
            Linearlayout18 = FindViewById<LinearLayout>(Resource.Id.Linearlayout18);
            Linearlayout19 = FindViewById<LinearLayout>(Resource.Id.Linearlayout19);
            Linearlayout20 = FindViewById<LinearLayout>(Resource.Id.Linearlayout20);
            Linearlayout21 = FindViewById<LinearLayout>(Resource.Id.Linearlayout21);
            Linearlayout22 = FindViewById<LinearLayout>(Resource.Id.Linearlayout22);
            Linearlayout23 = FindViewById<LinearLayout>(Resource.Id.Linearlayout23);
            Linearlayout24 = FindViewById<LinearLayout>(Resource.Id.Linearlayout24);
            Linearlayout25 = FindViewById<LinearLayout>(Resource.Id.Linearlayout25);
            Linearlayout26 = FindViewById<LinearLayout>(Resource.Id.Linearlayout26);
            Linearlayout27 = FindViewById<LinearLayout>(Resource.Id.Linearlayout27);
            Linearlayout28 = FindViewById<LinearLayout>(Resource.Id.Linearlayout28);
            Linearlayout29 = FindViewById<LinearLayout>(Resource.Id.Linearlayout29);
            Linearlayout30 = FindViewById<LinearLayout>(Resource.Id.Linearlayout30);
            Linearlayout31 = FindViewById<LinearLayout>(Resource.Id.Linearlayout31);
            Linearlayout32 = FindViewById<LinearLayout>(Resource.Id.Linearlayout32);
            Linearlayout33 = FindViewById<LinearLayout>(Resource.Id.Linearlayout33);
            Linearlayout34 = FindViewById<LinearLayout>(Resource.Id.Linearlayout34);
            Linearlayout35 = FindViewById<LinearLayout>(Resource.Id.Linearlayout35);
            Linearlayout36 = FindViewById<LinearLayout>(Resource.Id.Linearlayout36);
            Linearlayout37 = FindViewById<LinearLayout>(Resource.Id.Linearlayout37);
            Linearlayout38 = FindViewById<LinearLayout>(Resource.Id.Linearlayout38);
            Linearlayout39 = FindViewById<LinearLayout>(Resource.Id.Linearlayout39);
            Linearlayout40 = FindViewById<LinearLayout>(Resource.Id.Linearlayout40);

            Textview1 = FindViewById<TextView>(Resource.Id.Textview1);
            Textview2 = FindViewById<TextView>(Resource.Id.Textview2);
            Textview3 = FindViewById<TextView>(Resource.Id.Textview3);
            Textview4 = FindViewById<TextView>(Resource.Id.Textview4);
            Textview5 = FindViewById<TextView>(Resource.Id.Textview5);
            Textview6 = FindViewById<TextView>(Resource.Id.Textview6);
            Textview7 = FindViewById<TextView>(Resource.Id.Textview7);
            Textview8 = FindViewById<TextView>(Resource.Id.Textview8);
            Textview9 = FindViewById<TextView>(Resource.Id.Textview9);
            Textview10 = FindViewById<TextView>(Resource.Id.Textview10);
            Textview11 = FindViewById<TextView>(Resource.Id.Textview11);
            Textview12 = FindViewById<TextView>(Resource.Id.Textview12);
            Textview13 = FindViewById<TextView>(Resource.Id.Textview13);
            Textview14 = FindViewById<TextView>(Resource.Id.Textview14);
            Textview15 = FindViewById<TextView>(Resource.Id.Textview15);
            Textview16 = FindViewById<TextView>(Resource.Id.Textview16);
            Textview17 = FindViewById<TextView>(Resource.Id.Textview17);
            Textview18 = FindViewById<TextView>(Resource.Id.Textview18);
            Textview19 = FindViewById<TextView>(Resource.Id.Textview19);
            Textview20 = FindViewById<TextView>(Resource.Id.Textview20);
            Textview21 = FindViewById<TextView>(Resource.Id.Textview21);
            Textview22 = FindViewById<TextView>(Resource.Id.Textview22);
            Textview23 = FindViewById<TextView>(Resource.Id.Textview23);
            Textview24 = FindViewById<TextView>(Resource.Id.Textview24);
            Textview25 = FindViewById<TextView>(Resource.Id.Textview25);
            Textview26 = FindViewById<TextView>(Resource.Id.Textview26);
            Textview27 = FindViewById<TextView>(Resource.Id.Textview27);
            Textview28 = FindViewById<TextView>(Resource.Id.Textview28);
            Textview29 = FindViewById<TextView>(Resource.Id.Textview29);
            Textview30 = FindViewById<TextView>(Resource.Id.Textview30);
            Textview31 = FindViewById<TextView>(Resource.Id.Textview31);
            Textview32 = FindViewById<TextView>(Resource.Id.Textview32);
            Textview33 = FindViewById<TextView>(Resource.Id.Textview33);
            Textview34 = FindViewById<TextView>(Resource.Id.Textview34);
            Textview35 = FindViewById<TextView>(Resource.Id.Textview35);
            Textview36 = FindViewById<TextView>(Resource.Id.Textview36);
            Textview37 = FindViewById<TextView>(Resource.Id.Textview37);
            Textview38 = FindViewById<TextView>(Resource.Id.Textview38);
            Textview39 = FindViewById<TextView>(Resource.Id.Textview39);
            Textview40 = FindViewById<TextView>(Resource.Id.Textview40);
            
            Textview1.Click += (o, e) => PressFlatButton(1);
            Textview2.Click += (o, e) => PressFlatButton(2);
            Textview3.Click += (o, e) => PressFlatButton(3);
            Textview4.Click += (o, e) => PressFlatButton(4);
            Textview5.Click += (o, e) => PressFlatButton(5);
            Textview6.Click += (o, e) => PressFlatButton(1);
            Textview7.Click += (o, e) => PressFlatButton(2);
            Textview8.Click += (o, e) => PressFlatButton(3);
            Textview9.Click += (o, e) => PressFlatButton(4);
            Textview10.Click += (o, e) => PressFlatButton(5);
            Textview11.Click += (o, e) => PressFlatButton(5);
            Textview12.Click += (o, e) => PressFlatButton(4);
            Textview13.Click += (o, e) => PressFlatButton(2);
            Textview14.Click += (o, e) => PressFlatButton(1);
            Textview15.Click += (o, e) => PressFlatButton(3);
            Textview16.Click += (o, e) => PressFlatButton(2);
            Textview17.Click += (o, e) => PressFlatButton(1);
            Textview18.Click += (o, e) => PressFlatButton(5);
            Textview19.Click += (o, e) => PressFlatButton(4);
            Textview20.Click += (o, e) => PressFlatButton(3);
            Textview21.Click += (o, e) => PressFlatButton(5);
            Textview22.Click += (o, e) => PressFlatButton(4);
            Textview23.Click += (o, e) => PressFlatButton(2);
            Textview24.Click += (o, e) => PressFlatButton(1);
            Textview25.Click += (o, e) => PressFlatButton(3);
            Textview26.Click += (o, e) => PressFlatButton(3);
            Textview27.Click += (o, e) => PressFlatButton(1);
            Textview28.Click += (o, e) => PressFlatButton(2);
            Textview29.Click += (o, e) => PressFlatButton(4);
            Textview30.Click += (o, e) => PressFlatButton(5);
            Textview31.Click += (o, e) => PressFlatButton(3);
            Textview32.Click += (o, e) => PressFlatButton(4);
            Textview33.Click += (o, e) => PressFlatButton(5);
            Textview34.Click += (o, e) => PressFlatButton(1);
            Textview35.Click += (o, e) => PressFlatButton(2);
            Textview36.Click += (o, e) => PressFlatButton(3);
            Textview37.Click += (o, e) => PressFlatButton(4);
            Textview38.Click += (o, e) => PressFlatButton(5);
            Textview39.Click += (o, e) => PressFlatButton(1);
            Textview40.Click += (o, e) => PressFlatButton(2);

           ImageView Imageview1 = FindViewById<ImageView>(Resource.Id.Imageview1);
            ImageView Imageview2 = FindViewById<ImageView>(Resource.Id.Imageview2);
            ImageView Imageview3 = FindViewById<ImageView>(Resource.Id.Imageview3);
            ImageView Imageview4 = FindViewById<ImageView>(Resource.Id.Imageview4);
            ImageView Imageview5 = FindViewById<ImageView>(Resource.Id.Imageview5);
            ImageView Imageview6 = FindViewById<ImageView>(Resource.Id.Imageview6);
            ImageView Imageview7 = FindViewById<ImageView>(Resource.Id.Imageview7);
            ImageView Imageview8 = FindViewById<ImageView>(Resource.Id.Imageview8);
            ImageView Imageview9 = FindViewById<ImageView>(Resource.Id.Imageview9);
            ImageView Imageview10 = FindViewById<ImageView>(Resource.Id.Imageview10);
            ImageView Imageview11 = FindViewById<ImageView>(Resource.Id.Imageview11);
            ImageView Imageview12 = FindViewById<ImageView>(Resource.Id.Imageview12);
            ImageView Imageview13 = FindViewById<ImageView>(Resource.Id.Imageview13);
            ImageView Imageview14 = FindViewById<ImageView>(Resource.Id.Imageview14);
            ImageView Imageview15 = FindViewById<ImageView>(Resource.Id.Imageview15);
            ImageView Imageview16 = FindViewById<ImageView>(Resource.Id.Imageview16);
            ImageView Imageview17 = FindViewById<ImageView>(Resource.Id.Imageview17);
            ImageView Imageview18 = FindViewById<ImageView>(Resource.Id.Imageview18);
            ImageView Imageview19 = FindViewById<ImageView>(Resource.Id.Imageview19);
            ImageView Imageview20 = FindViewById<ImageView>(Resource.Id.Imageview20);
            ImageView Imageview21 = FindViewById<ImageView>(Resource.Id.Imageview21);
            ImageView Imageview22 = FindViewById<ImageView>(Resource.Id.Imageview22);
            ImageView Imageview23 = FindViewById<ImageView>(Resource.Id.Imageview23);
            ImageView Imageview24 = FindViewById<ImageView>(Resource.Id.Imageview24);
            ImageView Imageview25 = FindViewById<ImageView>(Resource.Id.Imageview25);
            ImageView Imageview26 = FindViewById<ImageView>(Resource.Id.Imageview26);
            ImageView Imageview27 = FindViewById<ImageView>(Resource.Id.Imageview27);
            ImageView Imageview28 = FindViewById<ImageView>(Resource.Id.Imageview28);
            ImageView Imageview29 = FindViewById<ImageView>(Resource.Id.Imageview29);
            ImageView Imageview30 = FindViewById<ImageView>(Resource.Id.Imageview30);
            ImageView Imageview31 = FindViewById<ImageView>(Resource.Id.Imageview31);
            ImageView Imageview32 = FindViewById<ImageView>(Resource.Id.Imageview32);
            ImageView Imageview33 = FindViewById<ImageView>(Resource.Id.Imageview33);
            ImageView Imageview34 = FindViewById<ImageView>(Resource.Id.Imageview34);
            ImageView Imageview35 = FindViewById<ImageView>(Resource.Id.Imageview35);
            ImageView Imageview36 = FindViewById<ImageView>(Resource.Id.Imageview36);
            ImageView Imageview37 = FindViewById<ImageView>(Resource.Id.Imageview37);
            ImageView Imageview38 = FindViewById<ImageView>(Resource.Id.Imageview38);
            ImageView Imageview39 = FindViewById<ImageView>(Resource.Id.Imageview39);
            ImageView Imageview40 = FindViewById<ImageView>(Resource.Id.Imageview40);


            //1   =>  Imageflat
            //2   => Textflat
            //3   =>  Videoflat
            //4   =>  Linkflat
            //5   =>  Designflat

            Imageview1.Click += (o, e) => PressFlatButton(1);
            Imageview2.Click += (o, e) => PressFlatButton(2);
            Imageview3.Click += (o, e) => PressFlatButton(3);
            Imageview4.Click += (o, e) => PressFlatButton(4);
            Imageview5.Click += (o, e) => PressFlatButton(5);
            Imageview6.Click += (o, e) => PressFlatButton(1);
            Imageview7.Click += (o, e) => PressFlatButton(2);
            Imageview8.Click += (o, e) => PressFlatButton(3);
            Imageview9.Click += (o, e) => PressFlatButton(4);
            Imageview10.Click += (o, e) => PressFlatButton(5);
            Imageview11.Click += (o, e) => PressFlatButton(5);
            Imageview12.Click += (o, e) => PressFlatButton(4);
            Imageview13.Click += (o, e) => PressFlatButton(2);
            Imageview14.Click += (o, e) => PressFlatButton(1);
            Imageview15.Click += (o, e) => PressFlatButton(3);
            Imageview16.Click += (o, e) => PressFlatButton(2);
            Imageview17.Click += (o, e) => PressFlatButton(1);
            Imageview18.Click += (o, e) => PressFlatButton(5);
            Imageview19.Click += (o, e) => PressFlatButton(4);
            Imageview20.Click += (o, e) => PressFlatButton(3);
            Imageview21.Click += (o, e) => PressFlatButton(5);
            Imageview22.Click += (o, e) => PressFlatButton(4);
            Imageview23.Click += (o, e) => PressFlatButton(2);
            Imageview24.Click += (o, e) => PressFlatButton(1);
            Imageview25.Click += (o, e) => PressFlatButton(3);
            Imageview26.Click += (o, e) => PressFlatButton(3);
            Imageview27.Click += (o, e) => PressFlatButton(1);
            Imageview28.Click += (o, e) => PressFlatButton(2);
            Imageview29.Click += (o, e) => PressFlatButton(4);
            Imageview30.Click += (o, e) => PressFlatButton(5);
            Imageview31.Click += (o, e) => PressFlatButton(3);
            Imageview32.Click += (o, e) => PressFlatButton(4);
            Imageview33.Click += (o, e) => PressFlatButton(5);
            Imageview34.Click += (o, e) => PressFlatButton(1);
            Imageview35.Click += (o, e) => PressFlatButton(2);
            Imageview36.Click += (o, e) => PressFlatButton(3);
            Imageview37.Click += (o, e) => PressFlatButton(4);
            Imageview38.Click += (o, e) => PressFlatButton(5);
            Imageview39.Click += (o, e) => PressFlatButton(1);
            Imageview40.Click += (o, e) => PressFlatButton(2);

            ImageView Closemenu1 = FindViewById<ImageView>(Resource.Id.Closemenu);
            ImageView Closemenu2 = FindViewById<ImageView>(Resource.Id.Closemenu2);
            ImageView Closemenu3 = FindViewById<ImageView>(Resource.Id.Closemenu3);
            ImageView Closemenu4 = FindViewById<ImageView>(Resource.Id.Closemenu4);
            ImageView Closemenu5 = FindViewById<ImageView>(Resource.Id.Closemenu5);
            ImageView Closemenu6 = FindViewById<ImageView>(Resource.Id.Closemenu6);
            ImageView Closemenu7 = FindViewById<ImageView>(Resource.Id.Closemenu7);
            ImageView Closemenu8 = FindViewById<ImageView>(Resource.Id.Closemenu8);

            Closemenu1.Click += (o, e) => CloseFlatoptions();
            Closemenu2.Click += (o, e) => CloseFlatoptions();
            Closemenu3.Click += (o, e) => CloseFlatoptions();
            Closemenu4.Click += (o, e) => CloseFlatoptions();
            Closemenu5.Click += (o, e) => CloseFlatoptions();
            Closemenu6.Click += (o, e) => CloseFlatoptions();
            Closemenu7.Click += (o, e) => CloseFlatoptions();
            Closemenu8.Click += (o, e) => CloseFlatoptions();

            Textmenu1.Click += (o, e) => PressFlatButton();
            Textmenu2.Click += (o, e) => PressFlatButton();
            Textmenu3.Click += (o, e) => PressFlatButton();
            Textmenu4.Click += (o, e) => PressFlatButton();
            Textmenu5.Click += (o, e) => PressFlatButton();
            Textmenu6.Click += (o, e) => PressFlatButton();
            Textmenu7.Click += (o, e) => PressFlatButton();
            Textmenu8.Click += (o, e) => PressFlatButton();

            AnimationUtils1 = AnimationUtils.LoadAnimation(this, Resource.Drawable.Rotatetop21);
            AnimationUtils2 = AnimationUtils.LoadAnimation(this, Resource.Drawable.Rotatetop21);
            AnimationUtils3 = AnimationUtils.LoadAnimation(this, Resource.Drawable.Rotatetop2);
            AnimationUtils4 = AnimationUtils.LoadAnimation(this, Resource.Drawable.Rotatetop31);
            AnimationUtils5 = AnimationUtils.LoadAnimation(this, Resource.Drawable.Rotatetop31);
            AnimationUtils6 = AnimationUtils.LoadAnimation(this, Resource.Drawable.Rotatetop41);
            AnimationUtils7 = AnimationUtils.LoadAnimation(this, Resource.Drawable.Rotatetop41);
            AnimationUtils8 = AnimationUtils.LoadAnimation(this, Resource.Drawable.Rotatetop1);
            AnimationUtils9 = AnimationUtils.LoadAnimation(this, Resource.Drawable.Rotatetop51);
            AnimationUtils10 = AnimationUtils.LoadAnimation(this, Resource.Drawable.Rotatetop51);
            
            Textview1.SetShadowLayer(10, 0, 0, Color.Black);
            Textview2.SetShadowLayer(10, 0, 0, Color.Black);
            Textview3.SetShadowLayer(10, 0, 0, Color.Black);
            Textview4.SetShadowLayer(10, 0, 0, Color.Black);
            Textview5.SetShadowLayer(10, 0, 0, Color.Black);
            Textview6.SetShadowLayer(10, 0, 0, Color.Black);
            Textview7.SetShadowLayer(10, 0, 0, Color.Black);
            Textview8.SetShadowLayer(10, 0, 0, Color.Black);
            Textview9.SetShadowLayer(10, 0, 0, Color.Black);
            Textview10.SetShadowLayer(10, 0, 0, Color.Black);
            Textview11.SetShadowLayer(10, 0, 0, Color.Black);
            Textview12.SetShadowLayer(10, 0, 0, Color.Black);
            Textview13.SetShadowLayer(10, 0, 0, Color.Black);
            Textview14.SetShadowLayer(10, 0, 0, Color.Black);
            Textview15.SetShadowLayer(10, 0, 0, Color.Black);
            Textview16.SetShadowLayer(10, 0, 0, Color.Black);
            Textview17.SetShadowLayer(10, 0, 0, Color.Black);
            Textview18.SetShadowLayer(10, 0, 0, Color.Black);
            Textview19.SetShadowLayer(10, 0, 0, Color.Black);
            Textview20.SetShadowLayer(10, 0, 0, Color.Black);
            Textview21.SetShadowLayer(10, 0, 0, Color.Black);
            Textview22.SetShadowLayer(10, 0, 0, Color.Black);
            Textview23.SetShadowLayer(10, 0, 0, Color.Black);
            Textview24.SetShadowLayer(10, 0, 0, Color.Black);
            Textview25.SetShadowLayer(10, 0, 0, Color.Black);
            Textview26.SetShadowLayer(10, 0, 0, Color.Black);
            Textview27.SetShadowLayer(10, 0, 0, Color.Black);
            Textview28.SetShadowLayer(10, 0, 0, Color.Black);
            Textview29.SetShadowLayer(10, 0, 0, Color.Black);
            Textview30.SetShadowLayer(10, 0, 0, Color.Black);
            Textview31.SetShadowLayer(10, 0, 0, Color.Black);
            Textview32.SetShadowLayer(10, 0, 0, Color.Black);
            Textview33.SetShadowLayer(10, 0, 0, Color.Black);
            Textview34.SetShadowLayer(10, 0, 0, Color.Black);
            Textview35.SetShadowLayer(10, 0, 0, Color.Black);
            Textview36.SetShadowLayer(10, 0, 0, Color.Black);
            Textview37.SetShadowLayer(10, 0, 0, Color.Black);
            Textview38.SetShadowLayer(10, 0, 0, Color.Black);
            Textview39.SetShadowLayer(10, 0, 0, Color.Black);
            Textview40.SetShadowLayer(10, 0, 0, Color.Black);

        }
        public bool OnScale(ScaleGestureDetector detector)
        {
            if (Build.VERSION.SdkInt < Build.VERSION_CODES.Kitkat)
            {
                if (Startscale)
                {
                    ImageScale = objwebview.Scale;
                    CheckFlatoptions(ImageScale);
                    // Log.Debug("Scale GestureDetector", "ScaleFactor: " + objwebview.Scale);
                }
            }
            return true;
        }
        public bool OnScaleBegin(ScaleGestureDetector detector)
        {
            return true;
        }
        public void OnScaleEnd(ScaleGestureDetector detector)
        {
            oldscale = detector.ScaleFactor;
        }
        public void onLoadfinished(WebView v)
        {
            LoadIntentdatas();
        }
    }
    public class ObservableWebView : WebView
    {
        private OnScrollChangeListener onScrollChangeListener;

        public ObservableWebView(Context context) : base(context)
        {
        }

        public ObservableWebView(Context context, IAttributeSet attrs) : base(context, attrs)
        {
        }

        public ObservableWebView(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
        {
        }


        protected override void OnScrollChanged(int l, int t, int oldl, int oldt)
        {
            base.OnScrollChanged(l, t, oldl, oldt);
            if (onScrollChangeListener != null)
            {
                onScrollChangeListener.onScrollChange(this, l, t, oldl, oldt);
            }
        }

        public void setOnScrollChangeListener(OnScrollChangeListener onScrollChangeListener)
        {
            this.onScrollChangeListener = onScrollChangeListener;
        }

        public OnScrollChangeListener getOnScrollChangeListener()
        {
            return onScrollChangeListener;
        }

        public interface OnScrollChangeListener
        {
            /**
             * Called when the scroll position of a view changes.
             *
             * @param v          The view whose scroll position has changed.
             * @param scrollX    Current horizontal scroll origin.
             * @param scrollY    Current vertical scroll origin.
             * @param oldScrollX Previous horizontal scroll origin.
             * @param oldScrollY Previous vertical scroll origin.
             */
            void onScrollChange(WebView v, int scrollX, int scrollY, int oldScrollX, int oldScrollY);
        }
        
    }
    public interface OnScaleChangeListener
    {
        /**
         * Called when the Scale position of a view changes.
         *
         */
        void onScaleChange(WebView v, float oldScale, float newScale);

        void onLoadfinished(WebView v);
    }
}

//if (e.PointerCount <= 1)
//{
//    switch (e.Action & MotionEventActions.Mask)
//    {
//        case MotionEventActions.Move:
//            if (mode == Mode.DRAG)
//            {
//                dx = e.GetX() - startX;
//                dy = e.GetY() - startY;
//            }
//            break;
//        case MotionEventActions.PointerDown:
//            mode = Mode.ZOOM;
//            break;
//        case MotionEventActions.PointerUp:
//            mode = Mode.DRAG;
//            break;
//        case MotionEventActions.Down:
//            Log.Debug(TAG, "DOWN");
//            if (scale > MIN_ZOOM)
//            {
//                mode = Mode.DRAG;
//                startX = e.GetX() - prevDx;
//                startY = e.GetY() - prevDy;
//            }
//            lastTouchDown = time();
//            break;
//        case MotionEventActions.Up:
//            Log.Debug(TAG, "UP");
//            mode = Mode.NONE;
//            prevDx = dx;
//            prevDy = dy;
//            if (time() - lastTouchDown < CLICK_ACTION_THRESHOLD)
//            {
//                menuLeftCenter.Visibility = ViewStates.Gone;
//                menuRightCenter.Visibility = ViewStates.Gone;
//                OpenFalticons((int)Math.Ceiling(e.GetX()), ((int)Math.Ceiling(e.GetY()) + AppValidation.ToPixels(this, 0)));
//            }
//            break;
//    }
//    scaleDetector.OnTouchEvent(e);
//    if ((mode == Mode.DRAG && scale >= MIN_ZOOM) || mode == Mode.ZOOM)
//    {
//        child().Parent.RequestDisallowInterceptTouchEvent(true);
//        float maxDx = (child().Width - (child().Width / scale)) / 2 * scale;
//        float maxDy = (child().Height - (child().Height / scale)) / 2 * scale;
//        dx = Math.Min(Math.Max(dx, -maxDx), maxDx);
//        dy = Math.Min(Math.Max(dy, -maxDy), maxDy);
//        Log.Debug(TAG, "Width: " + child().Width + ", scale " + scale + ", dx " + dx + ", max " + maxDx);
//        applyScaleAndTranslation();
//    }
//    return true;
//}
//else
//{
//    switch (e.Action & MotionEventActions.Mask)
//    {
//        case MotionEventActions.Down:
//            Log.Debug(TAG, "DOWN");
//            if (scale > MIN_ZOOM)
//            {
//                mode = Mode.DRAG;
//                startX = e.GetX() - prevDx;
//                startY = e.GetY() - prevDy;
//            }
//            break;
//        case MotionEventActions.Move:
//            if (mode == Mode.DRAG)
//            {
//                dx = e.GetX() - startX;
//                dy = e.GetY() - startY;
//            }
//            break;
//        case MotionEventActions.PointerDown:
//            mode = Mode.ZOOM;
//            break;
//        case MotionEventActions.PointerUp:
//            mode = Mode.DRAG;
//            break;
//        case MotionEventActions.Up:
//            Log.Debug(TAG, "UP");
//            mode = Mode.NONE;
//            prevDx = dx;
//            prevDy = dy;
//            break;
//    }
//    scaleDetector.OnTouchEvent(e);
//    if ((mode == Mode.DRAG && scale >= MIN_ZOOM) || mode == Mode.ZOOM)
//    {
//        child().Parent.RequestDisallowInterceptTouchEvent(true);
//        float maxDx = (child().Width - (child().Width / scale)) / 2 * scale;
//        float maxDy = (child().Height - (child().Height / scale)) / 2 * scale;
//        dx = Math.Min(Math.Max(dx, -maxDx), maxDx);
//        dy = Math.Min(Math.Max(dy, -maxDy), maxDy);
//        Log.Debug(TAG, "Width: " + child().Width + ", scale " + scale + ", dx " + dx + ", max " + maxDx);
//        applyScaleAndTranslation();
//    }
//    return true;
//}
//ScaleGestureDetector scaleDetector;
//public bool OnScale(ScaleGestureDetector detector)
//{
//    float scaleFactor = scaleDetector.ScaleFactor;
//    Log.Debug(TAG, "onScale" + scaleFactor);
//    if (lastScaleFactor == 0 || (Math.Sign(scaleFactor) == Math.Sign(lastScaleFactor)))
//    {
//        scale *= scaleFactor;
//        scale = Math.Max(MIN_ZOOM, Math.Min(scale, MAX_ZOOM));
//        lastScaleFactor = scaleFactor;                
//    }
//    else
//    {
//        lastScaleFactor = 0;
//    }
//    return true;
//}        
//public bool OnScaleBegin(ScaleGestureDetector detector)
//{
//    Log.Debug(TAG, "onScaleBegin");
//    return true;
//}
//public void OnScaleEnd(ScaleGestureDetector detector)
//{
//    Log.Debug(TAG, "onScaleEnd");
//}
//Scrollview = FindViewById<ScrollView>(Resource.Id.Scrollview);
//Scrollview.ViewTreeObserver.AddOnScrollChangedListener(this);
//< meta name = 'viewport' content = 'height = 200%, width = 100%, initial-scale=1.0, maximum-scale = 5.0, minimum-scale = 0.5, user-scalable = yes' />
//scaleDetector = new ScaleGestureDetector(this, this);
//ScrollView Scrollview;
//private static String TAG = "ZoomLayout";
//private static float MIN_ZOOM = 0.5f;
//private static float MAX_ZOOM = 5.0f;

//private float scale = 1.0f;
//private float lastScaleFactor = 0f;

//// Where the finger first  touches the screen
//private float startX = 0f;
//private float startY = 0f;

//// How much to translate the canvas
//private float dx = 0f;
//private float dy = 0f;
//private float prevDx = 0f;
//private float prevDy = 0f;
//, ViewTreeObserver.IOnScrollChangedListener, ScaleGestureDetector.IOnScaleGestureListener
//private void applyScaleAndTranslation()
//    {
//        child().ScaleX = (scale);
//        child().ScaleY = (scale);
//        child().TranslationX = (dx);
//        child().TranslationY = (dy);
//    }
//    private View child()
//    {
//        return objwebview;
//    }