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
using Android.Views;
using Imitator.Android.Services;

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
        private ProgressBar loader;
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
            loader = FindViewById<ProgressBar>(Resource.Id.loader);

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
                    loader.Visibility = ViewStates.Visible;
                    if (StaticDataClass.UserLogin != "" && StaticDataClass.UserPassword != "")
                    {
                        using (var client = ClientHelper.GetClient())
                        {
                            AuthService.InitializeClient(client);
                            UserShortModel o_data = null;

                            o_data = await AuthService.Login(StaticDataClass.UserLogin, StaticDataClass.UserPassword);

                            if (o_data.Result.ToString() == "OK")
                            {
                                FirebaseService.StartTracking();
                                loader.Visibility = ViewStates.Invisible;

                                Toast.MakeText(this, "Авторизация прошла успешно  !", ToastLength.Long).Show();
                                StaticUser.Token = o_data.Token;
                                Intent intent = new Intent(this, typeof(Activity.ActivityMainFunctionality));
                                StartActivity(intent);
                            }
                            else
                                Toast.MakeText(this, "Ошибка: " + o_data.ErrorInfo, ToastLength.Long).Show();
                            loader.Visibility = ViewStates.Invisible;
                        }
                    }
                    else
                    {
                        Toast.MakeText(this, "Перед авторизацией необходимо ввести логин и пароль.", ToastLength.Long).Show();
                        loader.Visibility = ViewStates.Invisible;
                    }
                }
                catch (Exception ex)
                {
                    Toast.MakeText(this, ex.Message, ToastLength.Long).Show();
                }
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

    }
}