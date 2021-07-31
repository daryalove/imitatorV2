using Android.App;
using Android.Hardware.Fingerprints;
using Android.OS;
using Android.Views;
using Android.Widget;
using Javax.Crypto;
using Plugin.Settings;
using System;

namespace Imitator.Android.Activity.Authorisation
{
    public class AuthFingerprintActivity : Fragment
    {
        private FingerprintManager fingerprintManager;
        private Cipher cipher;

        public AuthFingerprintActivity(FingerprintManager fingerprintManager, Cipher cipher)
        {
            this.fingerprintManager = fingerprintManager;
            this.cipher = cipher;
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.AuthorisationFingerprint, container, false);

            var IsFingerprintVerificationSuccessful = Convert.ToBoolean(CrossSettings.Current.GetValueOrDefault("IsFingerprintVerificationSuccessful", ""));

            if (IsFingerprintVerificationSuccessful)
            {
                FingerprintManager.CryptoObject cryptoObject = new FingerprintManager.CryptoObject(cipher);
                FingerprintHandler handler = new FingerprintHandler(Activity);
                handler.StartAuthentication(fingerprintManager, cryptoObject);
            }
            else
            {
                Toast.MakeText(Activity, "Что-то пошло не так при попытке доступа к функционалу отпечатка пальца", ToastLength.Short).Show();
            }

            return view;
        }

    }
}