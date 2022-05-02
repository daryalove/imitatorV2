using Xamarin.Essentials;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V7.App;
using Android.Text;
using Android.Widget;
using Imitator.CommonData.DataModels;
using Imitator.CommonData.ViewModels;
using Imitator.WebServices;
using Imitator.WebServices.Account;
using Imitator.WebServices.Device;
using Plugin.Settings;
using Android.Telephony;
using Settings = Android.Provider.Settings;
using System;

namespace Imitator.Android.Activity.Registration
{
    [Activity(Label = "RegistrationActivity")]
    public class RegistrationActivity : AppCompatActivity
    {
        private EditText UserFirstName;
        private EditText UserLastName;
        private EditText UserLogin;
        private EditText UserPassword;
        private Button BtnRegistration;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.PageRegistration);

            UserFirstName = FindViewById<EditText>(Resource.Id.UserLastNameRegistration);
            UserLastName = FindViewById<EditText>(Resource.Id.UserFirstNameRegistration);
            UserLogin = FindViewById<EditText>(Resource.Id.UserLoginRegistration);
            UserPassword = FindViewById<EditText>(Resource.Id.UserPasswordRegistration);
            BtnRegistration = FindViewById<Button>(Resource.Id.BtnRegistration);

            UserFirstName.TextChanged += UserFirstName_TextChanged;
            UserLastName.TextChanged += UserLastName_TextChanged;
            UserLogin.TextChanged += UserLogin_TextChanged;
            UserPassword.TextChanged += UserPassword_TextChanged;
           

            BtnRegistration.Click += async (s, e) =>
            {
                RegistrationUserMethod();
            };
            
        }

        private void UserLastName_TextChanged(object sender, TextChangedEventArgs e)
        {
            CrossSettings.Current.AddOrUpdateValue("UserLastNameRegistration", UserLastName.Text);
        }

        private void UserFirstName_TextChanged(object sender, TextChangedEventArgs e)
        {
            CrossSettings.Current.AddOrUpdateValue("UserFirstNameRegistration", UserFirstName.Text);
        }

        private void UserPassword_TextChanged(object sender, global::Android.Text.TextChangedEventArgs e)
        {
            CrossSettings.Current.AddOrUpdateValue("UserPasswordRegistration", UserPassword.Text);
        }

        private void UserLogin_TextChanged(object sender, global::Android.Text.TextChangedEventArgs e)
        {
            CrossSettings.Current.AddOrUpdateValue("UserLoginRegistration", UserLogin.Text);
        }

        private async void RegistrationBoxMethod()
        {
            var o_data = await SensorsService.RegisterBox(StaticBox.IMEI);

            if (o_data.Result.ToString() == "OK")
            {
                Toast.MakeText(this, "Регистрация прошла успешно !", ToastLength.Long).Show();

                Intent intent = new Intent(this, typeof(Activity.ActivityMainFunctionality));
                StartActivity(intent);
            }
            else
                Toast.MakeText(this, "Ошибка: " + o_data.ErrorInfo, ToastLength.Long).Show();
        }

        private async void RegistrationDeviceMethod()
        {
            try
            {
                TelephonyManager telephonyManager;
                // Android.Telephony.TelephonyManager mTelephonyMgr = (Android.Telephony.TelephonyManager)GetSystemService(Android.Content.Context.TelephonyService);
                //Telephone Number  
                telephonyManager = (TelephonyManager)GetSystemService(TelephonyService);
                string IMEI = "";
                if(Settings.Secure.GetString(ContentResolver, Settings.Secure.AndroidId) != null)
                {
                    IMEI = Settings.Secure.GetString(ContentResolver, Settings.Secure.AndroidId);
                }
                //if (telephonyManager.DeviceId != null)
                //{
                //    //IMEI number 
                //    IMEI = telephonyManager.DeviceId;
                //}
                //else if (Settings.Secure.GetString(ContentResolver, Settings.Secure.AndroidId) != null)
                //{
                //    //Android ID 
                //    IMEI = Settings.Secure.GetString(ContentResolver, Settings.Secure.AndroidId);
                //}

                using (var client = ClientHelper.GetClient(StaticUser.Token))
                {
                    SensorsService.InitializeClient(client);
                    DeviceModel model = new DeviceModel
                    {
                        Idiom = DeviceInfo.Idiom.ToString(),
                        IMEI = IMEI,
                        Manufacturer = DeviceInfo.Manufacturer,
                        ModelName = DeviceInfo.Model,
                        Name = DeviceInfo.Name,
                        Platform = DeviceInfo.Platform.ToString(),
                        Version = DeviceInfo.VersionString
                    };

                    var o_data = await SensorsService.RegisterDevice(model);

                    if (o_data.Result.ToString() == "OK")
                    {
                        StaticBox.DeviceId = o_data.Id;
                        StaticBox.IMEI = o_data.IMEI;
                        RegistrationBoxMethod();
                        //ShowAlertDialog("Ваше устройство(телефон) зарегистрированно. Осталось зарегистрировать виртуальный контейнер. " +
                        //    "Для этого нажмите ещё раз на кнопку «Сохранить»");
                        //Toast.MakeText(this, o_data.SuccessInfo, ToastLength.Long).Show();
                    }
                    else
                        Toast.MakeText(this, "Ошибка: " + o_data.ErrorInfo, ToastLength.Long).Show();
                }
            }
            catch (Exception ex)
            {
                Toast.MakeText(this, ex.Message, ToastLength.Long).Show();
            }
            
        }

        private async void AuthorisationUserMethod()
        {
            using (var client = ClientHelper.GetClient())
            {
                AuthService.InitializeClient(client);
                UserShortModel o_data = null;

                var UserLogin = CrossSettings.Current.GetValueOrDefault("UserLoginRegistration", ""); 
                var UserPassword = CrossSettings.Current.GetValueOrDefault("UserPasswordRegistration", "");

                o_data = await AuthService.Login(UserLogin, UserPassword);

                if (o_data.Result.ToString() == "OK")
                {
                    //Toast.MakeText(this, "Пользователь: " + o_data.UserFIO, ToastLength.Long).Show();
                    //Toast.MakeText(this, "Информация: " + o_data.Token, ToastLength.Long).Show();
                    StaticUser.Token = o_data.Token;
                    RegistrationDeviceMethod();
                }
                else
                    Toast.MakeText(this, "Ошибка: " + o_data.ErrorInfo, ToastLength.Long).Show();
            }
        }

        private async void RegistrationUserMethod()
        {
            UserModel user = new UserModel
            {
                UserFIO = CrossSettings.Current.GetValueOrDefault("UserLastNameRegistration", "") + " "
                   + CrossSettings.Current.GetValueOrDefault("UserFirstNameRegistration", ""),
                Email = CrossSettings.Current.GetValueOrDefault("UserLoginRegistration", ""),
                Password = CrossSettings.Current.GetValueOrDefault("UserPasswordRegistration", ""),
            };

            if(user.UserFIO != null && user.Email != null && user.Password != null)
            {

                using (var client = ClientHelper.GetClient())
                {
                    AuthService.InitializeClient(client);
                    UserShortModel o_data = null;

                    o_data = await AuthService.Register(user);

                    if (o_data.Result.ToString() == "OK")
                    {
                        AuthorisationUserMethod();
                    }
                    else if(o_data.Result.ToString() == "BadRequest")
                    {
                        Toast.MakeText(this, "Мобильное устройство уже зарегистрировано. " +
                            "Просьба выполнить вход под текущем пользователем", ToastLength.Long).Show();
                    }
                    else
                        Toast.MakeText(this, "Ошибка: " + o_data.ErrorInfo, ToastLength.Long).Show();
                }

            }
            else
            {
                Toast.MakeText(this, "Вы ввели не все данные !", ToastLength.Long).Show();
            }
        }

    }
}