using System;
using System.Collections.Generic;
using System.IO;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.OS;
using Android.Provider;
using Android.Database;
using Android.Widget;
using Android.Media;
using Android.Runtime;

namespace com.mopro.directconnect
{
	public class ImageHandler
    {
        public static int LOW_DPI_STATUS_BAR_HEIGHT = 6;
        public static int MEDIUM_DPI_STATUS_BAR_HEIGHT = 12;
        public static int HIGH_DPI_STATUS_BAR_HEIGHT = 24;
        Activity Context;
		public Java.IO.File _dir, _file;
		public ImageHandler(Activity Contextv)
		{
			Context = Contextv;
			CreateDirectoryForPictures();
		}        
		public Android.Net.Uri picUri;
		//This Method is used to collecting all images
		public void GalleryAction()
		{
			try
			{
				var imageIntent = new Intent(Intent.ActionPick, Android.Provider.MediaStore.Images.Media.ExternalContentUri);
				imageIntent.SetType("image/*");
                _file = new Java.IO.File(_dir, String.Format("File_{0}.jpg", Guid.NewGuid()));
                imageIntent.PutExtra(MediaStore.ExtraOutput, Android.Net.Uri.FromFile(_file));
                Context.StartActivityForResult(imageIntent, 0);
			}
			catch
			{
			}
		}
        public string GetPathToImage(Android.Net.Uri uri)
        {
            ICursor cursor = Context.ContentResolver.Query(uri, null, null, null, null);
            cursor.MoveToFirst();
            string document_id = cursor.GetString(0);
            if(document_id.Contains(":"))
                document_id = document_id.Split(':')[1];
            cursor.Close();
            cursor = Context.ContentResolver.Query(Android.Provider.MediaStore.Images.Media.ExternalContentUri, null, MediaStore.Images.Media.InterfaceConsts.Id + " = ? ", new String[] { document_id }, null);
            cursor.MoveToFirst();
            string path = cursor.GetString(cursor.GetColumnIndex(MediaStore.Images.Media.InterfaceConsts.Data));
            cursor.Close();
            return System.IO.Path.GetFileName(path);
        }
        //This Method is used to perform crop action
        public void PerformCrop()
		{
			try
			{
				Intent cropIntent = new Intent("com.android.camera.action.CROP");
				cropIntent.SetDataAndType(picUri, "image/*");
				cropIntent.PutExtra("crop", "true");
				cropIntent.PutExtra("aspectX", 1);
				cropIntent.PutExtra("aspectY", 1);
				cropIntent.PutExtra("outputX", 180);
				cropIntent.PutExtra("outputY", 180);
				cropIntent.PutExtra("return-data", true);
				Context.StartActivityForResult(cropIntent, 2);
			}
			catch
			{
			}
		}

		private void CreateDirectoryForPictures()
		{
            _dir = new Java.IO.File(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath + "/DirectConnect/");
            //_dir = new Java.IO.File(Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryPictures), "CameraAppDemo");
			if (!_dir.Exists())
			{
				_dir.Mkdirs();
			}
		}
		//This method is used to start camera activities
		public void CameraAction()
		{
			try
			{
				Intent intent = new Intent(MediaStore.ActionImageCapture);
				_file = new Java.IO.File(_dir, String.Format("File_{0}.jpg", Guid.NewGuid()));
				intent.PutExtra(MediaStore.ExtraOutput, Android.Net.Uri.FromFile(_file));
                Context.StartActivityForResult(intent, 1);
			}
			catch (Exception ex)
			{

			}
		}
        //This method is used to start camera activities
        public void VideoAction()
        {
            try
            {
                Intent intent = new Intent(MediaStore.ActionVideoCapture);
                _file = new Java.IO.File(_dir, String.Format("File_{0}.mp4", Guid.NewGuid()));
                intent.PutExtra(MediaStore.ExtraOutput, Android.Net.Uri.FromFile(_file));
                Context.StartActivityForResult(intent, 1);
            }
            catch (Exception ex)
            {

            }
        }
        public static Bitmap ConvertBytetoBitmap(byte[] imageasbyte)
        {
            Bitmap Bitmapvalue = null;
            try
            {
                if (imageasbyte != null)
                {
                    using (var ms = new MemoryStream(imageasbyte))
                    {
                        Bitmapvalue = BitmapFactory.DecodeByteArray(imageasbyte, 0, imageasbyte.Length);
                    }
                }
            }
            catch
            {
            }
            return Bitmapvalue;
        }
        public static void AppBackgroundview(VideoView Videoview)
        {
            if (Build.VERSION.SdkInt > Build.VERSION_CODES.Kitkat)
            {
                String uriPath = "android.resource://" + Application.Context.PackageName + "/" + Resource.Drawable.bg_video;
                Android.Net.Uri uri = Android.Net.Uri.Parse(uriPath);
                Videoview.SetMediaController(null);
                Videoview.SetVideoURI(uri);
                Videoview.Completion += (sender, e) =>
                {
                    Videoview.Start();
                };
                Videoview.Start();
            }
            else
            {
                Videoview.SetVideoPath("android.resource://" + Application.Context.PackageName + "/" + Resource.Drawable.bg_video);
                Videoview.Completion += (sender, e) =>
                {
                    Videoview.Start();
                };
                Videoview.Start();
            }
        }
    public static byte[] ConvertBitmaptoByte(Bitmap Bitmapvalue)
        {
            byte[] Bytevalue = null;
            try
            {
                if (Bitmapvalue != null)
                {

                    MemoryStream bos = new MemoryStream();
                    Bitmapvalue.Compress(Bitmap.CompressFormat.Png, 0, bos);
                    Bytevalue = bos.ToArray();
                }
            }
            catch
            {
            }
            return Bytevalue;
        }
    }
}
