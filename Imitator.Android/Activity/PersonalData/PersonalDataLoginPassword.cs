using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using Plugin.Settings;
using System;

namespace Imitator.Android.Activity.PersonalData
{
    [Obsolete]
    public class PersonalDataLoginPassword : Fragment
    {
        private View view;
        private TextView password;
        private ImageButton hideButton;
        private TextView lastName;
        private TextView firstName;
        private TextView login;

        private bool clicked = true;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            view = inflater.Inflate(Resource.Layout.PagePersonalDataLoginPassword, container, false);
            password = view.FindViewById<TextView>(Resource.Id.PasswordPersonalData);
            hideButton = view.FindViewById<ImageButton>(Resource.Id.BtnPasswordShowHide);
            lastName = view.FindViewById<TextView>(Resource.Id.LastNamePersonalData);
            firstName = view.FindViewById<TextView>(Resource.Id.FirstNamePersonalData);
            login = view.FindViewById<TextView>(Resource.Id.LoginPersonalData);

            firstName.Text = CrossSettings.Current.GetValueOrDefault("UserFirstNameRegistration", "");
            lastName.Text = CrossSettings.Current.GetValueOrDefault("UserLastNameRegistration", "");
            login.Text = CrossSettings.Current.GetValueOrDefault("UserLoginRegistration", "");

            hideButton.Click += HideButton_Click;

            return view;
        }

        private void HideButton_Click(object sender, EventArgs e)
        {
            if (clicked)
            {
                password.Text = CrossSettings.Current.GetValueOrDefault("UserPasswordRegistration", "");
                clicked = false;
            }
            else
            {
                password.Text = "***********";
                clicked = true;
            }
        }
    }
}