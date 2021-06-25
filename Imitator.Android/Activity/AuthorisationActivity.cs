using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.Content;
using Android.App;

using Android.Views;
using Android.Widget;
using Android.Support.V4.View;
using Android.OS;

namespace Imitator.Android.Activity
{
    public class AuthorisationActivity : Fragment
    {
        /// <summary>
        /// Конпка прехода на форму авторизации.
        /// </summary>
        private Button btn_auth_form;
        private ViewPager _viewpager;
        public int[] layouts;


        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.AuthorisationPage, container, false);

            Intent intent = new Intent(Activity, typeof(Activity.MainActivity2));
            StartActivity(intent);
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

    //public class ViewPagerAdapter : PagerAdapter
    //{
    //    LayoutInflater layoutInflater;
    //    int[] _layout;
    //    public string[] PageName = { "Логин/Пароль", "Touch ID" };
    //    FragmentTransaction transaction;

    //    public ViewPagerAdapter(int[] layout, ref FragmentTransaction transaction)
    //    {
    //        _layout = layout;
    //        this.transaction = transaction;
    //    }

    //    public override Java.Lang.ICharSequence GetPageTitleFormatted(int position)
    //    {
    //        return new Java.Lang.String(PageName[position]);
    //    }
    //    public override Java.Lang.Object InstantiateItem(ViewGroup container, int position)
    //    {
    //        //if (position == 1)
    //        //{
    //        //    layoutInflater = (LayoutInflater)Application.Context.GetSystemService(Context.LayoutInflaterService);
    //        //    View view = layoutInflater.Inflate(_layout[position], container, false);
    //        //    var btn_cost = view.FindViewById<Button>(Resource.Id.Slide0BtnAddOrder);
    //        //    btn_cost.Click += (sender, e) =>
    //        //    {
    //        //        AddOrderActivity content = new AddOrderActivity();
    //        //        transaction.Replace(Resource.Id.framelayout, content).AddToBackStack(null).Commit();
    //        //    };
    //        //    container.AddView(view);
    //        //    return view;
    //        //}
    //        //else
    //        //{
    //        //    layoutInflater = (LayoutInflater)Android.App.Application.Context.GetSystemService(Context.LayoutInflaterService);
    //        //    View view = layoutInflater.Inflate(_layout[position], container, false);
    //        //    container.AddView(view);
    //        //    return view;
    //        //}
    //        layoutInflater = (LayoutInflater)Application.Context.GetSystemService(Context.LayoutInflaterService);
    //        View view = layoutInflater.Inflate(_layout[position], container, false);
    //        container.AddView(view);
    //        return view;
    //    }

    //    public override int Count
    //    {
    //        get
    //        {
    //            return _layout.Length;
    //        }
    //    }

    //    public override bool IsViewFromObject(View view, Java.Lang.Object objectValue)
    //    {
    //        return view == objectValue;
    //    }

    //    public override void DestroyItem(ViewGroup container, int position, Java.Lang.Object objectValue)
    //    {
    //        View view = (View)objectValue;

    //        container.RemoveView(view);
    //    }
    //}
}