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
using Android.Preferences;

namespace com.mopro.directconnect
{
    public class AppPreferences
    {
        private ISharedPreferences mSharedPrefs;
        private ISharedPreferencesEditor mPrefsEditor;
        private Context mContext;
        public String username;
        public String password;

        public AppPreferences(Context context)
        {
            this.mContext = context;
            mSharedPrefs = PreferenceManager.GetDefaultSharedPreferences(mContext);
            mPrefsEditor = mSharedPrefs.Edit();
        }
        public void SaveUserdetails(string username, string password)
        {
            mPrefsEditor.PutString("username", username);
            mPrefsEditor.PutString("password", password);
            mPrefsEditor.Commit();
        }
        public bool Validateuser()
        {
             username = mSharedPrefs.GetString("username", "");
             password = mSharedPrefs.GetString("password", "");
            return (!String.IsNullOrEmpty(username) && !String.IsNullOrEmpty(password));
        }
    }
}