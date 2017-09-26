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
using Android.Provider;
using static Android.Provider.MediaStore;
using Android.Database;
using Android.Graphics;
using FFImageLoading;
using FFImageLoading.Views;
using System.Net;
using System.Threading.Tasks;
using System.IO;
using Android.Media;
using Android.Content.PM;

namespace com.mopro.directconnect
{
    [Activity(Label = "Viewimages", ScreenOrientation = ScreenOrientation.Portrait)]
    public class Viewimages : Activity
    {
        ImageHandler ObjImageHandler;
        List<String> objImages = new List<String>();
        List<long> objImagesid = new List<long>();
        ExpandableHeightGridView gallery;
        string Screenid;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Viewimages);
            Window.SetSoftInputMode(SoftInput.AdjustResize | SoftInput.StateHidden);
            ObjImageHandler = new ImageHandler(this);
            Screenid = Intent.GetStringExtra("Screen");
            // Click Back button Events Occurs below
            ImageView Backbutton = FindViewById<ImageView>(Resource.Id.Backbutton);
            Backbutton.Click += (o, e) => {
                PressBackbutton();
            };
            TextView headingtxt = FindViewById<TextView>(Resource.Id.headingtext);
            headingtxt.SetTypeface(AppFont.GetTitle(this), TypefaceStyle.Normal);
            ImageView Capturebutton = FindViewById<ImageView>(Resource.Id.Capturebutton);
            Capturebutton.SetImageResource(Resource.Drawable.camera);
            Capturebutton.Click += (o, e) => {
                ObjImageHandler.CameraAction();
            };
            getAllShownImagesPath(this);
            gallery = FindViewById<ExpandableHeightGridView>(Resource.Id.galleryGridView);
            gallery.FastScrollEnabled = (true);
            gallery.ScrollingCacheEnabled = (false);
            Loadimages();
        }
        private void PressBackbutton()
        {
            if (String.IsNullOrEmpty(Screenid))
            {
                StartActivity(new Intent(this, typeof(Findimage)));
                OverridePendingTransition(Resource.Drawable.slide_from_left, Resource.Drawable.slide_to_right);
            } else if(Screenid == "Design")  {

                StartActivity(new Intent(this, typeof(Designchanges)));
                OverridePendingTransition(Resource.Drawable.slide_from_left, Resource.Drawable.slide_to_right);
            }
            else if (Screenid == "Support")
            {
                StartActivity(new Intent(this, typeof(SupportRequest)));
                OverridePendingTransition(Resource.Drawable.slide_from_left, Resource.Drawable.slide_to_right);
            }
        }
        public async void Loadimages()
        {
            this.RunOnUiThread(()=>
            {
                gallery.Adapter = new ImageAdapter(this, objImages, objImagesid);
                gallery.ItemClick += Gallery_ItemClick;
            });
        }
        // Click Back button Events Occurs below
        public override void OnBackPressed()
        {
            base.OnBackPressed();
            PressBackbutton();
        }
        // Shows Alert for Inavlid User Login
        void Alertpopup(String Content)
        {
            AlertDialog.Builder alertDialog = new AlertDialog.Builder(this);
            alertDialog.SetTitle(Resources.GetString(Resource.String.error));
            alertDialog.SetMessage(Content);
            alertDialog.SetPositiveButton("OK", delegate
            {
                alertDialog.Dispose();
            });
            alertDialog.Show();
        }
        private void Gallery_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            Android.Graphics.Bitmap mBitmap = null;
            mBitmap = BitmapFactory.DecodeFile(objImages[e.Position]);
            if (mBitmap != null)
            {
                Selectingimage(objImages[e.Position]);
            }
            else
            {
                Alertpopup("Inavalid Image Format. Try again.");
            }
        }
        private void Selectingimage(String Path)
        {
            if (String.IsNullOrEmpty(Screenid))
            {
                Intent objIntent = new Intent(this, typeof(Imageupdates));
                objIntent.PutExtra("image", Path);
                StartActivity(objIntent);
                OverridePendingTransition(Resource.Drawable.slide_from_left, Resource.Drawable.slide_to_right);
            }
            else if (Screenid == "Design")
            {
                Intent objIntent = new Intent(this, typeof(Designchanges));
                objIntent.PutExtra("image", Path);
                StartActivity(objIntent);
                OverridePendingTransition(Resource.Drawable.slide_from_left, Resource.Drawable.slide_to_right);
            }
            else if (Screenid == "Support")
            {
                Intent objIntent = new Intent(this, typeof(SupportRequest));
                objIntent.PutExtra("image", Path);
                StartActivity(objIntent);
                OverridePendingTransition(Resource.Drawable.slide_from_left, Resource.Drawable.slide_to_right);
            }
        }
        //This Method is used to perform Resultof actitives
        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            try
            {
                if (resultCode != Result.Canceled)
                {
                    base.OnActivityResult(requestCode, resultCode, data);
                    if (requestCode == 1 && resultCode == Result.Ok)
                    {
                        if (data != null)
                        {
                            ObjImageHandler.picUri = data.Data;
                            Selectingimage(ObjImageHandler.picUri.Path);
                        }
                        else
                        {
                            Intent mediascanintent = new Intent(Intent.ActionMediaScannerScanFile);
                            ObjImageHandler.picUri = Android.Net.Uri.FromFile(ObjImageHandler._file);
                            Selectingimage(ObjImageHandler.picUri.Path);
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
        /**
    * Getting All Images Path.
    *
    * @param activity
    *            the activity
    * @return ArrayList with images Path
    */
        private async void getAllShownImagesPath(Activity activity)
        {
            try
            {
                objImages = new List<String>();
                objImagesid = new List<long>();
                Android.Net.Uri uri;
                ICursor cursor;
                int column_index_data, column_index_folder_name;
                String absolutePathOfImage = null;
                uri = Android.Provider.MediaStore.Images.Media.ExternalContentUri;
                String[] projection = { MediaColumns.Data, MediaStore.Images.Media.InterfaceConsts.Id, MediaStore.Images.Media.InterfaceConsts.BucketDisplayName };
                cursor = activity.ContentResolver.Query(uri, projection, null, null, null);
                column_index_data = cursor.GetColumnIndexOrThrow(MediaColumns.Data);
                int column_index = cursor.GetColumnIndexOrThrow(MediaStore.Images.Media.InterfaceConsts.Id);
                column_index_folder_name = cursor.GetColumnIndexOrThrow(MediaStore.Images.Media.InterfaceConsts.BucketDisplayName);
                while (cursor.MoveToNext())
                {
                    absolutePathOfImage = cursor.GetString(column_index_data);
                    objImages.Add(absolutePathOfImage);
                    objImagesid.Add(cursor.GetLong(column_index));
                }
            } catch { }
        }
        public class ImageAdapter : BaseAdapter
        {
            private readonly Activity context;
            List<String> Menudatas = new List<String>();
            List<long> objImagesid = new List<long>();
            public ImageAdapter(Activity c, List<String> Menudatasv, List<long> objImagesidv)
            {
                context = c;
                Menudatas = Menudatasv;
                objImagesid = objImagesidv;
            }

            public override int Count
            {
                get { return Menudatas.Count; }
            }

            public override Java.Lang.Object GetItem(int position)
            {
                return position;
            }

            public override long GetItemId(int position)
            {
                return 0;
            }
            public override View GetView(int position, View convertView, ViewGroup parent)
            {
                View view = convertView;
                if (view == null)
                    view = context.LayoutInflater.Inflate(Resource.Layout.viewimageslist, null);
                ImageViewAsync objImageView = view.FindViewById<ImageViewAsync>(Resource.Id.Image);
                objImageView.SetImageResource(Resource.Color.Transparent);
                try
                { 
                    context.RunOnUiThread(() =>
                    {
                        if (!String.IsNullOrEmpty(Menudatas[position]))
                        {
                            ImageService.Instance.LoadFile(Menudatas[position])
                   .Retry(2, 200)
                   .DownSample(context.Resources.DisplayMetrics.WidthPixels / 3, context.Resources.DisplayMetrics.WidthPixels / 3)
                   .LoadingPlaceholder("article_placeholder", FFImageLoading.Work.ImageSource.ApplicationBundle)
                   .ErrorPlaceholder("article_placeholder", FFImageLoading.Work.ImageSource.ApplicationBundle)
                   .IntoAsync(objImageView);
                            //.Into(objImageView);

                        }
                    });
                } catch { }
                return view;
            }
        }
    }

}

//         ImageService.Instance.LoadUrl("https://static.pexels.com/photos/60597/dahlia-red-blossom-bloom-60597.jpeg")
//.Retry(2, 200)
//.DownSample(context.Resources.DisplayMetrics.WidthPixels / 3, context.Resources.DisplayMetrics.WidthPixels / 3)
//.LoadingPlaceholder("article_placeholder", FFImageLoading.Work.ImageSource.ApplicationBundle)
//.ErrorPlaceholder("article_placeholder", FFImageLoading.Work.ImageSource.ApplicationBundle)
//.Into(objImageView);
//objImageView.SetImageURI(Android.Net.Uri.Parse(Menudatas[position]));

//< FFImageLoading.Views.ImageViewAsync
//    android: id = "@+id/Image"
//      android: scaleType = "matrix"
//      android: layout_width = "match_parent"
//      android: layout_height = "110dp" />