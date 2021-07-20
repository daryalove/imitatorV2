using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Hardware;
using Android.Media;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Util;
using Android.Views;
using Android.Widget;
using Com.Akaita.Android.Circularseekbar;
using Xamarin.Essentials;
using static Android.Views.View;
using static Com.Akaita.Android.Circularseekbar.CircularSeekBar;

namespace Imitator.Android.Activity.MainFunctionality
{
    [Obsolete]
    public class ActivitySensorsData : Fragment, IOnTouchListener/*, IOnScrollChangeListener*/
    {
        #region Объявление переменных пользовательсокго интерфейса

        private ScrollView scrollView;
        private View view;

        private ImageView SelectedSensorImage;
        private TextView SelectedSensorName;
        private TextView SelectedSensorValue;

        private TextView WeightText;
        private TextView TemperaturText;
        private TextView HumidityText;
        private TextView IlluminationText;
        private TextView BatteryText;

        private TextView WeightValue;
        private TextView TemperaturValue;
        private TextView HumidityValue;
        private TextView IlluminationValue;
        private TextView BatteryValue;

        //private View WeightView;
        //private View TemperaturView;
        //private View HumidityView;
        //private View IlluminationView;
        //private View BatteryView;

        private CircularSeekBar seekBar;

        private Button BtnIncreaseSensorValue;
        private Button BtnReduceSensorValue;

        #endregion

        private string[] mViews = new string[2] { "0", "0" };
        private string Unit;
        private List<Sensor> sensors = new List<Sensor>();
        private bool AccessToSeekBar = false; // При выполнении кода в функиции ChangeSeekBarCharacteristic() 
        //программа обращается к методу SeekBar_ProgressChanged и меняет значение поля SelectedSensorValue.
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            view = inflater.Inflate(Resource.Layout.PageSensorsData, container, false);
            scrollView = view.FindViewById<ScrollView>(Resource.Id.ScrollViewPageSensorsData);
            try
            {
                #region Инициализация переменных пользовательсокго интерфейса

                SelectedSensorImage = view.FindViewById<ImageView>(Resource.Id.SelectedSensorImage);
                SelectedSensorName = view.FindViewById<TextView>(Resource.Id.SelectedSensorName);
                SelectedSensorValue = view.FindViewById<TextView>(Resource.Id.SelectedSensorValue);

                WeightText = view.FindViewById<TextView>(Resource.Id.SensorWeightText);
                TemperaturText = view.FindViewById<TextView>(Resource.Id.SensorTemperatureText);
                HumidityText = view.FindViewById<TextView>(Resource.Id.SensorHumidityText);
                IlluminationText = view.FindViewById<TextView>(Resource.Id.SensorIlluminationText);
                BatteryText = view.FindViewById<TextView>(Resource.Id.SensorBatteryText);

                Sensor weight = new Sensor(0, 5000, "Вес", " кг", Resource.Drawable.SensorWeightImage);
                weight.linearLayout = view.FindViewById<LinearLayout>(Resource.Id.SensorWeightLinearLayout);
                weight.currentValue = view.FindViewById<TextView>(Resource.Id.SensorWeightValue);

                Sensor temperatur = new Sensor(-45, 80, "Температура", " °C", Resource.Drawable.SensorTemperatureImage);
                temperatur.linearLayout = view.FindViewById<LinearLayout>(Resource.Id.SensorTemperatureLinearLayout);
                temperatur.currentValue = view.FindViewById<TextView>(Resource.Id.SensorTemperatureValue);

                Sensor humidity = new Sensor(0, 100, "Влажность", " %", Resource.Drawable.SensorHumidityImage);
                humidity.linearLayout = view.FindViewById<LinearLayout>(Resource.Id.SensorHumidityLinearLayout);
                humidity.currentValue = view.FindViewById<TextView>(Resource.Id.SensorHumidityValue);

                Sensor illumination = new Sensor(0, 1000, "Освещённость", " лм", Resource.Drawable.SensorIlluminationImage);
                illumination.linearLayout = view.FindViewById<LinearLayout>(Resource.Id.SensorIlluminationLinearLayout);
                illumination.currentValue = view.FindViewById<TextView>(Resource.Id.SensorIlluminationValue);

                Sensor battery = new Sensor(0, 16, "Батарея", " %", Resource.Drawable.SensorBatteryImage);
                battery.linearLayout = view.FindViewById<LinearLayout>(Resource.Id.SensorBatteryLinearLayout);
                battery.currentValue = view.FindViewById<TextView>(Resource.Id.SensorBatteryValue);

                sensors.Add(weight);
                sensors.Add(temperatur);
                sensors.Add(humidity);
                sensors.Add(illumination);
                sensors.Add(battery);

                WeightText.Click += SelectedSensor_Click;
                TemperaturText.Click += SelectedSensor_Click;
                HumidityText.Click += SelectedSensor_Click;
                IlluminationText.Click += SelectedSensor_Click;
                BatteryText.Click += SelectedSensor_Click;

                seekBar = view.FindViewById<CircularSeekBar>(Resource.Id.SeekBarSelectedCharacteristic);

                BtnIncreaseSensorValue = view.FindViewById<Button>(Resource.Id.BtnIncreaseSensorValue);
                BtnReduceSensorValue = view.FindViewById<Button>(Resource.Id.BtnReduceSensorValue);

                #endregion

                BtnIncreaseSensorValue.Click += BtnIncreaseSensorValue_Click;
                BtnReduceSensorValue.Click += BtnReduceSensorValue_Click;

                SelectedSensorImage.SetImageResource(Resource.Drawable.SensorWeightImage);

                string[] subs = SelectedSensorValue.Text.Split(' ');
                seekBar.Progress = float.Parse(subs[0]);
                seekBar.Min = 0;
                seekBar.Max = 5000;
                AccessToSeekBar = true;
                seekBar.SetOnTouchListener(this);
                seekBar.ProgressChanged += SeekBar_ProgressChanged;
                //seekBar.CenterClicked += (s, e) =>
                //{
                //    CircularSeekBar view = (CircularSeekBar)e.P0;
                //    float progress = e.P1;
                //    Snackbar.Make(view, "Rest", Snackbar.LengthShort).Show();
                //    view.Progress = 0;
                //};
            }
            catch (System.Exception ex)
            {
                Toast.MakeText(Activity, ex.Message, ToastLength.Long).Show();
            }
        

            //seekBar.ProgressChanged += (s, e) =>
            //{
            //    SelectedSensorValue.Text = seekBar.Progress.ToString();
            //    //CircularSeekBar view = e.P0;
            //    //float progress = e.P1;
            //    //bool fromUser = e.P2;
            //    //if (progress < 33)
            //    //    view.RingColor = Color.Green;
            //    //else if (progress < 66)
            //    //    view.RingColor = Color.Blue;
            //    //else
            //    //    view.RingColor = Color.Red;
            //};

            return view;
        }


        private void SeekBar_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (AccessToSeekBar)
            {
                SelectedSensorValue.Text = Math.Round(seekBar.Progress, 1).ToString() + Unit;
            }            
            //CircularSeekBar view = e.P0;
            //float progress = e.P1;
            //bool fromUser = e.P2;
            //if (progress < 33)
            //    view.RingColor = Color.Green;
            //else if (progress < 66)
            //    view.RingColor = Color.Blue;
            //else
            //    view.RingColor = Color.Red;
        }

        private void BtnReduceSensorValue_Click(object sender, EventArgs e)
        {
            try
            {
                var mText = SelectedSensorValue.Text.Split(" ");
                var sensorValue = Math.Round(Convert.ToDecimal(mText[0]),1);
                var item = sensors.Find(x => x.SensorName == SelectedSensorName.Text);
                if (sensorValue > item.MinValue)
                {
                    sensorValue--;
                    SelectedSensorValue.Text = sensorValue.ToString() + item.Unit;
                    seekBar.Progress--;
                }
                else
                {
                    Toast.MakeText(Activity, "Нельзя установить число меньше допустимого.", ToastLength.Long).Show();
                }
            }
            catch (System.Exception ex)
            {
                Toast.MakeText(Activity, ex.Message, ToastLength.Long).Show();
            }           
        }

        private void BtnIncreaseSensorValue_Click(object sender, EventArgs e)
        {
            try
            {
                var mText = SelectedSensorValue.Text.Split(" ");
                var sensorValue = Math.Round(Convert.ToDecimal(mText[0]), 1);
                var item = sensors.Find(x => x.SensorName == SelectedSensorName.Text);
                if (sensorValue < item.MaxValue)
                {
                    sensorValue++;
                    SelectedSensorValue.Text = sensorValue.ToString() + item.Unit;
                    seekBar.Progress++;
                }
                else
                {
                    Toast.MakeText(Activity, "Нельзя установить число больше допустимого.", ToastLength.Long).Show();
                }
            }
            catch (System.Exception ex)
            {
                Toast.MakeText(Activity, ex.Message, ToastLength.Long).Show();
            }
        }

        private void SelectedSensor_Click(object sender, EventArgs e)
        {
            AccessToSeekBar = false;
            var text = ((TextView)sender).Text;
            var item = sensors.Find(x => x.SensorName == text);
            mViews[0] = mViews[1];
            if (mViews[0] != "0")
                ChangePreviousViewBackground();
            Unit = item.ChangeCurrentViewsBackground(ref mViews, ref SelectedSensorValue, ref SelectedSensorName, ref SelectedSensorImage);            
            ChangeSeekBarCharacteristic();
            AccessToSeekBar = true;
            
        }

        private void ChangeSeekBarCharacteristic()
        {
            var item = sensors.Find(x => x.SensorName == mViews[1]);
            var isChangeSeccessful = item.SetSensorCharacteristicsForSeekBar(ref seekBar, SelectedSensorValue.Text);
            if (!(isChangeSeccessful))
            {
                Toast.MakeText(Activity, "Что-то пошло не так...", ToastLength.Long).Show();
            }
        }
        private void ChangePreviousViewBackground()
        {
            AccessToSeekBar = false;
            var item = sensors.Find(x => x.SensorName == mViews[0]);
            item.linearLayout.SetBackgroundResource(Resource.Color.TransparentColor);
            item.currentValue.Text = SelectedSensorValue.Text;
        }

        public bool OnTouch(View v, MotionEvent e)
        {
            if (e.Action == MotionEventActions.Down)
                scrollView.Enabled = false;
            else if (e.Action == MotionEventActions.Up)
                scrollView.Enabled = true;
            return false;
        }

        //public void OnScrollChange(View v, int scrollX, int scrollY, int oldScrollX, int oldScrollY)
        //{
        //    throw new NotImplementedException();
        //}
    }

  
    public class Sensor
    {
        public ImageView SelectedSensorImage { get; set; }
        public LinearLayout linearLayout { get; set; }
        public TextView currentValue { get; set; }

        public int SelectedImageCode { get; set; }
        public int MaxValue { get; }
        public int MinValue { get; }
        public string SensorName { get; }
        public string Unit { get; }

        public Sensor(int MinValue, int MaxValue, string SensorName, string Unit, int SelectedImageCode)
        {
            this.MinValue = MinValue;
            this.MaxValue = MaxValue;
            this.SelectedImageCode = SelectedImageCode;
            this.SensorName = SensorName;
            this.Unit = Unit;
        }

        public string ChangeCurrentViewsBackground(ref string[] mViews, ref TextView selectedSensorValue,
            ref TextView selectedSensorName, ref ImageView SelectedSensorImage)
        {
            SelectedSensorImage.SetImageResource(SelectedImageCode);
            selectedSensorName.Text = SensorName;
            selectedSensorValue.Text = currentValue.Text;
            linearLayout.SetBackgroundResource(Resource.Drawable.EditTextStyle);
            mViews[1] = SensorName;
            return Unit;
        }
        public bool SetSensorCharacteristicsForSeekBar(ref CircularSeekBar seekBar, string selectedSensorValue)
        {
            try
            {
                seekBar.Max = MaxValue;
                seekBar.Min = MinValue;
                string[] subs = selectedSensorValue.Split(' ');
                seekBar.Progress = float.Parse(subs[0]);
                return true;
            }
            catch (System.Exception ex)
            {
                return false;
            }
        }
    }
}