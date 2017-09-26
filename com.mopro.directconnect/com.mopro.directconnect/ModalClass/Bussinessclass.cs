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

namespace com.mopro.directconnect
{
    // Sample Model class for the Website List
    public class Bussinessclass
    {
        public string name { get; set; }
        public string id { get; set; }
        public string location { get; set; }
        public string website { get; set; }
    }
    public class Chatclass
    {
        public string text { get; set; }
        public string id { get; set; }
        public bool isoutgoing { get; set; }
        public string time { get; set; }
    }
    public class Buttonclass
    {
        public int id { get; set; }
        public string name { get; set; }
        public int SelectedX { get; set; }
        public int SelectedY { get; set; }
        public bool status { get; set; }
    }
}