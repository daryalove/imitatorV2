using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Views;
using Android.Widget;
using Com.Akaita.Android.Circularseekbar;
using static Com.Akaita.Android.Circularseekbar.CircularSeekBar;

namespace Imitator.Android.Activity.MainFunctionality
{
    [Obsolete]
    public class ActivitySensorsData : Fragment
    {

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.PageSensorsData, container, false);

            var seekBar = view.FindViewById<CircularSeekBar>(Resource.Id.SeekBarSelectedCharacteristic);

            //seekBar.SetOnCenterClickedListener(this);
            //seekBar.SetOnCircularSeekBarChangeListener(this);

            seekBar.CenterClicked += (s, e) =>
            {
                CircularSeekBar view = (CircularSeekBar)e.P0;
                float progress = e.P1;
                Snackbar.Make(view, "Rest", Snackbar.LengthShort).Show();
                view.Progress = 0;
            };

            seekBar.ProgressChanged += (s, e) =>
            {
                //CircularSeekBar view = e.P0;
                //float progress = e.P1;
                //bool fromUser = e.P2;
                //if (progress < 33)
                //    view.RingColor = Color.Green;
                //else if (progress < 66)
                //    view.RingColor = Color.Blue;
                //else
                //    view.RingColor = Color.Red;
            };

            return view;
        }

    }
}