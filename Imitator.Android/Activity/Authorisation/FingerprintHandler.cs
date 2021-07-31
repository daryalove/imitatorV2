using Android;
using Android.Content;
using Android.Content.PM;
using Android.Hardware.Fingerprints;
using Android.OS;
using Android.Support.V4.App;
using Android.Widget;
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

        public override void OnAuthenticationSucceeded(FingerprintManager.AuthenticationResult result)
        {
            //this.ParentActivity.StartActivity(new Intent(mainActivity, typeof(HomeActivity)));
            Intent intent = new Intent(ParentActivity, typeof(ActivityMainFunctionality));
            ParentActivity.StartActivity(intent);
            Toast.MakeText(ParentActivity, "Авторизация прошла успешно !", ToastLength.Long).Show();
        }
    }
}