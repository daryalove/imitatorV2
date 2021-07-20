using Android.App;
using Android.Content;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.Content;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;

namespace Imitator.Android.Activity.PersonalData
{

    [Activity(Label = "ActivityPersonalData")]
    public class ActivityPersonalData : AppCompatActivity
    {
        private Button BtnPersonalDataFingerprint;
        private Button BtnPersonalDataLoginPassword;
        private Button BtnPersonalData;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.PagePersonalData);

            BtnPersonalDataFingerprint = FindViewById<Button>(Resource.Id.BtnPersonalDataFingerprint);
            BtnPersonalDataLoginPassword = FindViewById<Button>(Resource.Id.BtnPersonalDataLoginPassword);
            BtnPersonalData = FindViewById<Button>(Resource.Id.BtnPersonalData);

            Drawable FingerprintBackground = BtnPersonalDataFingerprint.Background;
            Drawable LoginPasswordBackground = BtnPersonalDataLoginPassword.Background;

            GradientDrawable FingerprintGradient = (GradientDrawable)FingerprintBackground;
            GradientDrawable LoginPasswordGradient = (GradientDrawable)LoginPasswordBackground;


            FragmentTransaction transaction = this.FragmentManager.BeginTransaction();

            PersonalDataFingerprint Fingerprint = new PersonalDataFingerprint();
            transaction.Replace(Resource.Id.FrameLayoutPersonalData, Fingerprint);
            transaction.Commit();

            LoginPasswordGradient.SetColor(ContextCompat.GetColor(this, Resource.Color.NotActivPageColor));
            FingerprintGradient.SetColor(ContextCompat.GetColor(this, Resource.Color.BackgroundColor));

            BtnPersonalDataFingerprint.Click += async (s, e) =>
            {
                try
                {
                    FragmentTransaction TFingerprint = this.FragmentManager.BeginTransaction();
                    PersonalDataFingerprint Fingerprint = new PersonalDataFingerprint();
                    TFingerprint.Replace(Resource.Id.FrameLayoutPersonalData, Fingerprint);
                    TFingerprint.Commit();
                    FingerprintGradient.SetColor(ContextCompat.GetColor(this, Resource.Color.BackgroundColor));
                    LoginPasswordGradient.SetColor(ContextCompat.GetColor(this, Resource.Color.NotActivPageColor));
                }
                catch (System.Exception ex)
                {
                    Toast.MakeText(this, ex.Message, ToastLength.Long).Show();
                }
            };

            BtnPersonalDataLoginPassword.Click += async (s, e) =>
            {
                try
                {
                    FragmentTransaction TLoginPassword = this.FragmentManager.BeginTransaction();
                    PersonalDataLoginPassword LoginPassword = new PersonalDataLoginPassword();
                    TLoginPassword.Replace(Resource.Id.FrameLayoutPersonalData, LoginPassword);
                    TLoginPassword.Commit();
                    FingerprintGradient.SetColor(ContextCompat.GetColor(this, Resource.Color.NotActivPageColor));
                    LoginPasswordGradient.SetColor(ContextCompat.GetColor(this, Resource.Color.BackgroundColor));
                }
                catch (System.Exception ex)
                {
                    Toast.MakeText(this, ex.Message, ToastLength.Long).Show();
                }
            };

            BtnPersonalData.Click += async (s, e) =>
            {
                Toast.MakeText(this, "Изменения успешно внесены ! ", ToastLength.Long).Show();
            };
        }

    }
}