using Android.App;
using Android.Content;
using Android.Gms.Location;
using Android.Locations;
using Android.Widget;
using Imitator.CommonData.DataModels;
using Imitator.CommonData.ViewModels;
using Imitator.WebServices;
using Imitator.WebServices.Device;
using Plugin.Settings;
using System;

namespace Imitator.Android.Services
{
    [BroadcastReceiver]
    class MyLocationBroadcastReceiver : BroadcastReceiver
    {
        public static string ACTION_PROCESS_LOCATION = "SmartBoxCity.UPDATE_LOCATION";
        GPSService _gpsService;
        public override void OnReceive(Context context, Intent intent)
        {
            if (intent != null)
            {
                string action = intent.Action;
                if (action.Equals(ACTION_PROCESS_LOCATION))
                {
                    LocationResult result = LocationResult.ExtractResult(intent);
                    if (result != null)
                    {
                        var location = result.LastLocation;

                        try
                        {
                            //when app in foreground
                            if (!StaticBox.IsStoppedGeo)
                                PostGeoData(location);
                            else
                                _gpsService = new GPSService(context);
                            _gpsService.RemoveLocation();
                        }
                        catch (Exception ex)
                        {
                            //when app is killed
                        }
                    }

                }
            }
        }

        private async void PostGeoData(Location location)
        {
            using (var client = ClientHelper.GetClient(CrossSettings.Current.GetValueOrDefault("token", "")))
            {
                PositionModel model = new PositionModel
                {
                    CurrentDate = DateTime.Now.ToString(),
                    Latitude = location.Latitude,
                    Longitude = location.Longitude,
                    DeviceId = StaticBox.DeviceId
                };

                LocationService.InitializeClient(client);
                var o_data = await LocationService.SetGPS(model);

                Toast.MakeText(Application.Context, o_data.SuccessInfo, ToastLength.Long).Show();
            }
        }
    }
}