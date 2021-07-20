using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.Content;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;

namespace Imitator.Android.Activity.Registration
{
    [Activity(Label = "RegistrationActivity")]
    public class RegistrationActivity : AppCompatActivity
    {

        private Button BtnRegistrationFingerprint;
        private Button BtnRegistrationLoginPassword;
        private Button BtnRegistration;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.PageRegistration);
            BtnRegistrationFingerprint = FindViewById<Button>(Resource.Id.BtnRegistrationFingerprint);
            BtnRegistrationLoginPassword = FindViewById<Button>(Resource.Id.BtnRegistrationLoginPassword);
            BtnRegistration = FindViewById<Button>(Resource.Id.BtnRegistration);

            // Эти переменные необходимы для изменения цвета кнопок BtnAuthorisationFingerprint и BtnAuthorisationLoginPassword.
            // А точнее для изменения свойства <solid ... /> в стилях ChangeAuthTypeLeftPart и ChangeAuthTypeRightPart(drawable)
            Drawable FingerprintBackground = BtnRegistrationFingerprint.Background;
            Drawable LoginPasswordBackground = BtnRegistrationLoginPassword.Background;

            GradientDrawable FingerprintGradient = (GradientDrawable)FingerprintBackground;
            GradientDrawable LoginPasswordGradient = (GradientDrawable)LoginPasswordBackground;


            FragmentTransaction transaction = this.FragmentManager.BeginTransaction();

            RegistrationFingerprintActivity Fingerprint = new RegistrationFingerprintActivity();
            transaction.Replace(Resource.Id.FrameLayoutRegistration, Fingerprint);
            transaction.Commit();

            LoginPasswordGradient.SetColor(ContextCompat.GetColor(this, Resource.Color.NotActivPageColor));
            FingerprintGradient.SetColor(ContextCompat.GetColor(this, Resource.Color.BackgroundColor));

            BtnRegistrationFingerprint.Click += async (s, e) =>
            {
                try
                {
                    FragmentTransaction TFingerprint = this.FragmentManager.BeginTransaction();
                    RegistrationFingerprintActivity Fingerprint = new RegistrationFingerprintActivity();
                    TFingerprint.Replace(Resource.Id.FrameLayoutRegistration, Fingerprint);
                    TFingerprint.Commit();
                    FingerprintGradient.SetColor(ContextCompat.GetColor(this, Resource.Color.BackgroundColor));
                    LoginPasswordGradient.SetColor(ContextCompat.GetColor(this, Resource.Color.NotActivPageColor));
                }
                catch (System.Exception ex)
                {
                    Toast.MakeText(this, ex.Message, ToastLength.Long).Show();
                }
            };

            BtnRegistrationLoginPassword.Click += async (s, e) =>
            {
                try
                {
                    FragmentTransaction TLoginPassword = this.FragmentManager.BeginTransaction();
                    RegistrationLoginPasswordActivity LoginPassword = new RegistrationLoginPasswordActivity();
                    TLoginPassword.Replace(Resource.Id.FrameLayoutRegistration, LoginPassword);
                    TLoginPassword.Commit();
                    FingerprintGradient.SetColor(ContextCompat.GetColor(this, Resource.Color.NotActivPageColor));
                    LoginPasswordGradient.SetColor(ContextCompat.GetColor(this, Resource.Color.BackgroundColor));
                }
                catch (System.Exception ex)
                {
                    Toast.MakeText(this, ex.Message, ToastLength.Long).Show();
                }
            };

            BtnRegistration.Click += async (s, e) =>
            {
                Toast.MakeText(this, "Регистрация прошла успешно ! " +
                    "Чтобы войти в приложение, введите свой логин и пароль на странице авторизации", ToastLength.Long).Show();
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
    }
}