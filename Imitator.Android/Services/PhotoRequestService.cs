using Android.App;
using Android.Content;
using Android.Support.V4.App;
using Android.Util;
using Android.Widget;
using Firebase.JobDispatcher;
using Imitator.CommonData.DataModels;
using Imitator.WebServices;
using Imitator.WebServices.Device;
using System;
using System.Threading.Tasks;

namespace Imitator.Android.Services
{
    [Service(Name = "com.xamarin.fjdtestapp.DemoJob")]
    [IntentFilter(new[] { FirebaseJobServiceIntent.Action })]
    public class PhotoRequestService: JobService
    {
        static readonly string TAG = "X:DemoService";

        public override void OnCreate()
        {
            base.OnCreate();
        }


        public override bool OnStartJob(IJobParameters jobParameters)
        {
            Task.Run(async () =>
            {
                await SearchPhotoRequest();


                if (StaticBox.IsAvalableRequest == "OK")
                {
                    // Create PendingIntent
                    StaticBox.CameraOpenOrNo = 1;
                    StaticBox.IsAvalableRequest = "1";

                    //FragmentTransaction transaction = Context.FragmentManager.BeginTransaction();
                    //ActivityPhotographicRecording ContentPhotographicRecording = new ActivityPhotographicRecording();
                    //transaction.Replace(Resource.Id.framelayoutFormMainFunctionality, ContentPhotographicRecording).AddToBackStack(null).Commit();
                    //PendingIntent resultPendingIntent = PendingIntent.GetActivity(this, 0, camera,
                    //PendingIntentFlags.UpdateCurrent);

                    //Create Notification
                    NotificationCompat.Builder builder = new NotificationCompat.Builder(Application.Context, "Channel")
                        .SetSmallIcon(Resource.Mipmap.ic_launcher)
                    .SetContentTitle("Запрос клиента на получение фото.")
                    .SetContentText("Сделать фото")
                    .SetAutoCancel(true)
                    .SetVibrate(new long[] { 1000, 1000 });

                    //Show Notification
                    Notification notification = builder.Build();
                    NotificationManager notificationManager = Application.Context.GetSystemService(Context.NotificationService) as NotificationManager;
                    notificationManager.Notify(1, notification);
                }
                JobFinished(jobParameters, true);
            });

            // Return true because of the asynchronous work
            return true;
        }

        private async Task SearchPhotoRequest()
        {
            try
            {
                using (var client = ClientHelper.GetClient(StaticUser.Token))
                {
                    SensorsService.InitializeClient(client);
                    var o_data = await SensorsService.SearchPhotoRequest(StaticBox.DeviceId);

                    if (o_data.Result.ToString() == "OK")
                    {
                        StaticBox.IsAvalableRequest = o_data.Result.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Debug(TAG, ex.Message);
                Toast.MakeText(Application.Context, TAG + ex.Message, ToastLength.Short).Show();
            }

        }

        public override bool OnStopJob(IJobParameters jobParameters)
        {
            Log.Debug(TAG, "DemoJob::OnStartJob");
            // nothing to do.
            return true;
        }
    }
}