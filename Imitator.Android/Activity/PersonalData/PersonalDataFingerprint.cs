using System;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.Content;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.Widget;
using Plugin.Settings;

namespace Imitator.Android.Activity.PersonalData
{
    [Obsolete]
    public class PersonalDataFingerprint : Fragment
    {
        private View view;
        private SwitchCompat ChangeAccessFingerprintAuthorisation;
        private TextView TextAccessFingerprintAuthorisation;
        bool AccessEvent;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            view = inflater.Inflate(Resource.Layout.PagePersonalDataFingerprint, container, false);

            ChangeAccessFingerprintAuthorisation = view.FindViewById<SwitchCompat>(Resource.Id.ChangeAccessFingerprintAuthorisation);
            TextAccessFingerprintAuthorisation = view.FindViewById<TextView>(Resource.Id.TextAccessFingerprintAuthorisation);
            ChangeAccessFingerprintAuthorisation.Click += ChangeAccessFingerprintAuthorisation_Click;

            if (CrossSettings.Current.GetValueOrDefault("AccessFingerprintAuthorisation", "") != "")
            {
                AccessEvent = Convert.ToBoolean(CrossSettings.Current.GetValueOrDefault("AccessFingerprintAuthorisation", string.Empty));

                ChangeAccessFingerprintAuthorisation.Checked = AccessEvent ? true : false;

                if (AccessEvent)
                {
                    TextAccessFingerprintAuthorisation.Text = "Авторизация с помощью отпечатка пальца разрешена";
                    TextAccessFingerprintAuthorisation.SetTextColor(new Color(ContextCompat.GetColor(Activity, Resource.Color.BackgroundColor)));
                }    
                else
                {
                    TextAccessFingerprintAuthorisation.Text = "Авторизация с помощью отпечатка пальца запрещена";
                    TextAccessFingerprintAuthorisation.SetTextColor(new Color(ContextCompat.GetColor(Activity, Resource.Color.EventNotAllowColor)));
                }
            }

            return view;
        }

        private void ChangeAccessFingerprintAuthorisation_Click(object sender, EventArgs e)
        {
            if (ChangeAccessFingerprintAuthorisation.Checked)
            {
                AlertDialog.Builder alert = new AlertDialog.Builder(Activity);
                alert.SetTitle("Подтверждение");
                alert.SetMessage("Вы действительно хотети разрешить авторизацию с помощью отпечаток пальца ?");
                alert.SetPositiveButton("Да", (senderAlert, args) =>
                {
                    ChangeAccessFingerprintAuthorisation.Checked = true;
                    TextAccessFingerprintAuthorisation.Text = "Авторизация с помощью отпечатка пальца разрешена";
                    TextAccessFingerprintAuthorisation.SetTextColor(new Color(ContextCompat.GetColor(Activity, Resource.Color.BackgroundColor)));
                    CrossSettings.Current.AddOrUpdateValue("AccessFingerprintAuthorisation", "true");
                });
                alert.SetNegativeButton("Отмена", (senderAlert, args) =>
                {
                    ChangeAccessFingerprintAuthorisation.Checked = false;
                });
                Dialog dialog = alert.Create();
                dialog.Show();
            }
            else
            {
                AlertDialog.Builder alert = new AlertDialog.Builder(Activity);
                alert.SetTitle("Подтверждение");
                alert.SetMessage("Вы действительно хотети запретить авторизацию с помощью отпечаток пальца ?");
                alert.SetPositiveButton("Да", (senderAlert, args) =>
                {
                    ChangeAccessFingerprintAuthorisation.Checked = false;
                    TextAccessFingerprintAuthorisation.Text = "Авторизация с помощью отпечатка пальца запрещена";
                    TextAccessFingerprintAuthorisation.SetTextColor(new Color(ContextCompat.GetColor(Activity, Resource.Color.EventNotAllowColor)));
                    CrossSettings.Current.AddOrUpdateValue("AccessFingerprintAuthorisation", "false");
                });
                alert.SetNegativeButton("Отмена", (senderAlert, args) =>
                {
                    ChangeAccessFingerprintAuthorisation.Checked = true;
                });
                Dialog dialog = alert.Create();
                dialog.Show();
            }
            
        }

    }
}