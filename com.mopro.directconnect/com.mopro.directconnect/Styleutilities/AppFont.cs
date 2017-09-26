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
using Android.Util;

namespace com.mopro.directconnect
{
    //Application Font initialization and Used for anywhere inside the application
    public class AppFont
    {
        public static Typeface GetText(Context c)
        {
            Typeface Font = Typeface.CreateFromAsset(c.Assets, "Fonts/OpenSansRegular.ttf");
            return Font;
        }
        public static Typeface GetButton(Context c)
        {
            Typeface Font = Typeface.CreateFromAsset(c.Assets, "Fonts/ProximaNovaSemibold.otf");
            return Font;
        }
        public static Typeface GetTitle(Context c)
        {
            Typeface Font = Typeface.CreateFromAsset(c.Assets, "Fonts/ProximaNovaSemibold.otf");
           // Typeface Font = Typeface.CreateFromAsset(c.Assets, "Fonts/ProximaNovaBold.otf");
            return Font;
        }       
    }
    public class HeadingText : TextView
    {
        private const string Tag = "TextView";

        protected HeadingText(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
        }

        public HeadingText(Context context)
            : this(context, null)
        {
        }

        public HeadingText(Context context, IAttributeSet attrs)
            : this(context, attrs, 0)
        {
        }

        public HeadingText(Context context, IAttributeSet attrs, int defStyle)
            : base(context, attrs, defStyle)
        {
            var a = context.ObtainStyledAttributes(attrs, Resource.Styleable.CustomFonts);
            var customFont = a.GetString(Resource.Styleable.CustomFonts_customFont);
            SetCustomFont();
            a.Recycle();
        }

        public void SetCustomFont()
        {
            Typeface tf;
            try
            {
                tf = Typeface.CreateFromAsset(Context.Assets, "Font/ProximaNovaSemibold.otf");
            }
            catch (Exception e)
            {
                Log.Error(Tag, string.Format("Could not get Typeface: {0} Error: {1}", "", e));
                return;
            }

            if (null == tf) return;

            var tfStyle = TypefaceStyle.Normal;
            if (null != Typeface) //Takes care of android:textStyle=""
                tfStyle = Typeface.Style;
            SetTypeface(tf, tfStyle);
        }
    }
}