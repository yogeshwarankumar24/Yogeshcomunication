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

namespace com.mopro.directconnect
{
    [Activity(Label = "DirectConnect", ScreenOrientation = ScreenOrientation.Portrait)]
    public class Referscreen : Activity, OnScaleChangeListener, ScaleGestureDetector.IOnScaleGestureListener, View.IOnTouchListener, ObservableWebView.OnScrollChangeListener
    {
        Button Submitbutton;
        ObservableWebView objwebview;
        RelativeLayout NewRelativeLayout;
        RelativeLayout Submitlayout, Scrollviewlayout;
        int CurrentWidth, CurrentHeight;
        private int xDelta, xDeltadup;
        private int yDelta, yDeltadup;
        List<Buttonclass> ObjButtonlist;
        List<RelativeLayout> objLayoutlist = new List<RelativeLayout>();
        int WebviewScrollx, WebviewScrollY;
        float ImageScale, ClickScalev, StartScale;
        bool Startscale;
        ScaleGestureDetector GestureDetector;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Referscreen);
            Window.SetSoftInputMode(SoftInput.StateHidden);
            TextView headingtext = FindViewById<TextView>(Resource.Id.headingtext);
            headingtext.SetTypeface(AppFont.GetTitle(this), Android.Graphics.TypefaceStyle.Normal);
            // Click Back button Events Occurs below
            ImageView Backbutton = FindViewById<ImageView>(Resource.Id.Backbutton);
            Backbutton.Click += (o, e) => {
                StartActivity(new Intent(this, typeof(Requestedit)));
                OverridePendingTransition(Resource.Drawable.slide_from_left, Resource.Drawable.slide_to_right);
            };
            ObjButtonlist = new List<Buttonclass>();
            Submitlayout = FindViewById<RelativeLayout>(Resource.Id.Submitlayout);
            Submitlayout.Visibility = ViewStates.Gone;
            objwebview = FindViewById<ObservableWebView>(Resource.Id.webview);
            CookieManager cm = CookieManager.Instance;
            cm.SetAcceptCookie(true);
            objwebview.Settings.JavaScriptEnabled = true;
            objwebview.Settings.BuiltInZoomControls = true;
            objwebview.Settings.DisplayZoomControls = false;
            objwebview.Settings.SetSupportZoom(true);
            objwebview.SetInitialScale(1);
            objwebview.setOnScrollChangeListener(this);
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
            objwebview.SetBackgroundColor(Color.White);
            objwebview.ScrollBarStyle = ScrollbarStyles.InsideOverlay;
            objwebview.SetWebViewClient(new Webviewclient(this));
            objwebview.AddJavascriptInterface(this, "app");
            Submitbutton = FindViewById<Button>(Resource.Id.Submitbutton);
            objwebview.Tag = "Web";
            GestureDetector = new ScaleGestureDetector(this, this);
            objwebview.SetOnTouchListener(this);
            TextView Biztab = FindViewById<TextView>(Resource.Id.Biztab);
            Biztab.SetTypeface(AppFont.GetText(this), Android.Graphics.TypefaceStyle.Normal);
            Submitbutton.SetTypeface(AppFont.GetButton(this), TypefaceStyle.Normal);
            Scrollviewlayout = FindViewById<RelativeLayout>(Resource.Id.Scrollviewlayout);
            WebView.LayoutParams objlayout = new AbsoluteLayout.LayoutParams(Resources.DisplayMetrics.WidthPixels + 10000, Resources.DisplayMetrics.HeightPixels + 100000, 0, 0);
            Scrollviewlayout.LayoutParameters = objlayout;

            Submitbutton.Click += (o, e) => PressSubmitButton();
            if (Build.VERSION.SdkInt < Build.VERSION_CODES.Kitkat)
            {
                AddContent(1, "1", 150, 300);
                AddContent(2, "2", 250, 450);
            }
        }
        private void PressSubmitButton()
        {
            Intent objIntent = new Intent(this, typeof(Requestedit));
            StartActivity(objIntent);
            OverridePendingTransition(Resource.Drawable.slide_from_left, Resource.Drawable.slide_to_right);
        }
        private async void AddContent(int Value, String Textname, int XLeftMargin, int XTopMargin)
        {
            try
            {
                //SelectedX = XLeftMargin;
                //SelectedY = XTopMargin;
                NewRelativeLayout = new RelativeLayout(this);
                RelativeLayout.LayoutParams RelativelayoutParams = new RelativeLayout.LayoutParams(AppValidation.ToPixels(this, 70), AppValidation.ToPixels(this, 70));
                RelativelayoutParams.LeftMargin = XLeftMargin;
                RelativelayoutParams.TopMargin = XTopMargin;
                ImageView NewImageView = new ImageView(this);
                RelativeLayout.LayoutParams ImageViewlayoutParams = new RelativeLayout.LayoutParams(AppValidation.ToPixels(this, 30), AppValidation.ToPixels(this, 30));
                ImageViewlayoutParams.LeftMargin = AppValidation.ToPixels(this, 38);
                ImageViewlayoutParams.TopMargin = AppValidation.ToPixels(this, 38);
                switch (Value)
                {
                    case 1:
                        {
                            NewImageView.SetBackgroundResource(Resource.Drawable.Imageflat);
                            break;
                        }
                    case 2:
                        {
                            NewImageView.SetBackgroundResource(Resource.Drawable.Textflat);
                            break;
                        }
                    case 3:
                        {
                            NewImageView.SetBackgroundResource(Resource.Drawable.Videoflat);
                            break;
                        }
                    case 4:
                        {
                            NewImageView.SetBackgroundResource(Resource.Drawable.Linkflat);
                            break;
                        }
                    case 5:
                        {
                            NewImageView.SetBackgroundResource(Resource.Drawable.Designflat);
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
                NewRelativeLayout.AddView(NewTextView, layoutParams);
                NewRelativeLayout.AddView(NewImageView, ImageViewlayoutParams);
                NewRelativeLayout.SetOnTouchListener(this);
                NewRelativeLayout.Tag = "false";
                NewRelativeLayout.Id = Value;
                Scrollviewlayout.AddView(NewRelativeLayout, RelativelayoutParams);
                Scrollviewlayout.BringChildToFront(NewRelativeLayout);
                objLayoutlist.Add(NewRelativeLayout);
                ObjButtonlist.Add(new Buttonclass() { id = Value, name = Textname, SelectedX = XLeftMargin, SelectedY = XTopMargin, status = false });
                //Scrollview.RequestChildFocus(NewRelativeLayout, NewRelativeLayout);
            }
            catch (Exception ex) { }
        }
        public void onScrollChange(WebView v, int scrollX, int scrollY, int oldScrollX, int oldScrollY)
        {
            WebviewScrollx = scrollX;
            WebviewScrollY = scrollY;
        }
        // Click Back button Events Occurs below
        public override void OnBackPressed()
        {
            base.OnBackPressed();
            StartActivity(new Intent(this, typeof(Requestedit)));
            OverridePendingTransition(Resource.Drawable.slide_from_left, Resource.Drawable.slide_to_right);
        }
        [JavascriptInterface]
        [Export]
        // to become consistent with Java/JS interop convention, the argument cannot be System.String.
        public void size(int width, int height)
        {
            this.RunOnUiThread(() =>
            {
                CurrentHeight = (int)(height * objwebview.Scale * Resources.DisplayMetrics.Density);
                CurrentWidth = (int)(width * objwebview.Scale * Resources.DisplayMetrics.Density);
            });
        }
        public class Webviewclient : WebViewClient
        {
            public Activity mActivity;
            //bool loadingFinished = false;
            private OnScaleChangeListener OnScaleChangeListener;
            public Webviewclient(Activity mActivity)
            {
                this.mActivity = mActivity;
                this.OnScaleChangeListener = ((OnScaleChangeListener)mActivity);
            }
            public override void OnScaleChanged(WebView view, float oldScale, float newScale)
            {
                base.OnScaleChanged(view, oldScale, newScale);
                //if(loadingFinished)
                OnScaleChangeListener.onScaleChange(view, oldScale, newScale);
            }
            public override void OnPageStarted(WebView view, string url, Bitmap favicon)
            {
                //loadingFinished = false;
                base.OnPageStarted(view, url, favicon);
            }
            public override bool ShouldOverrideUrlLoading(WebView view, string url)
            {
                //loadingFinished = false;
                return true;
            }
            public override void OnPageFinished(WebView view, string url)
            {
                base.OnPageFinished(view, url);
                //loadingFinished = true;
                OnScaleChangeListener.onLoadfinished(view);
            }
        }
        public void onScaleChange(WebView v, float oldScale, float newScale)
        {
            if (Startscale)
            {
                if (oldScale != newScale)
                {
                    ImageScale = newScale / StartScale;
                    CheckFlatoptions(ImageScale);
                }
               // Log.Debug("Scale GestureDetector", "ScaleFactor: " + newScale);
            }
        }
        void CheckFlatoptions(float Scale)
        {            
            this.RunOnUiThread(() =>
            {
                Matrix matrix = objwebview.Matrix;
                float[] pts = { 0, 0 };
                matrix.MapPoints(pts);
                for (int i = 0; i < ObjButtonlist.Count; i++)
                {
                    if (objLayoutlist[i].Tag.ToString() == "false")
                    {
                        //RelativeLayout.LayoutParams layoutParams = new RelativeLayout.LayoutParams(objLayoutlist[i].Width, objLayoutlist[i].Height);
                        RelativeLayout.LayoutParams layoutParams = (RelativeLayout.LayoutParams)objLayoutlist[i].LayoutParameters;
                        layoutParams.TopMargin = (int)((ObjButtonlist[i].SelectedY * Scale) + pts[0]);
                        layoutParams.LeftMargin = (int)((ObjButtonlist[i].SelectedX * Scale) + pts[1]);
                        objLayoutlist[i].LayoutParameters = layoutParams;
                    }
                    else
                    {
                        ClickScalev = (float)(objLayoutlist[i].Tag);
                        //RelativeLayout.LayoutParams layoutParams = new RelativeLayout.LayoutParams(objLayoutlist[i].Width, objLayoutlist[i].Height);
                        RelativeLayout.LayoutParams layoutParams = (RelativeLayout.LayoutParams)objLayoutlist[i].LayoutParameters;
                        layoutParams.TopMargin = (int)((ObjButtonlist[i].SelectedY * (Scale / ClickScalev)) - pts[0]);
                        layoutParams.LeftMargin = (int)((ObjButtonlist[i].SelectedX * (Scale / ClickScalev)) - pts[1]);
                        objLayoutlist[i].LayoutParameters = layoutParams;
                    }
                }
            });
        }
        public bool OnTouch(View view, MotionEvent evente)
        {
            if (view.Tag.ToString() == "Web")
            {
                if (!Startscale)
                {
                    Startscale = true;
                    StartScale = objwebview.Scale;
                }
                GestureDetector.OnTouchEvent(evente);
                return base.OnTouchEvent(evente);
            }
            else
            {
                int x = (int)evente.RawX;
                int y = (int)evente.RawY;
               switch (evente.Action & MotionEventActions.Mask)
                {
                    case MotionEventActions.Down:
                        RelativeLayout.LayoutParams lParams = (RelativeLayout.LayoutParams)view.LayoutParameters;
                        xDelta = x - lParams.LeftMargin;
                        yDelta = y - lParams.TopMargin;
                        view.Parent.RequestDisallowInterceptTouchEvent(true);
                        Submitlayout.Visibility = ViewStates.Gone;
                        RelativeLayout.LayoutParams layoutParams0 = new RelativeLayout.LayoutParams(objwebview.Width, objwebview.Height);
                        layoutParams0.BottomMargin = AppValidation.ToPixels(this, 0);
                        objwebview.LayoutParameters = layoutParams0;
                        break;
                    case MotionEventActions.Up:
                        view.Parent.RequestDisallowInterceptTouchEvent(false);
                        Submitlayout.Visibility = ViewStates.Visible;                        
                        RelativeLayout.LayoutParams layoutParams2 = new RelativeLayout.LayoutParams(objwebview.Width, objwebview.Height);
                        layoutParams2.BottomMargin = AppValidation.ToPixels(this, 72);
                        objwebview.LayoutParameters = layoutParams2;
                        //objwebview.LoadUrl("javascript:document.getElementById('footer').style.display = 'inline-block';");
                        break;
                    case MotionEventActions.Move:
                        {
                            xDeltadup = x - xDelta;
                            yDeltadup = y - yDelta;
                            if (xDeltadup > (-30) && yDeltadup > (-30) && ((WebviewScrollx + Resources.DisplayMetrics.WidthPixels) - xDeltadup) > AppValidation.ToPixels(this, 65) && ((WebviewScrollY + Resources.DisplayMetrics.HeightPixels) - yDeltadup) > AppValidation.ToPixels(this, 65))
                            {
                                if (ImageScale <= 1)
                                    view.Tag = "false";
                                else 
                                {
                                    view.Tag = ImageScale;
                                }
                                RelativeLayout.LayoutParams layoutParams = (RelativeLayout.LayoutParams)view.LayoutParameters;
                                layoutParams.LeftMargin = xDeltadup;
                                layoutParams.TopMargin = yDeltadup;
                                ObjButtonlist.FirstOrDefault(a => a.id == view.Id).SelectedX = xDeltadup;
                                ObjButtonlist.FirstOrDefault(a => a.id == view.Id).SelectedY = yDeltadup;
                                view.LayoutParameters = layoutParams;
                            }
                            break;
                        }
                }

                return true;
            }
        }
        public bool OnScale(ScaleGestureDetector detector)
        {
            if (Build.VERSION.SdkInt < Build.VERSION_CODES.Kitkat)
            {
                if (Startscale)
                {
                    ImageScale = objwebview.Scale;
                    CheckFlatoptions(ImageScale);
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
        }
        public void onLoadfinished(WebView v)
        {
            AddContent(1, "1", 150, 300);
            AddContent(2, "2", 250, 450);
        }
    }    
}