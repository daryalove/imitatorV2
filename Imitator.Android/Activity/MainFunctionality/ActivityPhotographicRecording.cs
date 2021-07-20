using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using static Android.App.ActionBar;

namespace Imitator.Android.Activity.MainFunctionality
{
    [Obsolete]
    public class ActivityPhotographicRecording : Fragment
    {
        private int EvenRowStyle = Resource.Style.EvenRow;
        private int OddRowStyle = Resource.Style.OddRow;
        private int RowsCount;
        private int ColumnCount;

        private string[,] RowName = {
            { "№ п/п","Значение датчика", "Датчик"},
            { "1", "20", "Вес груза"},
            { "2","20", "Температура"},
            { "3", "20","Влажность"},
            { "4","20", "Свет"},
            { "5","20", "Уровень батареи"},
            { "6","20", "Сигнал сети"},
            { "7","20", "Положение"},
            { "8","20", "Контейнер"},
            { "9","20", "Дверь"},
            { "10","20", "Долгота"},
            { "11","20", "Широта"},
            { "12","20", "Дата/Время"}
        };
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            //RowName = new Dictionary<string, string>(12);
            //RowName.Add("1", "Вес груза");
            //RowName.Add("2", "Температура");
            //RowName.Add("3", "Влажность");
            //RowName.Add("4", "Свет");
            //RowName.Add("5", "Уровень батареи");
            //RowName.Add("6", "Сигнал сети");
            //RowName.Add("7", "Положение");
            //RowName.Add("8", "Контейнер");
            //RowName.Add("9", "Дверь");
            //RowName.Add("10", "Долгота");
            //RowName.Add("11", "Широта");
            //RowName.Add("12", "Дата/Время");

            RowsCount = 13;
            ColumnCount = 3;
        }

    
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.PagePhotographicRecording, container, false);

            var tableLayout = view.FindViewById<TableLayout>(Resource.Id.SensorsValueTable);

            

            for (int i = 0; i < RowsCount; i++)
            {
                TableRow tableRow = new TableRow(new ContextThemeWrapper(Activity, EvenRowStyle));

                //if (i % 2 == 0)
                //{
                //    tableRow = new TableRow(new ContextThemeWrapper(Activity, EvenRowStyle));
                //}
                //else
                //{
                //    tableRow = new TableRow(new ContextThemeWrapper(Activity, OddRowStyle));
                //}


                //TableRow tableRow = new TableRow(Activity);

                //tableRow.LayoutParameters = new TableRow.LayoutParams( ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent);
                //tableRow.SetBackgroundColor(Color.Rgb(25, 191, 126));

                //tableRow.setLayoutParams(new LayoutParams(LayoutParams.MATCH_PARENT,
                //        LayoutParams.WRAP_CONTENT));
                //tableRow.setBackgroundResource(R.drawable.shelf);

                for (int j = 0; j < ColumnCount; j++)
                {
                    TextView text = new TextView(new ContextThemeWrapper(Activity, Resource.Style.InTableTextStyle));
                    //if (j == 0)
                    //{
                    //    text.SetBackgroundResource(Resource.Drawable.EditTextStyle);
                    //}
                    text.Text = RowName[i, j];
                    tableRow.AddView(text, j);
                }

                tableLayout.AddView(tableRow, i);
            }

            return view;
        }
    }
}