using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using Plugin.Settings;
using System;

namespace Imitator.Android.Activity.Authorisation
{
    [Obsolete]
    public class AuthLoginPasswordActivity : Fragment
    {
        private EditText UserLogin;
        private EditText UserPassword;
        private CheckBox RememberData;
        bool CheckedEvent;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.AuthorisationLoginPassword, container, false);

            UserLogin = view.FindViewById<EditText>(Resource.Id.UserLoginAuthorisation);
            UserPassword = view.FindViewById<EditText>(Resource.Id.UserPasswordAuthorisation);
            RememberData = view.FindViewById<CheckBox>(Resource.Id.CheckBoxRememberData);

            int x = 5;
            ref int xRef = ref x;
            UserLogin.TextChanged += UserLogin_TextChanged;
            UserPassword.TextChanged += UserPassword_TextChanged;

            RememberData.Click += RememberData_Click;

            if (CrossSettings.Current.GetValueOrDefault("CheckedEventAuthorisation", string.Empty) != "")
            {
                CheckedEvent = Convert.ToBoolean(CrossSettings.Current.GetValueOrDefault("CheckedEventAuthorisation", string.Empty));
                UserLogin.Text = CrossSettings.Current.GetValueOrDefault("UserLoginAuthorisation", "");
                UserPassword.Text = CrossSettings.Current.GetValueOrDefault("UserPasswordAuthorisation", "");
                RememberData.Checked = CheckedEvent ? true : false;
            }            

            return view;
        }

        private void RememberData_Click(object sender, EventArgs e)
        {
            CheckedPropertyCheck();
        }

        private void CheckedPropertyCheck()
        {
            if (RememberData.Checked)
            {
                CrossSettings.Current.AddOrUpdateValue("UserLoginAuthorisation", UserLogin.Text);
                CrossSettings.Current.AddOrUpdateValue("UserPasswordAuthorisation", UserPassword.Text);
                CrossSettings.Current.AddOrUpdateValue("CheckedEventAuthorisation", "true");
            }
            else
            {
                CrossSettings.Current.AddOrUpdateValue("UserLoginAuthorisation", "");
                CrossSettings.Current.AddOrUpdateValue("UserPasswordAuthorisation", "");
                CrossSettings.Current.AddOrUpdateValue("CheckedEventAuthorisation", "false");
            }
        }

        private void UserPassword_TextChanged(object sender, global::Android.Text.TextChangedEventArgs e)
        {
            StaticDataClass.UserPassword = UserPassword.Text;           
            CheckedPropertyCheck();
        }

        private void UserLogin_TextChanged(object sender, global::Android.Text.TextChangedEventArgs e)
        {
            StaticDataClass.UserLogin = UserLogin.Text;
            CheckedPropertyCheck();
        }
    }
}