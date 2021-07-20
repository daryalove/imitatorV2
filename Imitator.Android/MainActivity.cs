using Android.App;
using Android.OS;
using Android.Content.PM;
using Android.Widget;
using Android.Support.V7.App;
using Android.Content;
using Android.Runtime;
using Imitator.Android.Activity.Registration;
using Android.Support.Design.Widget;

namespace Imitator.Android
{
    [Activity(Label = "@string/app_name", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        /// <summary>
        /// Кнoпка прехода на форму авторизации.
        /// </summary>
        private Button BtnOpenAuthorisationPage;
        private Button BtnOpenRegistrationPage;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.PageMain);

            BtnOpenAuthorisationPage = FindViewById<Button>(Resource.Id.BtnOpenAuthorisationPage);
            BtnOpenRegistrationPage = FindViewById<Button>(Resource.Id.BtnOpenRegistrationPage);

            BtnOpenRegistrationPage.Click += async (s, e) =>
            {
                try
                {
                    Intent Auth = new Intent(this, typeof(RegistrationActivity));
                    StartActivity(Auth);
                    this.Finish();
                }
                catch (System.Exception ex)
                {
                    Toast.MakeText(this, ex.Message, ToastLength.Long).Show();
                }
            };

            BtnOpenAuthorisationPage.Click += async (s, e) =>
            {
                try
                {
                    Intent Auth = new Intent(this, typeof(Activity.AuthorisationActivity));
                    StartActivity(Auth);
                    this.Finish();
                }
                catch (System.Exception ex)
                {
                    Toast.MakeText(this, ex.Message, ToastLength.Long).Show();
                }
            };

            string dir_path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
        }

        ////permissions events
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }


        //public override void OnCreate(Bundle savedInstanceState)
        //{
        //    base.OnCreate(savedInstanceState);
        //}
        //public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        //{
        //    var view = inflater.Inflate(Resource.Layout.PageMain, container, false);

        //    FragmentTransaction transaction = this.FragmentManager.BeginTransaction();
        //    BtnOpenAuthorisationPage = view.FindViewById<Button>(Resource.Id.BtnOpenAuthorisationPage);

        //    BtnOpenAuthorisationPage.Click += async (s, e) =>
        //    {

        //        try
        //        {
        //            AuthorisationActivity _authorisationActivity = new AuthorisationActivity();
        //            transaction.Replace(Resource.Id.framelayout, _authorisationActivity);
        //            transaction.Commit();
        //        }
        //        catch (System.Exception ex)
        //        {
        //            Toast.MakeText(Activity, ex.Message, ToastLength.Long).Show();
        //        }
        //    };

        //    return view;
        //}       
    }


}

