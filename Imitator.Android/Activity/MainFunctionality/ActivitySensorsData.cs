using Android.App;
using Android.Graphics;
using Android.OS;
using Android.Support.V4.Content;
using Android.Views;
using Android.Widget;
using Com.Akaita.Android.Circularseekbar;
using Imitator.CommonData.DataModels;
using Imitator.CommonData.ViewModels.Responses;
using Imitator.WebServices;
using Imitator.WebServices.Device;
using System;
using System.Collections.Generic;
using static Android.Views.View;
using Settings = Android.Provider.Settings;
using static Com.Akaita.Android.Circularseekbar.CircularSeekBar;
using Android.Content;

namespace Imitator.Android.Activity.MainFunctionality
{
    [Obsolete]
    public class ActivitySensorsData : Fragment, IOnTouchListener/*, IOnScrollChangeListener*/
    {
        #region Объявление переменных пользовательсокго интерфейса

        //private ScrollView scrollView;
        private View view;

        private ImageView SelectedSensorImage;
        private TextView SelectedSensorName;
        private TextView SelectedSensorValue;

        public static string WeightName = "Вес,кг";
        public static string TempName = "Температура,°С";
        public static string HumidityName = "Влажность,%";
        public static string LightName = "Освещенность,лм";
        public static string BatteryName = "Батарея,V";

        private CircularSeekBar seekBar;
        private ProgressBar preloader;

        private Button BtnIncreaseSensorValue;
        private Button BtnReduceSensorValue;
        private Button BtnSaveChangedSensorValues;

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
            //scrollView = view.FindViewById<ScrollView>(Resource.Id.ScrollViewPageSensorsData);

            try
            {
                #region Инициализация переменных пользовательсокго интерфейса

                SelectedSensorImage = view.FindViewById<ImageView>(Resource.Id.SelectedSensorImage);
                SelectedSensorName = view.FindViewById<TextView>(Resource.Id.SelectedSensorName);
                SelectedSensorValue = view.FindViewById<TextView>(Resource.Id.SelectedSensorValue);

                seekBar = view.FindViewById<CircularSeekBar>(Resource.Id.SeekBarSelectedCharacteristic);
                preloader = view.FindViewById<ProgressBar>(Resource.Id.loader);

                BtnIncreaseSensorValue = view.FindViewById<Button>(Resource.Id.BtnIncreaseSensorValue);
                BtnReduceSensorValue = view.FindViewById<Button>(Resource.Id.BtnReduceSensorValue);
                BtnSaveChangedSensorValues = view.FindViewById<Button>(Resource.Id.BtnSaveChangedSensorValues);

                Sensor weight = new Sensor(0, 5000, "", Resource.Drawable.SensorWeightImage);
                weight.linearLayout = view.FindViewById<LinearLayout>(Resource.Id.SensorWeightLinearLayout);
                weight.CurrentValue = view.FindViewById<TextView>(Resource.Id.SensorWeightValue);                
                weight.SensorName = view.FindViewById<TextView>(Resource.Id.SensorWeightText);
                weight.SensorName.Text = WeightName;
                weight.SensorName.Click += SelectedSensor_Click;
                weight.CurrentValue.Text = StaticBox.Sensors["Вес груза"];

                Sensor temperatur = new Sensor(-45, 80, "", Resource.Drawable.SensorTemperatureImage);
                temperatur.linearLayout = view.FindViewById<LinearLayout>(Resource.Id.SensorTemperatureLinearLayout);
                temperatur.CurrentValue = view.FindViewById<TextView>(Resource.Id.SensorTemperatureValue);
                temperatur.SensorName = view.FindViewById<TextView>(Resource.Id.SensorTemperatureText);
                temperatur.SensorName.Text = TempName;
                temperatur.SensorName.Click += SelectedSensor_Click;
                temperatur.CurrentValue.Text = StaticBox.Sensors["Температура"];

                Sensor humidity = new Sensor(0, 100, "", Resource.Drawable.SensorHumidityImage);
                humidity.linearLayout = view.FindViewById<LinearLayout>(Resource.Id.SensorHumidityLinearLayout);
                humidity.CurrentValue = view.FindViewById<TextView>(Resource.Id.SensorHumidityValue);
                humidity.SensorName = view.FindViewById<TextView>(Resource.Id.SensorHumidityText);
                humidity.SensorName.Text = HumidityName;
                humidity.SensorName.Click += SelectedSensor_Click;
                humidity.CurrentValue.Text = StaticBox.Sensors["Влажность"];

                Sensor illumination = new Sensor(0, 1000, "", Resource.Drawable.SensorIlluminationImage);
                illumination.linearLayout = view.FindViewById<LinearLayout>(Resource.Id.SensorIlluminationLinearLayout);
                illumination.CurrentValue = view.FindViewById<TextView>(Resource.Id.SensorIlluminationValue);
                illumination.SensorName = view.FindViewById<TextView>(Resource.Id.SensorIlluminationText);
                illumination.SensorName.Text = LightName;
                illumination.SensorName.Click += SelectedSensor_Click;
                illumination.CurrentValue.Text = StaticBox.Sensors["Освещенность"];

                Sensor battery = new Sensor(0, 16, "", Resource.Drawable.SensorBatteryImage);
                battery.linearLayout = view.FindViewById<LinearLayout>(Resource.Id.SensorBatteryLinearLayout);
                battery.CurrentValue = view.FindViewById<TextView>(Resource.Id.SensorBatteryValue);
                battery.SensorName = view.FindViewById<TextView>(Resource.Id.SensorBatteryText);
                battery.SensorName.Text = BatteryName;
                battery.SensorName.Click += SelectedSensor_Click;
                battery.CurrentValue.Text = StaticBox.Sensors["Уровень заряда аккумулятора"];

                sensors.Add(weight);
                sensors.Add(temperatur);
                sensors.Add(humidity);
                sensors.Add(illumination);
                sensors.Add(battery);

                #endregion

                BtnIncreaseSensorValue.Click += BtnIncreaseSensorValue_Click;
                BtnReduceSensorValue.Click += BtnReduceSensorValue_Click;
                BtnSaveChangedSensorValues.Click += BtnSaveChangedSensorValues_Click;

                SelectedSensorImage.SetImageResource(Resource.Drawable.SensorTemperatureImage);
                SelectedSensorName.Text = sensors.Find(w => w.SensorName.Text == TempName).SensorName.Text;


                seekBar.Min = sensors.Find(w => w.SensorName.Text == TempName).MinValue;
                seekBar.Max = sensors.Find(w => w.SensorName.Text == TempName).MaxValue;
                AccessToSeekBar = true;
                seekBar.SetOnTouchListener(this);
                seekBar.ProgressChanged += SeekBar_ProgressChanged;

                // Если данные с сервера уже были получены, не отправлять запрос на сервер, а воспользоваться имеющимися в статическом классе данными
                if (OnNullOrEmptySensorsValueVerification())
                    GetSensorsData();
                else
                    SetSensorsValue(StaticBox.Sensors["Вес груза"], StaticBox.Sensors["Температура"],
                        StaticBox.Sensors["Влажность"], StaticBox.Sensors["Освещенность"], StaticBox.Sensors["Уровень заряда аккумулятора"]);

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

        private bool OnNullOrEmptySensorsValueVerification()
        {
            string.IsNullOrEmpty(StaticBox.Sensors["Вес груза"]);
            if (string.IsNullOrEmpty(StaticBox.Sensors["Температура"]) 
                || string.IsNullOrEmpty(StaticBox.Sensors["Влажность"]) 
                || string.IsNullOrEmpty(StaticBox.Sensors["Освещенность"])
                || string.IsNullOrEmpty(StaticBox.Sensors["Уровень заряда аккумулятора"])
                || string.IsNullOrEmpty(StaticBox.Sensors["Уровень сигнала"])
                || string.IsNullOrEmpty(StaticBox.Sensors["Состояние дверей"])
                || string.IsNullOrEmpty(StaticBox.Sensors["Состояние контейнера"])
                || string.IsNullOrEmpty(StaticBox.Sensors["Местоположение контейнера"]))
            {
                return true;
            }
            return false;
        }

        private void BtnSaveChangedSensorValues_Click(object sender, EventArgs e)
        {
            UpdataSensorsData();
        }

        private async void UpdataSensorsData()
        {
            try
            {
                preloader.Visibility = ViewStates.Visible;

                using (var client = ClientHelper.GetClient(StaticUser.Token))
                {
                    SensorsService.InitializeClient(client);

                    EditBoxViewModel ForAnotherServer = new EditBoxViewModel
                    {
                        id = StaticBox.IMEI,

                        Sensors = new Dictionary<string, string>
                        {
                            ["Вес груза"] = sensors.Find(w => w.SensorName.Text == WeightName).CurrentValue.Text.Replace(",", "."),
                            ["Температура"] = sensors.Find(w => w.SensorName.Text == TempName).CurrentValue.Text.Replace(",", "."),
                            ["Влажность"] = sensors.Find(w => w.SensorName.Text == HumidityName).CurrentValue.Text.Replace(",", "."),
                            ["Освещенность"] = sensors.Find(w => w.SensorName.Text == LightName).CurrentValue.Text.Replace(",", "."),
                            ["Уровень заряда аккумулятора"] = sensors.Find(w => w.SensorName.Text == BatteryName).CurrentValue.Text.Replace(",", "."),
                            ["Уровень сигнала"] = "-8",
                            ["Состояние дверей"] = StaticBox.Sensors["Состояние дверей"],
                            ["Состояние контейнера"] = StaticBox.Sensors["Состояние контейнера"],
                            ["Местоположение контейнера"] = StaticBox.Sensors["Местоположение контейнера"]
                        },
                    };

                    var o_data = await SensorsService.EditBox(ForAnotherServer);

                    if (o_data.Status == "0")
                    {
                        Toast.MakeText(Activity, o_data.Message, ToastLength.Long).Show();

                        StaticBox.Sensors["Вес груза"] = sensors.Find(w => w.SensorName.Text == WeightName).CurrentValue.Text;
                        StaticBox.Sensors["Температура"] = sensors.Find(w => w.SensorName.Text == TempName).CurrentValue.Text;
                        StaticBox.Sensors["Влажность"] = sensors.Find(w => w.SensorName.Text == HumidityName).CurrentValue.Text;
                        StaticBox.Sensors["Освещенность"] = sensors.Find(w => w.SensorName.Text == LightName).CurrentValue.Text;
                        StaticBox.Sensors["Уровень заряда аккумулятора"] = sensors.Find(w => w.SensorName.Text == BatteryName).CurrentValue.Text;
                        StaticBox.Sensors["Уровень сигнала"] = "-8";
                    }
                    else
                        Toast.MakeText(Activity, "Не получилось изменить значения датчиков. " +
                            "Ошибка: " + o_data.Message, ToastLength.Long).Show();

                    preloader.Visibility = ViewStates.Invisible;
                }    
            }
            catch (System.Exception ex)
            {
                Toast.MakeText(Activity, ex.Message, ToastLength.Long).Show();
                preloader.Visibility = ViewStates.Invisible;
            }           
        }

        private async void GetSensorsData()
        {        
            try
            {
                using (var client = ClientHelper.GetClient(StaticUser.Token))
                {
                    SensorsService.InitializeClient(client);
                    var o_data = await SensorsService.GetInfoBox(StaticBox.IMEI);

                    if (o_data.Result.ToString() == "OK")
                    {
                        if (StaticBox.Sensors["Состояние контейнера"] == "0")
                        {
                            Color WeightSensorTextColor = new Color(ContextCompat.GetColor(Activity, Resource.Color.NotActivPageColor));
                            sensors.Find(w => w.SensorName.Text == WeightName).SensorName.SetTextColor(WeightSensorTextColor);
                            sensors.Find(w => w.SensorName.Text == WeightName).CurrentValue.SetTextColor(WeightSensorTextColor);

                            AlertDialog.Builder alert = new AlertDialog.Builder(Activity);
                            alert.SetTitle("Оповещение");
                            alert.SetMessage("Состояние контейнера:  сложен. Невозможно изменить значение датчика веса.");
                            alert.SetPositiveButton("Ок", (senderAlert, args) => { });
                            Dialog dialog = alert.Create();
                            dialog.Show();
                        }

                        SetSensorsValue(StaticBox.Sensors["Вес груза"], StaticBox.Sensors["Температура"],
                            StaticBox.Sensors["Влажность"], StaticBox.Sensors["Освещенность"], StaticBox.Sensors["Уровень заряда аккумулятора"]);
                        Toast.MakeText(Activity, "Данные обновлены.", ToastLength.Long).Show();
                    }
                    else
                    {
                        SetSensorsValue("???", "???", "???", "???", "???");
                        Toast.MakeText(Activity, "Не удалось получить значения датчиков. " +
                            "Ошибка: " + o_data.ErrorInfo, ToastLength.Long).Show();
                    }
                }
                
            }
            catch (System.Exception ex)
            {
                Toast.MakeText(Activity, ex.Message, ToastLength.Long).Show();
            }
        }

        private void SetSensorsValue(string weight, string temperatur, string humidity, string illumination, string battery)
        {
            sensors.Find(w => w.SensorName.Text == WeightName).CurrentValue.Text = weight + sensors.Find(w => w.SensorName.Text == WeightName).Unit;
            sensors.Find(w => w.SensorName.Text == TempName).CurrentValue.Text = temperatur + sensors.Find(w => w.SensorName.Text == TempName).Unit;
            sensors.Find(w => w.SensorName.Text == HumidityName).CurrentValue.Text = humidity + sensors.Find(w => w.SensorName.Text == HumidityName).Unit;
            sensors.Find(w => w.SensorName.Text == LightName).CurrentValue.Text = illumination + sensors.Find(w => w.SensorName.Text == LightName).Unit;
            sensors.Find(w => w.SensorName.Text == BatteryName).CurrentValue.Text = battery + sensors.Find(w => w.SensorName.Text == BatteryName).Unit;

            SelectedSensorValue.Text = temperatur;
            string[] subs = SelectedSensorValue.Text.Split(' ');
            seekBar.Progress = float.Parse(subs[0]);
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
                var item = sensors.Find(x => x.SensorName.Text == SelectedSensorName.Text);
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
                var item = sensors.Find(x => x.SensorName.Text == SelectedSensorName.Text);
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
            if (text == WeightName && StaticBox.Sensors["Состояние контейнера"] == "0")
            {
                Toast.MakeText(Activity, "Состояние контейнера:  сложен. Невозможно изменить значение датчика веса.", ToastLength.Long).Show();
            }
            else
            {
                var item = sensors.Find(x => x.SensorName.Text == text);
                mViews[0] = mViews[1];
                if (mViews[0] != "0")
                    ChangePreviousViewBackground();
                Unit = item.ChangeCurrentViewsBackground(ref mViews, ref SelectedSensorValue, ref SelectedSensorName, ref SelectedSensorImage);
                ChangeSeekBarCharacteristic();
                AccessToSeekBar = true;
            }            
        }

        private void ChangeSeekBarCharacteristic()
        {
            var item = sensors.Find(x => x.SensorName.Text == mViews[1]);
            var isChangeSeccessful = item.SetSensorCharacteristicsForSeekBar(ref seekBar, SelectedSensorValue.Text);
            if (!(isChangeSeccessful))
            {
                Toast.MakeText(Activity, "Что-то пошло не так...", ToastLength.Long).Show();
            }
        }
        private void ChangePreviousViewBackground()
        {
            AccessToSeekBar = false;
            var item = sensors.Find(x => x.SensorName.Text == mViews[0]);
            item.linearLayout.SetBackgroundResource(Resource.Color.TransparentColor);
            //Так как в переменной SelectedSensorValue.Text значению датчика указывается вместе с 
            // мерой измерение, её необходимо убрать, чтобы на сервер отправлялись корректные значения датчиков
            item.CurrentValue.Text = SelectedSensorValue.Text;
        }

        public bool OnTouch(View v, MotionEvent e)
        {
            //if (e.Action == MotionEventActions.Down)
            //    scrollView.Enabled = false;
            //else if (e.Action == MotionEventActions.Up)
            //    scrollView.Enabled = true;
            return false;
        }

    }
    
    public class Sensor
    {
        public ImageView SelectedSensorImage { get; set; }
        public LinearLayout linearLayout { get; set; }
        public TextView CurrentValue { get; set; }
        public TextView SensorName { get; set; }

        public int SelectedImageCode { get; set; }
        public int MaxValue { get; }
        public int MinValue { get; }
        public string Unit { get; }

        public Sensor(int MinValue, int MaxValue, string Unit, int SelectedImageCode)
        {
            this.MinValue = MinValue;
            this.MaxValue = MaxValue;
            this.SelectedImageCode = SelectedImageCode;
            this.Unit = Unit;
        }

        public string ChangeCurrentViewsBackground(ref string[] mViews, ref TextView selectedSensorValue,
            ref TextView selectedSensorName, ref ImageView SelectedSensorImage)
        {
            SelectedSensorImage.SetImageResource(SelectedImageCode);
            selectedSensorName.Text = SensorName.Text;
            selectedSensorValue.Text = CurrentValue.Text;
            linearLayout.SetBackgroundResource(Resource.Drawable.EditTextStyle);
            mViews[1] = SensorName.Text;
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