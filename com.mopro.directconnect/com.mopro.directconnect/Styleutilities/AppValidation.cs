using System;
using System.Text.RegularExpressions;
using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Widget;
using Android.Views;
using Android.Util;

namespace com.mopro.directconnect
{
    public static class AppValidation
    {
        public static float PointOfView(View view)
        {
            int[] location = new int[2];
            view.GetLocationInWindow(location);
            return location[1];
        }
        public static int ToPixels(Activity mContext, int value)
        {
            int pixel = (int)TypedValue.ApplyDimension(ComplexUnitType.Dip, value, mContext.Resources.DisplayMetrics);
            return pixel;
        }
        // convertToPixels(120)
        public static Drawable ErrorIcon { get; set; }
        //Validate the Name weather is null or empty and validate is correct or not
        public static bool NameValidation(EditText objName)
		{
			if (String.IsNullOrWhiteSpace(objName.Text))
			{
				//objName.SetError("Enter Name", ErrorIcon);
				objName.RequestFocus();
                objName.SetBackgroundResource(Resource.Drawable.EdittextError);
                objName.SetHintTextColor(Color.Red);
                return false;
			}
			if (objName.Text.Length <= 2)
			{
				//objName.SetError("Enter Valid Name", ErrorIcon);
				objName.RequestFocus();
                objName.SetBackgroundResource(Resource.Drawable.EdittextError);
                objName.SetHintTextColor(Color.Red);
                return false;
			}
			return true;
        }
        //Validate the Text weather is null or empty and validate is correct or not
        public static bool TextValidation(EditText objName, String Value)
		{
			if (String.IsNullOrWhiteSpace(objName.Text))
			{
				//objName.SetError(Value, ErrorIcon);
				objName.RequestFocus();
                objName.SetBackgroundResource(Resource.Drawable.EdittextError);
                objName.SetHintTextColor(Color.Red);
                return false;
			}
			if (objName.Text.Length <= 2)
			{
				//objName.SetError(Value, ErrorIcon);
				objName.RequestFocus();
                objName.SetBackgroundResource(Resource.Drawable.EdittextError);
                objName.SetHintTextColor(Color.Red);
                return false;
			}
			return true;
        }
        //Validate the Number weather is null or empty and validate is correct or not
        public static bool NumberValidation(EditText objName, String Value)
		{
			if (String.IsNullOrWhiteSpace(objName.Text))
			{
				//objName.SetError(Value, ErrorIcon);
				objName.RequestFocus();
                objName.SetBackgroundResource(Resource.Drawable.EdittextError);
                objName.SetHintTextColor(Color.Red);
                return false;
			}
			if (objName.Text.Length <= 0)
			{
				//objName.SetError(Value, ErrorIcon);
				objName.RequestFocus();
                objName.SetBackgroundResource(Resource.Drawable.EdittextError);
                objName.SetHintTextColor(Color.Red);
                return false;
			}
			return true;
        }
        //Validate the Year weather is null or empty and validate is correct or not
        public static bool YearValidation(EditText objName, String Value)
		{
			if (String.IsNullOrWhiteSpace(objName.Text))
			{
				//objName.SetError(Value, ErrorIcon);
				objName.RequestFocus();
                objName.SetBackgroundResource(Resource.Drawable.EdittextError);
                objName.SetHintTextColor(Color.Red);
                return false;
			}
			return true;
        }
        //Validate the Month weather is null or empty and validate is correct or not
        public static bool MonthValidation(EditText objName, String Value)
		{
			if (String.IsNullOrWhiteSpace(objName.Text))
			{
				//objName.SetError(Value, ErrorIcon);
				objName.RequestFocus();
                objName.SetBackgroundResource(Resource.Drawable.EdittextError);
                objName.SetHintTextColor(Color.Red);
                return false;
			}
			if (int.Parse(objName.Text) >= 12)
			{
				//objName.SetError(Value, ErrorIcon);
				objName.RequestFocus();
                objName.SetBackgroundResource(Resource.Drawable.EdittextError);
                objName.SetHintTextColor(Color.Red);
                return false;
			}
			return true;
        }
        //Validate the Password weather is null or empty and validate is correct or not
        public static bool PasswordValidation(EditText objPassword1, EditText objPassword2)
		{
			if (String.IsNullOrWhiteSpace(objPassword1.Text) || String.IsNullOrEmpty(objPassword1.Text))
			{
				//objPassword1.SetError("Enter Password", ErrorIcon);
				objPassword1.RequestFocus();
                objPassword1.SetBackgroundResource(Resource.Drawable.EdittextError);
                objPassword1.SetHintTextColor(Color.Red);
                return false;
			}
			if (String.IsNullOrWhiteSpace(objPassword2.Text) || String.IsNullOrEmpty(objPassword2.Text))
			{
				//objPassword2.SetError("Enter Password", ErrorIcon);
				objPassword2.RequestFocus();
                objPassword2.SetBackgroundResource(Resource.Drawable.EdittextError);
                objPassword2.SetHintTextColor(Color.Red);
                return false;
			}
			if (objPassword1.Text.Length <= 3 || objPassword2.Text.Length <= 3)
			{
				//objPassword1.SetError("Minimum Lenght 8", ErrorIcon);
				//objPassword2.SetError("Minimum Lenght 8", ErrorIcon);
				objPassword2.RequestFocus();
                objPassword2.SetBackgroundResource(Resource.Drawable.EdittextError);
                objPassword2.SetHintTextColor(Color.Red);
                return false;
			}
			if (objPassword1.Text != objPassword2.Text)
			{
				//objPassword1.SetError("Password and Confirm Password are Mismatched", ErrorIcon);
				//objPassword2.SetError("Password and Confirm Password are Mismatched", ErrorIcon);
				objPassword2.RequestFocus();
                objPassword2.SetBackgroundResource(Resource.Drawable.EdittextError);
                objPassword2.SetHintTextColor(Color.Red);
                return false;
			}
			return true;
        }
        //Validate the Email weather is null or empty and validate is correct or not
        public static bool EmailValidation(EditText objEmail)
		{
			if (String.IsNullOrWhiteSpace(objEmail.Text))
			{
				//objEmail.SetError("Enter Email", ErrorIcon);
				objEmail.RequestFocus();
                objEmail.SetBackgroundResource(Resource.Drawable.EdittextError);
                objEmail.SetHintTextColor(Color.Red);
                return false;
			}
			if (!Android.Util.Patterns.EmailAddress.Matcher(objEmail.Text.ToString()).Matches())
			{
				//objEmail.SetError("Enter Valid Email", ErrorIcon);
				objEmail.RequestFocus();
                objEmail.SetBackgroundResource(Resource.Drawable.EdittextError);
                objEmail.SetHintTextColor(Color.Red);
                return false;
			}
			return true;
        }
        //Validate the Mobile weather is null or empty and validate is correct or not
        public static bool MobileValidation(EditText objMobile)
		{
			if (String.IsNullOrWhiteSpace(objMobile.Text))
			{
				//objMobile.SetError("Enter MobileNumber", ErrorIcon);
				objMobile.RequestFocus();
                objMobile.SetBackgroundResource(Resource.Drawable.EdittextError);
                objMobile.SetHintTextColor(Color.Red);
                return false;
			}
			if (!Android.Util.Patterns.Phone.Matcher(objMobile.Text.ToString()).Matches() || objMobile.Text.Length != 10)
			{
				//objMobile.SetError("Enter Valid MobileNumber", ErrorIcon);
				objMobile.RequestFocus();
                objMobile.SetBackgroundResource(Resource.Drawable.EdittextError);
                objMobile.SetHintTextColor(Color.Red);
                return false;
			}
			return true;
		}
        public static void ListViewHeight(ListView listviewv, int Count, IListAdapter objbaseadapter)
        {
            int TotalHeight = 0;
            for (int i = 0; i < Count; i++)
            {
                View listItem = objbaseadapter.GetView(i, null, listviewv);
                listItem.Measure(0, 0);
                TotalHeight += listItem.MeasuredHeight;
            }
            ViewGroup.LayoutParams LayoutParams = listviewv.LayoutParameters;
            LayoutParams.Height = TotalHeight + 40 + (listviewv.DividerHeight * (objbaseadapter.Count - 1));
            listviewv.LayoutParameters = LayoutParams;
            listviewv.RequestLayout();
        }
    }

    public class EditTextTouch : Java.Lang.Object, View.IOnTouchListener
    {
        public bool OnTouch(View v, MotionEvent e)
        {
            v.Parent.RequestDisallowInterceptTouchEvent(true);
            switch (e.Action & MotionEventActions.Mask)
            {
                case MotionEventActions.Up:
                    v.Parent.RequestDisallowInterceptTouchEvent(false);
                    break;
            }
            return false;
        }
    }

}
