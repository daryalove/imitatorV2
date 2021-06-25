using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V4.Widget;
using Android.Content.PM;
using Android.Widget;
using Android.Support.V7.App;
using TelephonyManager = Android.Telephony.TelephonyManager;
using Android.Views;
using Android;
using Android.Content;
using Secure = Android.Provider.Settings.Secure;
using Android.Bluetooth;
using System.Threading.Tasks;
using Com.Karumi.Dexter.Listener;
using Com.Karumi.Dexter.Listener.Multi;
using Com.Karumi.Dexter;
using System.Collections.Generic;
using Plugin.Settings;
using Imitator.CommonData;
using System;
using Imitator.Android.Activity;

namespace Imitator.Android
{
    public class MainActivity : Fragment
    {
        /// <summary>
        /// Кнoпка прехода на форму авторизации.
        /// </summary>
        private Button BtnOpenAuthorisationPage;

        private int MY_PERMISSIONS_REQUEST_CAMERA = 100;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.PageMain, container, false);

            FragmentTransaction transaction = this.FragmentManager.BeginTransaction();
            BtnOpenAuthorisationPage = view.FindViewById<Button>(Resource.Id.BtnOpenAuthorisationPage);

            BtnOpenAuthorisationPage.Click += async (s, e) =>
            {

                try
                {
                    AuthorisationActivity _authorisationActivity = new AuthorisationActivity();
                    transaction.Replace(Resource.Id.framelayout, _authorisationActivity);
                    transaction.Commit();
                }
                catch (System.Exception ex)
                {
                    Toast.MakeText(Activity, ex.Message, ToastLength.Long).Show();
                }
            };

            return view;
        }       
    }

   
}

