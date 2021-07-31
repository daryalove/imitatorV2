using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Imitator.WebServices.Device;

namespace Imitator.Android.Activity.MainFunctionality
{
    [Obsolete]
    public class ActivitySetCancelAlarm :Fragment
    {
        private RadioButton OverweightAlarm { get; set; }
        private RadioButton UnauthorizedRollerActivationAlarm { get; set; }
        private RadioButton LiquidLeakAlarm { get; set; }
        private Button BtnSetAlarm { get; set; }
        private Button BtnCancelAlarm { get; set; }
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.PageSetCancelAlarm, container, false);

            OverweightAlarm = view.FindViewById<RadioButton>(Resource.Id.OverweightAlarm);
            UnauthorizedRollerActivationAlarm = view.FindViewById<RadioButton>(Resource.Id.UnauthorizedRollerActivationAlarm);
            LiquidLeakAlarm = view.FindViewById<RadioButton>(Resource.Id.LiquidLeakAlarm);

            BtnSetAlarm = view.FindViewById<Button>(Resource.Id.BtnSetAlarm);
            BtnCancelAlarm = view.FindViewById<Button>(Resource.Id.BtnCancelAlarm);
            BtnSetAlarm.Click += SetAlarm_Click;
            BtnCancelAlarm.Click += CancelAlarm_Click;

            //OverweightAlarm.Click += RadioButtonClick;
            //UnauthorizedRollerActivationAlarm.Click += RadioButtonClick;
            //LiquidLeakAlarm.Click += RadioButtonClick;
            return view;            
        }

        private void CancelAlarm_Click(object sender, EventArgs e)
        {
            if (OverweightAlarm.Checked)
            {
                CancelAlarmMethod("1");
            }
            else if (UnauthorizedRollerActivationAlarm.Checked)
            {
                CancelAlarmMethod("2");
            }
            else if (LiquidLeakAlarm.Checked)
            {
                CancelAlarmMethod("3");
            }
        }

        private void SetAlarm_Click(object sender, EventArgs e)
        {
            if (OverweightAlarm.Checked)
            {
                SetAlarmMethod("1");
            }
            else if (UnauthorizedRollerActivationAlarm.Checked)
            {
                SetAlarmMethod("2");
            }
            else if (LiquidLeakAlarm.Checked)
            {
                SetAlarmMethod("3");
            }            
        }

        private async void SetAlarmMethod(string AlarmId)
        {
            var o_data = await SensorsService.MakeRequestAlarm(AlarmId);

            if (o_data.Status.ToString() == "0")
            {
                Toast.MakeText(Activity, "Тревога установлена успешно.", ToastLength.Long).Show();
            }
            else
            {
                Toast.MakeText(Activity, "Не получилось установить тревогу. Ошибка: " + o_data.Message, ToastLength.Long).Show();
            }
        }

        private async void  CancelAlarmMethod(string AlarmId)
        {
            var o_data = await SensorsService.CancelAlarm(AlarmId);

            if (o_data.Status.ToString() == "0")
            {
                Toast.MakeText(Activity, "Тревога была успешно отменена.", ToastLength.Long).Show();
            }
            else
            {
                Toast.MakeText(Activity, "Не получилось отменить тревогу. Ошибка: " + o_data.Message, ToastLength.Long).Show();
            }
        }

        //private void RadioButtonClick(object sender, EventArgs e)
        //{
        //    RadioButton rb = (RadioButton)sender;
        //}
    }

}