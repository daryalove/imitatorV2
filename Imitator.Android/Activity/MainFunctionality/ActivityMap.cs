using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace Imitator.Android.Activity.MainFunctionality
{
    [Obsolete]
    public class ActivityMap : Fragment
    {
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.PageMap, container, false);

            //var layout = view.FindViewById<SlidingUpPanelLayout>(Resource.Id.sliding_client_layout);

            //layout.AnchorPoint = 0.3f;
            //layout.PanelExpanded += (s, e) => Log.Info(Tag, "PanelExpanded");
            //layout.PanelCollapsed += (s, e) => Log.Info(Tag, "PanelCollapsed");
            //layout.PanelAnchored += (s, e) => Log.Info(Tag, "PanelAnchored");
            //layout.PanelSlide += (s, e) =>
            //{
            //    if (e.SlideOffset < 0.2)
            //    {
            //        //if (SupportActionBar.IsShowing)
            //        //    SupportActionBar.Hide();
            //    }
            //    else
            //    {
            //        //if (!SupportActionBar.IsShowing)
            //        //    SupportActionBar.Show();
            //    }
            //};


            //MapsInitializer.Initialize(Activity);
            //mMapView = view.FindViewById<MapView>(Resource.Id.FragmentMap);

            //switch (GooglePlayServicesUtil.IsGooglePlayServicesAvailable(Activity))
            //{
            //    case ConnectionResult.Success:
            //        Toast.MakeText(Activity, "SUCCESS", ToastLength.Long).Show();
            //        mMapView.OnCreate(savedInstanceState);
            //        mMapView.GetMapAsync(this);
            //        break;
            //    case ConnectionResult.ServiceMissing:
            //        Toast.MakeText(Activity, "ServiceMissing", ToastLength.Long).Show();
            //        break;
            //    case ConnectionResult.ServiceVersionUpdateRequired:
            //        Toast.MakeText(Activity, "Update", ToastLength.Long).Show();
            //        break;
            //    default:
            //        Toast.MakeText(Activity, GooglePlayServicesUtil.IsGooglePlayServicesAvailable(Activity), ToastLength.Long).Show();
            //        break;
            //}


            return view;
        }

       

        //public void OnMapReady(GoogleMap googleMap)
        //{
        //    try
        //    {
        //        this.GMap = googleMap;

        //        LatLng location = new LatLng(47.264998, 39.720358);
        //        PolylineOptions rectOptions = new PolylineOptions()
        //        {

        //        };
        //        rectOptions.Geodesic(true);
        //        rectOptions.InvokeWidth(1);
        //        rectOptions.InvokeColor(Color.Blue);

        //        for (int i = 0; i < 1; i++)
        //        {
        //            var latitude = 47.264998;
        //            var longitude = 39.720358;

        //            LatLng new_location = new LatLng(
        //               latitude,
        //                longitude);

        //            rectOptions.Add(new_location);

        //            if (i == 0)
        //            {
        //                MarkerOptions markerOpt1 = new MarkerOptions();
        //                //location = new LatLng(latitude, longitude);

        //                markerOpt1.SetPosition(new LatLng(latitude, longitude));
        //                markerOpt1.SetTitle("Пункт отправления\n" + "Орбительная ул.");
        //                //markerOpt1.SetSnippet(StaticOrder.Inception_address);

        //                var bmDescriptor = BitmapDescriptorFactory.DefaultMarker(BitmapDescriptorFactory.HueBlue);
        //                markerOpt1.InvokeIcon(bmDescriptor);

        //                googleMap.AddMarker(markerOpt1);

        //                continue;
        //            }
        //            //else if (i == 1)
        //            //{
        //            //    MarkerOptions markerOpt1 = new MarkerOptions();
        //            //    //location = new LatLng(latitude, longitude);

        //            //    markerOpt1.SetPosition(new LatLng(latitude, longitude));
        //            //    markerOpt1.SetTitle("Пункт назначения\n" + "Горизонт");
        //            //    //markerOpt1.SetSnippet(StaticOrder.Destination_address);

        //            //    var bmDescriptor = BitmapDescriptorFactory.DefaultMarker(BitmapDescriptorFactory.HueRed);
        //            //    markerOpt1.InvokeIcon(bmDescriptor);

        //            //    googleMap.AddMarker(markerOpt1);

        //            //    continue;
        //            //}
        //            MarkerOptions markerOptions = new MarkerOptions();

        //            markerOptions.SetPosition(new_location);
        //            markerOptions.SetTitle(i.ToString());
        //            googleMap.AddMarker(markerOptions);

        //        }

        //        googleMap.AddPolyline(rectOptions);

        //        CameraPosition.Builder builder = CameraPosition.InvokeBuilder();
        //        builder.Target(location);
        //        builder.Zoom(10);
        //        builder.Bearing(0);
        //        builder.Tilt(65);

        //        CameraPosition cameraPosition = builder.Build();
        //        CameraUpdate cameraUpdate = CameraUpdateFactory.NewCameraPosition(cameraPosition);

        //        //googleMap.UiSettings.ZoomControlsEnabled = true;
        //        //googleMap.UiSettings.CompassEnabled = true;
        //        googleMap.MoveCamera(cameraUpdate);
        //    }
        //    catch (Exception ex)
        //    {
        //        Toast.MakeText(Activity, ex.Message, ToastLength.Long).Show();
        //    }
        //}
    }
}