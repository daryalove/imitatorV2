using Android;
using Android.Content;
using Android.Content.PM;
using Android.Hardware.Fingerprints;
using Android.OS;
using Android.Support.V4.App;
using Android.Widget;
using Imitator.CommonData.DataModels;
using Imitator.CommonData.ViewModels;
using Imitator.WebServices;
using Imitator.WebServices.Account;
using Plugin.Settings;
using System;
using _Activity = Android.App.Activity;

namespace Imitator.Android.Activity.Authorisation
{
    internal class FingerprintHandler : FingerprintManager.AuthenticationCallback
    {
        private _Activity ParentActivity;

        public FingerprintHandler(_Activity ParentActivity)
        {
            this.ParentActivity = ParentActivity;
        }

        internal void StartAuthentication(FingerprintManager fingerprintManager, FingerprintManager.CryptoObject cryptoObject)
        {
            CancellationSignal cancellationSignal = new CancellationSignal();
            if (ActivityCompat.CheckSelfPermission(ParentActivity, Manifest.Permission.UseFingerprint)
                != (int)Permission.Granted)
                return;
            fingerprintManager.Authenticate(cryptoObject, cancellationSignal, 0, this, null);
        }
        public override void OnAuthenticationFailed()
        {
            Toast.MakeText(ParentActivity, "Ошибка авторизации по отпечатку пальца!", ToastLength.Long).Show();
        }

        public async override void OnAuthenticationSucceeded(FingerprintManager.AuthenticationResult result)
        {
            try
            {
                using (var client = ClientHelper.GetClient())
                {
                    AuthService.InitializeClient(client);
                    UserShortModel o_data = null;

                    var login = CrossSettings.Current.GetValueOrDefault("UserLoginRegistration", "");
                    var password = CrossSettings.Current.GetValueOrDefault("UserPasswordRegistration", ""); 

                    o_data = await AuthService.Login(login, password);

                    if (o_data.Result.ToString() == "OK")
                    {
                        Toast.MakeText(ParentActivity, "Авторизация прошла успешно !", ToastLength.Long).Show();
                        StaticUser.Token = o_data.Token;

                        Intent intent = new Intent(ParentActivity, typeof(ActivityMainFunctionality));
                        ParentActivity.StartActivity(intent);
                    }
                    else
                        Toast.MakeText(ParentActivity, "Попробуйте повторить вход. " + o_data.ErrorInfo, ToastLength.Long).Show();
                }
            }
            catch (Exception ex)
            {
                Toast.MakeText(ParentActivity, "Попробуйте повторить вход. " + ex.Message, ToastLength.Long).Show();
            }
        }
    }
}