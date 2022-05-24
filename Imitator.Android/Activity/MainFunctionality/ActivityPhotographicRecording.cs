using System;
using System.IO;
using Android;
using Android.App;
using Android.Content.PM;
using Android.Graphics;
using Android.Media;
using Android.OS;
using URI = Android.Net.Uri;
using Compat = Android.Support.V4.App.ActivityCompat;
using Android.Support.V4.Content;
using Android.Views;
using Android.Widget;
using Imitator.CommonData.DataModels;
using Android.Runtime;
using Android.Content;
using Android.Provider;
using Android.Util;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using Imitator.CommonData.ViewModels.Responses;
using Imitator.Android.Helpers;

namespace Imitator.Android.Activity.MainFunctionality
{
    [Obsolete]
    public class ActivityPhotographicRecording : Fragment
    {
        private ImageView photoView;
        private int EvenRowStyle = Resource.Style.EvenRow;

        static readonly int REQUEST_CAMERA_WriteExternalStorage = 0;

        static string filepath = "/storage/emulated/0/Pictures/";

        private int OddRowStyle = Resource.Style.OddRow;
        private int MainRowStyle = Resource.Style.MainRow;
        private int RowsCount;
        private int ColumnCount;

        private static string boxState = (StaticBox.Sensors["Состояние контейнера"] == "0") ? "сложен" : "разложен";
        private static string doorState = (StaticBox.Sensors["Состояние дверей"] == "0") ? "закрыта" : "открыта";

        private string[,] RowName = {
            { "№ п/п","    Значение датчика", "Датчик"},
            { "1", StaticBox.Sensors["Вес груза"], "Вес груза,кг"},
            { "2",StaticBox.Sensors["Температура"], "Температура,°C"},
            { "3", StaticBox.Sensors["Влажность"],"Влажность,%"},
            { "4",StaticBox.Sensors["Освещенность"], "Освещенность,лм"},
            { "5",StaticBox.Sensors["Уровень заряда аккумулятора"], "Батарея,V"},
            { "6",StaticBox.Sensors["Уровень сигнала"], "Сигнал сети,дБ"},
            { "7",StaticBox.Sensors["Местоположение контейнера"], "Положение"},
            { "8",boxState, "Контейнер"},
            { "9",doorState, "Роллета"},
            { "10",StaticBox.Longitude.ToString(), "Долгота"},
            { "11",StaticBox.Latitude.ToString(), "Широта"},
            { "12",StaticBox.CreatedAtSensors.ToString(), "Дата/Время"}
        };
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            //RowName = new Dictionary<string, string>(12);
            //RowName.Add("1", "Вес груза");
            //RowName.Add("2", "Температура");
            //RowName.Add("3", "Влажность");
            //RowName.Add("4", "Свет");
            //RowName.Add("5", "Уровень батареи");
            //RowName.Add("6", "Сигнал сети");
            //RowName.Add("7", "Положение");
            //RowName.Add("8", "Контейнер");
            //RowName.Add("9", "Дверь");
            //RowName.Add("10", "Долгота");
            //RowName.Add("11", "Широта");
            //RowName.Add("12", "Дата/Время");


            RowsCount = 13;
            ColumnCount = 3;
        }

    
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.PagePhotographicRecording, container, false);

            var tableLayout = view.FindViewById<TableLayout>(Resource.Id.SensorsValueTable);
            photoView = view.FindViewById<ImageView>(Resource.Id.photobox);

            //s_open_close_container_1.Text = (StaticBox.Sensors["Состояние контейнера"] == "0") ? "сложен" : "разложен";
            //s_lock_unlock_door_1.Text = (StaticBox.Sensors["Состояние дверей"] == "0") ? "закрыта" : "открыта";

            if (StaticBox.CameraOpenOrNo == 1)
            {
                CheckPermission();
            }

            for (int i = 0; i < RowsCount; i++)
            {
                TableRow tableRow = new TableRow(new ContextThemeWrapper(Activity, EvenRowStyle));

                if (i == 0)
                {
                    tableRow = new TableRow(new ContextThemeWrapper(Activity, MainRowStyle));
                }
                else
                {
                    tableRow = new TableRow(new ContextThemeWrapper(Activity, EvenRowStyle));
                }


                //TableRow tableRow = new TableRow(Activity);

                //tableRow.LayoutParameters = new TableRow.LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent);
                //tableRow.SetBackgroundColor(Color.Rgb(25, 191, 126));

                //tableRow.setLayoutParams(new LayoutParams(LayoutParams.MATCH_PARENT,
                //        LayoutParams.WRAP_CONTENT));
                //tableRow.setBackgroundResource(R.drawable.shelf);

                for (int j = 0; j < ColumnCount; j++)
                {
                    TextView text = new TextView(new ContextThemeWrapper(Activity, Resource.Style.InTableTextStyle));
                    if (j == 0)
                    {
                        text.SetBackgroundResource(Resource.Drawable.EditTextStyle);
                    }
                    text.Text = RowName[i, j];
                    tableRow.AddView(text, j);
                }

                tableLayout.AddView(tableRow, i);
            }

            return view;
        }

        Bitmap loadAndResizeBitmap(string filePath)
        {
            BitmapFactory.Options options = new BitmapFactory.Options { InJustDecodeBounds = true };
            BitmapFactory.DecodeFile(filePath, options);

            int REQUIRED_SIZE = 100;
            int width_tmp = options.OutWidth, height_tmp = options.OutHeight;
            int scale = 4;
            while (true)
            {
                if (width_tmp / 2 < REQUIRED_SIZE || height_tmp / 2 < REQUIRED_SIZE)
                    break;
                width_tmp /= 2;
                height_tmp /= 2;
                scale++;
            }

            options.InSampleSize = scale;
            options.InJustDecodeBounds = false;
            //failed
            Bitmap resizedBitmap = BitmapFactory.DecodeFile(filePath, options);

            ExifInterface exif = null;
            try
            {
                //failed
                exif = new ExifInterface(filePath);
                string orientation = exif.GetAttribute(ExifInterface.TagOrientation);

                Matrix matrix = new Matrix();
                switch (orientation)
                {
                    case "1": // landscape
                        break;
                    case "3":
                        matrix.PreRotate(180);
                        resizedBitmap = Bitmap.CreateBitmap(resizedBitmap, 0, 0, resizedBitmap.Width, resizedBitmap.Height, matrix, false);
                        matrix.Dispose();
                        matrix = null;
                        break;
                    case "4":
                        matrix.PreRotate(180);
                        resizedBitmap = Bitmap.CreateBitmap(resizedBitmap, 0, 0, resizedBitmap.Width, resizedBitmap.Height, matrix, false);
                        matrix.Dispose();
                        matrix = null;
                        break;
                    case "5":
                        matrix.PreRotate(90);
                        resizedBitmap = Bitmap.CreateBitmap(resizedBitmap, 0, 0, resizedBitmap.Width, resizedBitmap.Height, matrix, false);
                        matrix.Dispose();
                        matrix = null;
                        break;
                    case "6": // portrait
                        matrix.PreRotate(90);
                        resizedBitmap = Bitmap.CreateBitmap(resizedBitmap, 0, 0, resizedBitmap.Width, resizedBitmap.Height, matrix, false);
                        matrix.Dispose();
                        matrix = null;
                        break;
                    case "7":
                        matrix.PreRotate(-90);
                        resizedBitmap = Bitmap.CreateBitmap(resizedBitmap, 0, 0, resizedBitmap.Width, resizedBitmap.Height, matrix, false);
                        matrix.Dispose();
                        matrix = null;
                        break;
                    case "8":
                        matrix.PreRotate(-90);
                        resizedBitmap = Bitmap.CreateBitmap(resizedBitmap, 0, 0, resizedBitmap.Width, resizedBitmap.Height, matrix, false);
                        matrix.Dispose();
                        matrix = null;
                        break;
                }

                return resizedBitmap;
            }

            catch (IOException ex)
            {
                Console.WriteLine("An exception was thrown when reading exif from media file...:" + ex.Message);
                return null;
            }
        }

        public void CheckPermission()
        {
            if ((ContextCompat.CheckSelfPermission(Activity, Manifest.Permission.Camera) == (int)Permission.Granted) && (ContextCompat.CheckSelfPermission(Activity, Manifest.Permission.WriteExternalStorage) == (int)Permission.Granted))
            {
                // Camera and store permission has  been granted
                takePicture();
            }
            else
            {
                // Camera and store permission has not been granted
                RequestPermission();
            }

        }

        private void RequestPermission()
        {
            Compat.RequestPermissions(Activity, new System.String[] { Manifest.Permission.Camera, Manifest.Permission.WriteExternalStorage }, REQUEST_CAMERA_WriteExternalStorage);
        }


        //get result of persmissions
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            if (requestCode == REQUEST_CAMERA_WriteExternalStorage)
            {


                if (PermissionUtil.VerifyPermissions(grantResults))
                {
                    // All required permissions have been granted, display Camera.

                    takePicture();

                }
                else
                {
                    // permissions did not grant, push up a snackbar to notificate USERS
                    //Snackbar.Make(layout,"Permissions were not granted", Snackbar.LengthShort).Show();
                }

            }
            else
            {
                base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            }
        }

        //private void ShowCamera()
        //{
        //    Intent picker = new Intent(MediaStore.ActionImageCapture);
        //    DateTime now = DateTime.Now;

        //    var intent = picker.(new StoreCameraMediaOptions
        //    {
        //        Name = "picture_" + now.Day + "_" + now.Month + "_" + now.Year + ".jpg",
        //        Directory = null
        //    });
        //    StartActivityForResult(intent, 0);

        //}
        void takePicture()
        {

            Intent intent = new Intent(MediaStore.ActionImageCapture);
            StartActivityForResult(intent, 1);
        }

        //protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        //{
        //    base.OnActivityResult(requestCode, resultCode, data);

        //    if (requestCode == 1)
        //    {
        //        if (resultCode == Result.Ok)
        //        {
        //            data.GetMediaFileExtraAsync(this).ContinueWith(t =>
        //            {
        //                using (Android.Graphics.Bitmap bmp = loadAndResizeBitmap(t.Result.Path))
        //                {
        //                    StaticBox.CameraOpenOrNo = 0;
        //                    if (bmp != null)
        //                        photobox.SetImageBitmap(bmp);
        //                }

        //            }, TaskScheduler.FromCurrentSynchronizationContext());
        //        }
        //    }
        //}
        public override async void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            Bitmap bitmap = null;
            StaticBox.CameraOpenOrNo = 0;
            //If user did not take a photeo , he will get result of bitmap, it is null
            try
            {
                bitmap = (Bitmap)data.Extras.Get("data");

            }
            catch (System.Exception e)
            {
                Log.Error("TakePhotoDemo1", e.Message);
                Toast.MakeText(Activity, "Не удалось загрузить фото", ToastLength.Short).Show();
            }

            if (bitmap != null)
            {
                byte[] bitmapData;
                var stream = new MemoryStream();
                bitmap.Compress(Bitmap.CompressFormat.Png, 0, stream);
                bitmapData = stream.ToArray();
                var fileContent = new ByteArrayContent(bitmapData);
                fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("application/octet-stream");
                fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                {
                    Name = "file",
                    FileName = StaticBox.IMEI + "." + DateTime.Now + ".jpg"
                };
                string boundary = "---8d0f01e6b3b5dafaaadaad";
                MultipartFormDataContent multipartContent = new MultipartFormDataContent(boundary);
                multipartContent.Add(fileContent);

                try
                {
                    HttpClient httpClient = new HttpClient();
                    var uri = new System.Uri("http://smartboxcity.ru:8003/imitator/media?file=" + bitmap);
                    HttpResponseMessage response = await httpClient.PostAsync(uri.ToString(), multipartContent);
                    if (response.IsSuccessStatusCode)
                    {
                        string content = await response.Content.ReadAsStringAsync();
                        AuthApiData<BaseResponseObject> result = new AuthApiData<BaseResponseObject>();
                        result = JsonConvert.DeserializeObject<AuthApiData<BaseResponseObject>>(content);
                        Toast.MakeText(Activity, result.Message, ToastLength.Short).Show();
                    }
                }
                catch (System.Exception e)
                {
                    Toast.MakeText(Activity, "Не удалось загрузить фото на сервер. " + e.Message, ToastLength.Short).Show();
                }

                //var myHttpClient = new HttpClient();
                //
                //HttpResponseMessage responseFromAnotherServer = await myHttpClient.PostAsync(uri.ToString(), bitmap);
                //string s_result_from_another_server;
                //using (HttpContent responseContent = responseFromAnotherServer.Content)
                //{
                //    s_result_from_another_server = await responseContent.ReadAsStringAsync();
                //}
                var basePath = Application.Context.GetExternalFilesDir(null).AbsolutePath;
                var path = MediaStore.Images.Media.InsertImage(Context.ContentResolver, bitmap, "screen", "shot");
                if (path != null)
                {
                    var uriPath = URI.Parse(path);
                    var realPath = GetRealPathFromURI(uriPath);
                    bitmap = loadAndResizeBitmap(realPath);
                    photoView.SetImageBitmap(bitmap);
                }
                else
                {
                    Toast.MakeText(Activity, "Не удалось загрузить фото. Скорее всего, нет разрешеия на доступ к галерее", ToastLength.Short).Show();
                }

            }
            else
            {
                Toast.MakeText(Activity, "Не удалось загрузить фото", ToastLength.Short).Show();
            }

        }
        private string GetRealPathFromURI(URI uri)
        {
            string doc_id = "";
            using (var c1 = Context.ContentResolver.Query(uri, null, null, null, null))
            {
                c1.MoveToFirst();
                string document_id = c1.GetString(0);
                doc_id = document_id.Substring(document_id.LastIndexOf(":") + 1);
            }

            string path = null;

            // The projection contains the columns we want to return in our query.
            string selection = MediaStore.Images.Media.InterfaceConsts.Id + " =? ";
            using (var cursor = Context.ContentResolver.Query(MediaStore.Images.Media.ExternalContentUri, null, selection, new string[] { doc_id }, null))
            {
                if (cursor == null) return path;
                var columnIndex = cursor.GetColumnIndexOrThrow(MediaStore.Images.Media.InterfaceConsts.Data);
                cursor.MoveToFirst();
                path = cursor.GetString(columnIndex);
            }
            return path;
        }
    }
}