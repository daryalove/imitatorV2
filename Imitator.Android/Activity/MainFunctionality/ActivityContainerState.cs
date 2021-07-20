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

namespace Imitator.Android.Activity.MainFunctionality
{
    [Obsolete]
    public class ActivityContainerState : Fragment
    {
        private Spinner ContainerPosition;
        private TextView TextDoorState;
        private TextView TextContainerState;
        private ImageButton BtnChangeContainerState;
        private ImageButton BtnChangeDoorState;
        private string PositionName;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {

            var view = inflater.Inflate(Resource.Layout.PageContainerState, container, false);

            ContainerPosition = view.FindViewById<Spinner>(Resource.Id.SpinnerContainerPosition);
            TextContainerState = view.FindViewById<TextView>(Resource.Id.TextContainerState);
            TextDoorState = view.FindViewById<TextView>(Resource.Id.TextDoorState);
            BtnChangeContainerState = view.FindViewById<ImageButton>(Resource.Id.BtnChangeContainerState);
            BtnChangeDoorState = view.FindViewById<ImageButton>(Resource.Id.BtnChangeDoorState);

            BtnChangeContainerState.Click += BtnChangeContainerState_Click;
            BtnChangeDoorState.Click += BtnChangeDoorState_Click;

            ContainerPosition.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(Spinner_ItemSelected);
            var adapter = ArrayAdapter.CreateFromResource( Activity, Resource.Array.ArrayContainerPosition, Resource.Layout.SpinnerItem);
            ContainerPosition.Adapter = adapter;

            return view;
        }

        private void BtnChangeDoorState_Click(object sender, EventArgs e)
        {
            switch (TextDoorState.Text)
            {
                case "Открыта":
                    TextDoorState.Text = "Закрыта";
                    return;
                case "Закрыта":
                    TextDoorState.Text = "Открыта";
                    return;
            }
        }

        private void BtnChangeContainerState_Click(object sender, EventArgs e)
        {
            switch (TextContainerState.Text)
            {
                case "Разложен":
                    TextContainerState.Text = "Сложен";
                    return;
                case "Сложен":
                    TextContainerState.Text = "Разложен";
                    return;
            }
        }

        private void Spinner_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            var spinner = sender as Spinner;
            PositionName = spinner.GetItemAtPosition(e.Position).ToString();
        }
    }
}