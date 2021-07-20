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
using Android.Support.V7.App;
using Android.Graphics.Drawables;
using Android.Support.V4.Content;

namespace Imitator.Android.Activity
{
    [Activity(Label = "AuthorisationActivity")]
    public class AuthorisationActivity : AppCompatActivity
    {
        private TextView AuthorisationTypeText;
        private Button BtnAuthorisationFingerprint;
        private Button BtnAuthorisationLoginPassword;
        private Button BtnAuthorisation;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.AuthorisationPage);

            AuthorisationTypeText = FindViewById<TextView>(Resource.Id.AuthorisationTypeText);
            BtnAuthorisationFingerprint = FindViewById<Button>(Resource.Id.BtnAuthorisationFingerprint);
            BtnAuthorisationLoginPassword = FindViewById<Button>(Resource.Id.BtnAuthorisationLoginPassword);
            BtnAuthorisation = FindViewById<Button>(Resource.Id.BtnAuthorisation);

            // Эти переменные необходимы для изменения цвета кнопок BtnAuthorisationFingerprint и BtnAuthorisationLoginPassword.
            // А точнее для изменения свойства <solid ... /> в стилях ChangeAuthTypeLeftPart и ChangeAuthTypeRightPart(drawable)
            Drawable FingerprintBackground = BtnAuthorisationFingerprint.Background;
            Drawable LoginPasswordBackground = BtnAuthorisationLoginPassword.Background;

            GradientDrawable FingerprintGradient = (GradientDrawable)FingerprintBackground;
            GradientDrawable LoginPasswordGradient = (GradientDrawable)LoginPasswordBackground;


            FragmentTransaction transaction = this.FragmentManager.BeginTransaction();

            AuthFingerprintActivity Fingerprint = new AuthFingerprintActivity();
            transaction.Replace(Resource.Id.FrameLayoutAuthorisation, Fingerprint);
            transaction.Commit();

            LoginPasswordGradient.SetColor(ContextCompat.GetColor(this, Resource.Color.NotActivPageColor));
            FingerprintGradient.SetColor(ContextCompat.GetColor(this, Resource.Color.BackgroundColor));

            BtnAuthorisationFingerprint.Click += async (s, e) =>
            {
                try
                {
                    FragmentTransaction TFingerprint = this.FragmentManager.BeginTransaction();
                    AuthFingerprintActivity Fingerprint = new AuthFingerprintActivity();
                    TFingerprint.Replace(Resource.Id.FrameLayoutAuthorisation, Fingerprint);
                    TFingerprint.Commit();
                    AuthorisationTypeText.Text = "Укажите отпечаток пальца";
                    FingerprintGradient.SetColor(ContextCompat.GetColor(this, Resource.Color.BackgroundColor));
                    LoginPasswordGradient.SetColor(ContextCompat.GetColor(this, Resource.Color.NotActivPageColor));
                }
                catch (System.Exception ex)
                {
                    Toast.MakeText(this, ex.Message, ToastLength.Long).Show();
                }            
            };

            BtnAuthorisationLoginPassword.Click += async (s, e) =>
            {
                try
                {
                    FragmentTransaction TLoginPassword = this.FragmentManager.BeginTransaction();
                    AuthLoginPasswordActivity LoginPassword = new AuthLoginPasswordActivity();
                    TLoginPassword.Replace(Resource.Id.FrameLayoutAuthorisation, LoginPassword);
                    TLoginPassword.Commit();

                    AuthorisationTypeText.Text = "Введите логин и пароль";
                    FingerprintGradient.SetColor(ContextCompat.GetColor(this, Resource.Color.NotActivPageColor));
                    LoginPasswordGradient.SetColor(ContextCompat.GetColor(this, Resource.Color.BackgroundColor));
                }
                catch (System.Exception ex)
                {
                    Toast.MakeText(this, ex.Message, ToastLength.Long).Show();
                }
            };

            BtnAuthorisation.Click += async (s, e) =>
            {
                try
                {
                    Intent intent = new Intent(this, typeof(Activity.ActivityMainFunctionality));
                    StartActivity(intent);
                }
                catch (System.Exception ex)
                {
                    Toast.MakeText(this, ex.Message, ToastLength.Long).Show();
                }
            };
        }

        /// <summary>
        /// Конпка прехода на форму авторизации.
        /// </summary>
        //private ViewPager _viewpager;
        //public int[] layouts;


        //public override void OnCreate(Bundle savedInstanceState)
        //{
        //    base.OnCreate(savedInstanceState);
        //}
        //public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        //{
        //    var view = inflater.Inflate(Resource.Layout.AuthorisationPage, container, false);
        //    var BtnAuthorisation = view.FindViewById<Button>(Resource.Id.BtnAuthorisation);

        //    BtnAuthorisation.Click += async (s, e) =>
        //    {
        //        try
        //        {
        //            Intent intent = new Intent(Activity, typeof(Activity.ActivityMainFunctionality));
        //            StartActivity(intent);
        //        }
        //        catch (System.Exception ex)
        //        {
        //            Toast.MakeText(Activity, ex.Message, ToastLength.Long).Show();
        //        }
        //    };

            
        //    //try
        //    //{

        //    //    layouts = new int[]
        //    //    {
        //    //         Resource.Layout.AuthorisationLoginPassword,
        //    //         Resource.Layout.AuthorisationFingerprint
        //    //    };

        //    //    _viewpager = view.FindViewById<ViewPager>(Resource.Id.viewPager);

        //    //    FragmentTransaction transaction = this.FragmentManager.BeginTransaction();
        //    //    ViewPagerAdapter adapter = new ViewPagerAdapter(layouts, ref transaction);
        //    //    _viewpager.Adapter = adapter;

        //    //    //_viewpager.PageSelected += ViewPager_PageSelected;
        //    //}
        //    //catch (Exception ex)
        //    //{
        //    //    Toast.MakeText(Activity, "" + ex.Message, ToastLength.Long).Show();
        //    //}
        //    return view;
        //}
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