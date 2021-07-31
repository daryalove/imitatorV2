using System;
using Android.Content;
using Android.App;
using Android.Widget;
using Android.OS;
using Android.Support.V7.App;
using Android.Graphics.Drawables;
using Android.Support.V4.Content;
using Imitator.WebServices.Account;
using Imitator.WebServices;
using Imitator.CommonData.ViewModels;
using Imitator.CommonData.DataModels;
using Plugin.Settings;
using Android.Hardware.Fingerprints;
using ActivityCompat = Android.Support.V4.App.ActivityCompat;
using Permission = Android.Content.PM.Permission;
using AlertDialog = Android.App.AlertDialog;
using Android;
using Java.Security;
using Javax.Crypto;
using Android.Security.Keystore;

namespace Imitator.Android.Activity.Authorisation
{
    [Activity(Label = "AuthorisationActivity")]
    public class AuthorisationActivity : AppCompatActivity
    {
        private KeyStore keyStore;
        private Cipher cipher;
        private FingerprintManager fingerprintManager;
        private string KEY_NAME = "Ahsan";

        private TextView AuthorisationTypeText;
        private Button BtnAuthorisationFingerprint;
        private Button BtnAuthorisationLoginPassword;
        private Button BtnAuthorisation;
        bool AccessEvent;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.AuthorisationPage);

            CheckFingerprintPermition();

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

            AuthLoginPasswordActivity LoginPassword = new AuthLoginPasswordActivity();
            transaction.Replace(Resource.Id.FrameLayoutAuthorisation, LoginPassword);
            transaction.Commit();

            AuthorisationTypeText.Text = "Введите логин и пароль";

            LoginPasswordGradient.SetColor(ContextCompat.GetColor(this, Resource.Color.BackgroundColor));
            FingerprintGradient.SetColor(ContextCompat.GetColor(this, Resource.Color.NotActivPageColor));

            BtnAuthorisationFingerprint.Click += async (s, e) =>
            {

                if (CrossSettings.Current.GetValueOrDefault("AccessFingerprintAuthorisation", "") != "")
                {
                    AccessEvent = Convert.ToBoolean(CrossSettings.Current.GetValueOrDefault("AccessFingerprintAuthorisation", string.Empty));

                    if (AccessEvent)
                    {
                        FragmentTransaction TFingerprint = this.FragmentManager.BeginTransaction();
                        AuthFingerprintActivity Fingerprint = new AuthFingerprintActivity(fingerprintManager, cipher);
                        TFingerprint.Replace(Resource.Id.FrameLayoutAuthorisation, Fingerprint);
                        TFingerprint.Commit();
                        AuthorisationTypeText.Text = "Приложите палец к датчику на телефоне, чтобы подтвердить свою личность";
                        FingerprintGradient.SetColor(ContextCompat.GetColor(this, Resource.Color.BackgroundColor));
                        LoginPasswordGradient.SetColor(ContextCompat.GetColor(this, Resource.Color.NotActivPageColor));
                    }
                    else
                    {
                        AlertDialogCall();
                    }
                }
                else
                {
                    AlertDialogCall();
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
                    if (StaticDataClass.UserLogin != "" && StaticDataClass.UserPassword != "")
                    {
                        using (var client = ClientHelper.GetClient())
                        {
                            AuthService.InitializeClient(client);
                            UserShortModel o_data = null;

                            o_data = await AuthService.Login(StaticDataClass.UserLogin, StaticDataClass.UserPassword);

                            if (o_data.Result.ToString() == "OK")
                            {
                                //Toast.MakeText(this, "Пользователь: " + o_data.UserFIO, ToastLength.Long).Show();
                                //Toast.MakeText(this, "Информация: " + o_data.Token, ToastLength.Long).Show();
                                Toast.MakeText(this, "Авторизация прошла успешно  !", ToastLength.Long).Show();
                                StaticUser.Token = o_data.Token;
                                Intent intent = new Intent(this, typeof(Activity.ActivityMainFunctionality));
                                StartActivity(intent);
                            }
                            else
                                Toast.MakeText(this, "Ошибка: " + o_data.ErrorInfo, ToastLength.Long).Show();
                        }
                    }
                    else
                    {
                        Toast.MakeText(this, "Перед авторизацией необходимо ввести логин и пароль.", ToastLength.Long).Show();
                    }
                }
                catch (Exception ex)
                {
                    Toast.MakeText(this, ex.Message, ToastLength.Long).Show();
                }
                //try
                //{
                //    Intent intent = new Intent(this, typeof(Activity.ActivityMainFunctionality));
                //    StartActivity(intent);
                //}
                //catch (System.Exception ex)
                //{
                //    Toast.MakeText(this, ex.Message, ToastLength.Long).Show();
                //}
            };
        }
        private void AlertDialogCall()
        {
            AlertDialog.Builder alert = new AlertDialog.Builder(this);
            alert.SetTitle("Оповещение");
            alert.SetMessage("Авторизация с помощью отпечатка пальца недоступна. " +
                "Получить доступ к этой функции возможно только в личных данных пользователя после входа в аккаунт.");
            alert.SetPositiveButton("Ок", (senderAlert, args) => { });
            Dialog dialog = alert.Create();
            dialog.Show();
        }

        private void CheckFingerprintPermition()
        {
            KeyguardManager keyguardManager = (KeyguardManager)GetSystemService(KeyguardService);
            fingerprintManager = (FingerprintManager)GetSystemService(FingerprintService);
            if (ActivityCompat.CheckSelfPermission(this, Manifest.Permission.UseFingerprint)
                != (int)Permission.Granted)
                return;
            if (!fingerprintManager.IsHardwareDetected)
                Toast.MakeText(this, "Авторизация с помощью отпечатка пальца не включена", ToastLength.Short).Show();
            else
            {
                if (!fingerprintManager.HasEnrolledFingerprints)
                    Toast.MakeText(this, "Зарегистрируйте хотя бы один отпечаток пальца в настройках", ToastLength.Short).Show();
                else
                {
                    if (!keyguardManager.IsKeyguardSecure)
                        Toast.MakeText(this, "Безопасность экрана блокировки не включена в настройках", ToastLength.Short).Show();
                    else
                        GenKey();
                    if (CipherInit())
                    {
                        CrossSettings.Current.AddOrUpdateValue("IsFingerprintVerificationSuccessful", "true");
                    }
                    else
                        CrossSettings.Current.AddOrUpdateValue("IsFingerprintVerificationSuccessful", "false");
                }
            }
        }

        private bool CipherInit()
        {
            try
            {
                cipher = Cipher.GetInstance(KeyProperties.KeyAlgorithmAes
                    + "/"
                    + KeyProperties.BlockModeCbc
                    + "/"
                    + KeyProperties.EncryptionPaddingPkcs7);
                keyStore.Load(null);
                IKey key = (IKey)keyStore.GetKey(KEY_NAME, null);
                cipher.Init(CipherMode.EncryptMode, key);
                return true;
            }
            catch (Exception ex) { return false; }
        }

        private void GenKey()
        {
            keyStore = KeyStore.GetInstance("AndroidKeyStore");
            KeyGenerator keyGenerator = null;
            keyGenerator = KeyGenerator.GetInstance(KeyProperties.KeyAlgorithmAes, "AndroidKeyStore");
            keyStore.Load(null);
            keyGenerator.Init(new KeyGenParameterSpec.Builder(KEY_NAME, KeyStorePurpose.Encrypt | KeyStorePurpose.Decrypt)
                .SetBlockModes(KeyProperties.BlockModeCbc)
                .SetUserAuthenticationRequired(true)
                .SetEncryptionPaddings(KeyProperties
                .EncryptionPaddingPkcs7).Build());
            keyGenerator.GenerateKey();
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