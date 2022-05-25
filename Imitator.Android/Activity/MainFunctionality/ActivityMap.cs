using System;
using Android.App;
using Android.Gms.Common;
using Android.Gms.Location;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.Graphics;
using Android.OS;
using Android.Util;
using Android.Views;
using Android.Widget;
using Cheesebaron.SlidingUpPanel;
using Imitator.CommonData.DataModels;
using Imitator.CommonData.ViewModels;
using Imitator.WebServices;
using Imitator.WebServices.Device;
using Plugin.Settings;

namespace Imitator.Android.Activity.MainFunctionality
{
    [Obsolete]
    public class ActivityMap : Fragment, IOnMapReadyCallback
    {
        private MapView mMapView = null;
        private static TextView txtLongitude;

        private const string SavedStateActionBarHidden = "saved_state_action_bar_hidden";

        private GoogleMap GMap;

        FusedLocationProviderClient fusedLocationProviderClient;
        LocationRequest locationRequest;
        LocationCallback locationCallback;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.PageMap, container, false);

            var layout = view.FindViewById<SlidingUpPanelLayout>(Resource.Id.sliding_layout);
            //txtLongitude = view.FindViewById<TextView>(Resource.Id.TxtLongitude);

            layout.AnchorPoint = 0.3f;
            layout.PanelExpanded += (s, e) => Log.Info(Tag, "PanelExpanded");
            layout.PanelCollapsed += (s, e) => Log.Info(Tag, "PanelCollapsed");
            layout.PanelAnchored += (s, e) => Log.Info(Tag, "PanelAnchored");
            layout.PanelSlide += (s, e) =>
            {
                if (e.SlideOffset < 0.2)
                {
                    //if (SupportActionBar.IsShowing)
                    //    SupportActionBar.Hide();
                }
                else
                {
                    //if (!SupportActionBar.IsShowing)
                    //    SupportActionBar.Show();
                }
            };


            MapsInitializer.Initialize(Activity);
            mMapView = view.FindViewById<MapView>(Resource.Id.FragmentMapUser);

            switch (GooglePlayServicesUtil.IsGooglePlayServicesAvailable(Activity))
            {
                case ConnectionResult.Success:
                    mMapView.OnCreate(savedInstanceState);
                    mMapView.GetMapAsync(this);
                    break;
                case ConnectionResult.ServiceMissing:
                    Toast.MakeText(Activity, "ServiceMissing", ToastLength.Long).Show();
                    break;
                case ConnectionResult.ServiceVersionUpdateRequired:
                    Toast.MakeText(Activity, "Update", ToastLength.Long).Show();
                    break;
                default:
                    Toast.MakeText(Activity, GooglePlayServicesUtil.IsGooglePlayServicesAvailable(Activity), ToastLength.Long).Show();
                    break;
            }

            if (StaticBox.Sensors["Местоположение контейнера"] == "На складе" || StaticBox.Sensors["Местоположение контейнера"] == "У заказчика")
            {
                AlertDialog.Builder alert = new AlertDialog.Builder(Activity);
                alert.SetTitle("Внимание !");
                alert.SetMessage("Местоположение контейнера не изменяется, так как он находится на складе или у заказчика");
                alert.SetPositiveButton("Закрыть", (senderAlert, args) =>
                {
                    Toast.MakeText(Activity, "Предупреждение было закрыто!", ToastLength.Short).Show();
                });
                Dialog dialog = alert.Create();
                dialog.Show();
            }
            BuildLocationRequest();
            BuildLocationCallBack();

            fusedLocationProviderClient = LocationServices.GetFusedLocationProviderClient(Activity);

            fusedLocationProviderClient.RequestLocationUpdates(locationRequest,
                locationCallback, Looper.MyLooper());

            return view;
        }

        private void BuildLocationCallBack()
        {
            locationCallback = new AuthLocationCallBack(this);
        }

        private void BuildLocationRequest()
        {
            locationRequest = new LocationRequest();
            locationRequest.SetPriority(LocationRequest.PriorityBalancedPowerAccuracy);
            if (StaticBox.Sensors["Местоположение контейнера"] != "На складе" || StaticBox.Sensors["Местоположение контейнера"] != "У заказчика")
            {
                locationRequest.SetInterval(1000);
                locationRequest.SetFastestInterval(3000);
                locationRequest.SetSmallestDisplacement(10f);
            }
        }



        public void OnMapReady(GoogleMap googleMap)
        {
            try
            {
                this.GMap = googleMap;

                if (StaticBox.Latitude == 0 || StaticBox.Longitude == 0)
                {
                    AlertDialog.Builder alert = new AlertDialog.Builder(Activity);
                    alert.SetTitle("Внимание !");
                    alert.SetMessage("Местоположение контейнера не изменяется, так как на телефоне отключен GPS.\n Включите, пожалуйста, GPS.");
                    alert.SetPositiveButton("Закрыть", (senderAlert, args) =>
                    {
                        Toast.MakeText(Activity, "Предупреждение было закрыто!", ToastLength.Short).Show();
                    });
                    Dialog dialog = alert.Create();
                    dialog.Show();
                    return;
                }
                double latitude = StaticBox.Latitude;
                double longitude = StaticBox.Longitude;

                LatLng location = new LatLng(latitude, longitude);
                //PolylineOptions rectOptions = new PolylineOptions()
                //{

                //};
                //rectOptions.Geodesic(true);
                //rectOptions.InvokeWidth(1);
                //rectOptions.InvokeColor(Color.Blue);

                MarkerOptions markerOpt1 = new MarkerOptions();
                //location = new LatLng(latitude, longitude);

                markerOpt1.SetPosition(location);
                markerOpt1.SetTitle("Контейнер здесь");
                //markerOpt1.SetSnippet(StaticOrder.Inception_address);

                var bmDescriptor = BitmapDescriptorFactory.DefaultMarker(BitmapDescriptorFactory.HueBlue);
                markerOpt1.InvokeIcon(bmDescriptor);

                googleMap.AddMarker(markerOpt1);

               // googleMap.AddPolyline(rectOptions);

                CameraPosition.Builder builder = CameraPosition.InvokeBuilder();
                builder.Target(location);
                builder.Zoom(10);
                builder.Bearing(0);
                builder.Tilt(65);

                CameraPosition cameraPosition = builder.Build();
                CameraUpdate cameraUpdate = CameraUpdateFactory.NewCameraPosition(cameraPosition);

                //googleMap.UiSettings.ZoomControlsEnabled = true;
                //googleMap.UiSettings.CompassEnabled = true;
                googleMap.MoveCamera(cameraUpdate);
            }
            catch (Exception ex)
            {
                Toast.MakeText(Activity, ex.Message, ToastLength.Long).Show();
            }
        }

        internal class AuthLocationCallBack : LocationCallback // !!!!
        {
            private ActivityMap activityUserBox;

            public AuthLocationCallBack(ActivityMap activityUserBoxy)
            {
                this.activityUserBox = activityUserBoxy;
            }

            public override void OnLocationResult(LocationResult result)
            {
                base.OnLocationResult(result);

                try
                {
                    if (result == null)
                    {
                        return;
                    }

                    StaticBox.Latitude = result.LastLocation.Latitude;
                    StaticBox.Longitude = result.LastLocation.Longitude;
                    StaticBox.CurrentDate = DateTime.Now;

                    //txtLongitude.Text = result.LastLocation.Longitude.ToString();
                    //s_latitude.Text = result.LastLocation.Longitude.ToString();
                    //s_date_time.Text = DateTime.Now.ToString();

                    PostGeoData();
                }
                catch (Exception ex)
                {
                    Toast.MakeText(Application.Context, ex.Message, ToastLength.Short).Show();
                }
            }

            private async void PostGeoData()
            {
                using (var client = ClientHelper.GetClient(StaticUser.Token))
                {
                    PositionModel model = new PositionModel
                    {
                        CurrentDate = DateTime.Now.ToString(),
                        Latitude = StaticBox.Latitude,
                        Longitude = StaticBox.Longitude,
                        DeviceId = StaticBox.DeviceId
                    };

                    LocationService.InitializeClient(client);
                    var o_data = await LocationService.SetGPS(model);

                    Toast.MakeText(Application.Context, o_data.SuccessInfo, ToastLength.Long).Show();
                }
            }
        }

        public override void OnResume()
        {
            base.OnResume();
            if (mMapView != null)
                mMapView.OnResume();
        }

        public override void OnLowMemory()
        {
            base.OnLowMemory();
            if (mMapView != null)
                mMapView.OnLowMemory();
        }

        public override void OnPause()
        {
            base.OnPause();
            if (mMapView != null)
                mMapView.OnPause();
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            if (mMapView != null)
                mMapView.OnDestroy();
        }
    }
}