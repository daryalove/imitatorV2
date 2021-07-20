using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Com.Karumi.Dexter;
using Com.Karumi.Dexter.Listener;
using Com.Karumi.Dexter.Listener.Multi;

namespace Imitator.Android
{
    //[Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public class HomeActivity : AppCompatActivity/*, BottomNavigationView.IOnNavigationItemSelectedListener*/
    {
        //protected override void OnCreate(Bundle savedInstanceState)
        //{
        //    base.OnCreate(savedInstanceState);

        //    Xamarin.Essentials.Platform.Init(this, savedInstanceState);
        //    SetContentView(Resource.Layout.PageHome);

        //    FragmentTransaction transaction = this.FragmentManager.BeginTransaction();


        //    MainActivity mainActivity = new MainActivity();
        //    transaction.Replace(Resource.Id.framelayout, mainActivity);
        //    transaction.Commit();

        //    string dir_path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
        //}

        ////permissions events
        //public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        //{
        //    Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

        //    base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        //}

        ////public bool OnNavigationItemSelected(IMenuItem item)
        ////{
        ////    switch (item.ItemId)
        ////    {
        ////        case Resource.Id.navigation_home:
        ////            return true;
        ////        case Resource.Id.navigation_dashboard:
        ////            return true;
        ////        case Resource.Id.navigation_notifications:
        ////            return true;
        ////        case Resource.Id.navigation_information:
        ////            return true;
        ////    }
        ////    return false;
        ////}

        ////public override bool OnCreateOptionsMenu(IMenu menu)
        ////{
        ////    MenuInflater.Inflate(Resource.Menu.menu_main, menu);
        ////    return true;
        ////}

        ////public override bool OnOptionsItemSelected(IMenuItem item)
        ////{

        ////    int id = item.ItemId;
        ////    if (id == Resource.Id.action_settings)
        ////    {
        ////        return true;
        ////    }

        ////    return base.OnOptionsItemSelected(item);
        ////}

        //private class SamplePermissionListener : Java.Lang.Object, IMultiplePermissionsListener
        //{
        //    MainActivity activity;
        //    public SamplePermissionListener(MainActivity activity)
        //    {
        //        this.activity = activity;
        //    }

        //    public void OnPermissionDenied(PermissionDeniedResponse p0)
        //    {
        //        //Snackbar.Make(activity.main_form, "Permission Denied", Snackbar.LengthShort).Show();
        //    }

        //    public void OnPermissionGranted(PermissionGrantedResponse p0)
        //    {
        //        //Snackbar.Make(activity.main_form, "Permission Granted", Snackbar.LengthShort).Show();
        //    }

        //    public void OnPermissionRationaleShouldBeShown(IList<PermissionRequest> p0, IPermissionToken p1)
        //    {
        //        p1.ContinuePermissionRequest();
        //        throw new System.NotImplementedException();
        //    }

        //    public void OnPermissionsChecked(MultiplePermissionsReport p0)
        //    {
        //        if (p0.AreAllPermissionsGranted())
        //        {

        //        }

        //        if (p0.IsAnyPermissionPermanentlyDenied)
        //        {
        //            // show alert dialog navigating to Settings

        //        }
        //    }
        //}

    }

    //internal class MyDismissListener : Java.Lang.Object, IDialogInterfaceOnDismissListener
    //{
    //    private IPermissionToken token;

    //    public MyDismissListener(IPermissionToken token)
    //    {
    //        this.token = token;
    //    }

    //    public void OnDismiss(IDialogInterface dialog)
    //    {
    //        token.CancelPermissionRequest();
    //    }
    //}
}