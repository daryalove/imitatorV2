using System;
using System.Collections.Generic;
using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using Imitator.CommonData.DataModels;
using Imitator.CommonData.ViewModels.Responses;
using Imitator.WebServices;
using Imitator.WebServices.Device;

namespace Imitator.Android.Activity.MainFunctionality
{
    [Obsolete]
    public class ActivityContainerState : Fragment
    {
        private Spinner ContainerPosition;
        private TextView DoorState;
        private TextView ContainerState;
        private ImageButton BtnChangeContainerState;
        private ImageButton BtnChangeDoorState;
        private Button BtnSaveChangedContainerState;
        private ImageView Image;

        public string PositionName { get; private set; }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {

            var view = inflater.Inflate(Resource.Layout.PageContainerState, container, false);

            ContainerPosition = view.FindViewById<Spinner>(Resource.Id.SpinnerContainerPosition);
            ContainerState = view.FindViewById<TextView>(Resource.Id.TextContainerState);
            DoorState = view.FindViewById<TextView>(Resource.Id.TextDoorState);
            BtnChangeContainerState = view.FindViewById<ImageButton>(Resource.Id.BtnChangeContainerState);
            BtnChangeDoorState = view.FindViewById<ImageButton>(Resource.Id.BtnChangeDoorState);
            BtnSaveChangedContainerState = view.FindViewById<Button>(Resource.Id.BtnSaveChangedContainerState);
            Image = view.FindViewById<ImageView>(Resource.Id.ImageContainerDoorState);

            BtnChangeContainerState.Click += BtnChangeContainerState_Click;
            BtnChangeDoorState.Click += BtnChangeDoorState_Click;
            BtnSaveChangedContainerState.Click += BtnSaveChangedContainerState_Click;

            ContainerPosition.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(Spinner_ItemSelected);
            var adapter = ArrayAdapter.CreateFromResource( Activity, Resource.Array.ArrayContainerPosition, Resource.Layout.SpinnerItem);
            ContainerPosition.Adapter = adapter;


            if (OnNullOrEmptySensorsValueVerification())
                GetSensorsData();
            else
                SetSensorsValue();

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

        private void BtnSaveChangedContainerState_Click(object sender, EventArgs e)
        {
            UpdataSensorsData();
        }

        private void BtnChangeDoorState_Click(object sender, EventArgs e)
        {
            if(ContainerState.Text == "Сложен")
            {
                Image.SetImageResource(Resource.Drawable.closed_box);
                Toast.MakeText(Activity, "Состояние контейнера:  Сложен. Невозможно открыть либо закрыть дверь.", ToastLength.Long).Show();
            }
            else
            {
                switch (DoorState.Text)
                {
                    case "Открыта":
                        {
                            DoorState.Text = "Закрыта";
                            Image.SetImageResource(Resource.Drawable.close_door);
                            return;
                        }
                    case "Закрыта":
                        {
                            DoorState.Text = "Открыта";
                            Image.SetImageResource(Resource.Drawable.OpenDoor);
                            return;
                        }
                }
            }          
        }

        private void BtnChangeContainerState_Click(object sender, EventArgs e)
        {
            switch (ContainerState.Text)
            {
                case "Разложен":
                    ContainerState.Text = "Сложен";
                    DoorState.Text = "Закрыта";
                    BtnChangeDoorState.SetBackgroundResource(Resource.Drawable.NotAllowChangingSensorValuesButtonStyle);
                    Image.SetImageResource(Resource.Drawable.closed_box);
                    Toast.MakeText(Activity, "Состояние контейнера:  Сложен. Невозможно открыть либо закрыть дверь.", ToastLength.Long).Show();
                    return;
                case "Сложен":
                    ContainerState.Text = "Разложен";
                    BtnChangeDoorState.SetBackgroundResource(Resource.Drawable.ChangingSensorValuesButtonStyle);
                    Image.SetImageResource(Resource.Drawable.close_door);
                    return;
            }
        }

        private void Spinner_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            var spinner = sender as Spinner;
            PositionName = spinner.GetItemAtPosition(e.Position).ToString();
        }

        private async void UpdataSensorsData()
        {
            try
            {
                StaticBox.Sensors["Состояние дверей"] = (DoorState.Text == "Закрыта") ? "0" : "1";
                StaticBox.Sensors["Состояние контейнера"] = (ContainerState.Text == "Сложен") ? "0" : "1";
                StaticBox.Sensors["Местоположение контейнера"] = PositionName;

                using (var client = ClientHelper.GetClient(StaticUser.Token))
                {
                    SensorsService.InitializeClient(client);

                    EditBoxViewModel ForAnotherServer = new EditBoxViewModel
                    {
                        //id = "866588031322022",
                        id = StaticBox.IMEI,
                        Sensors = new Dictionary<string, string>
                        {
                            ["Вес груза"] = StaticBox.Sensors["Вес груза"],
                            ["Температура"] = StaticBox.Sensors["Температура"],
                            ["Влажность"] = StaticBox.Sensors["Влажность"],
                            ["Освещенность"] = StaticBox.Sensors["Освещенность"],
                            ["Уровень заряда аккумулятора"] = StaticBox.Sensors["Уровень заряда аккумулятора"],
                            ["Уровень сигнала"] = StaticBox.Sensors["Уровень сигнала"],
                            ["Состояние дверей"] = StaticBox.Sensors["Состояние дверей"],
                            ["Состояние контейнера"] = StaticBox.Sensors["Состояние контейнера"],
                            ["Местоположение контейнера"] = StaticBox.Sensors["Местоположение контейнера"]
                        },
                    };

                    var o_data = await SensorsService.EditBox(ForAnotherServer);

                    if (o_data.Status == "0")
                    {
                        Toast.MakeText(Activity, o_data.Message, ToastLength.Long).Show();

                        StaticBox.CurrentDate = DateTime.Now;
                        StaticBox.CameraOpenOrNo = 1;

                        ActivityPhotographicRecording ContentPhotographicRecording = new ActivityPhotographicRecording();
                        FragmentTransaction transaction = this.FragmentManager.BeginTransaction();
                        transaction.Replace(Resource.Id.framelayoutFormMainFunctionality, ContentPhotographicRecording).AddToBackStack(null).Commit();
                    }
                    else
                        Toast.MakeText(Activity, "Не получилось изменить значения датчиков. " +
                            "Ошибка: " + o_data.Message, ToastLength.Long).Show();
                }
            }
            catch (System.Exception ex)
            {
                Toast.MakeText(Activity, ex.Message, ToastLength.Long).Show();
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
                            AlertDialog.Builder alert = new AlertDialog.Builder(Activity);
                            alert.SetTitle("Оповещение");
                            alert.SetMessage("Состояние контейнера:  сложен. Невозможно открыть либо закрыть дверь.");
                            alert.SetPositiveButton("Ок", (senderAlert, args) => { });
                            Dialog dialog = alert.Create();
                            dialog.Show();
                        }

                        SetSensorsValue();
                    }
                    else
                    {
                        ContainerState.Text = "Не известно";
                        DoorState.Text = "Не известно";
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

        private void SetSensorsValue()
        {
            if (StaticBox.Sensors["Состояние контейнера"] == "0")
            {
                ContainerState.Text = "Сложен";
                DoorState.Text = "Закрыта";
                BtnChangeDoorState.SetBackgroundResource(Resource.Drawable.NotAllowChangingSensorValuesButtonStyle);
                Image.SetImageResource(Resource.Drawable.closed_box);
            }
            else if (StaticBox.Sensors["Состояние контейнера"] == "1")
            {
                ContainerState.Text = "Разложен";

                if (StaticBox.Sensors["Состояние дверей"] == "0")
                {
                    DoorState.Text = "Закрыта";
                    Image.SetBackgroundResource(Resource.Drawable.close_door);
                }
                else
                {
                    DoorState.Text = "Открыта";
                    Image.SetBackgroundResource(Resource.Drawable.OpenDoor);
                }
            }

            var positions = Resources.GetStringArray(Resource.Array.ArrayContainerPosition);
            var index = Array.IndexOf(positions, StaticBox.Sensors["Местоположение контейнера"]);
            ContainerPosition.SetSelection(index);
        }
    }
}