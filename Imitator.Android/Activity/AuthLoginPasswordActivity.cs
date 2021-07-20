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

namespace Imitator.Android.Activity
{
    public class AuthLoginPasswordActivity : Fragment
    {
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.AuthorisationLoginPassword, container, false);

            //try
            //{

            //    layouts = new int[]
            //    {
            //         Resource.Layout.AuthorisationLoginPassword,
            //         Resource.Layout.AuthorisationFingerprint
            //    };

            //    _viewpager = view.FindViewById<ViewPager>(Resource.Id.viewPager);

            //    FragmentTransaction transaction = this.FragmentManager.BeginTransaction();
            //    ViewPagerAdapter adapter = new ViewPagerAdapter(layouts, ref transaction);
            //    _viewpager.Adapter = adapter;

            //    //_viewpager.PageSelected += ViewPager_PageSelected;
            //}
            //catch (Exception ex)
            //{
            //    Toast.MakeText(Activity, "" + ex.Message, ToastLength.Long).Show();
            //}
            return view;
        }
    }
}