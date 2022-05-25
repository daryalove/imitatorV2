using Imitator.CommonData.ViewModels.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace Imitator.CommonData.DataModels
{
    public class StaticBox
    {
        public static Guid DeviceId { get; set; }
        public static string IMEI { get; set; }
        public static Dictionary<string, string> Imitators { get; set; }
        public static Dictionary<string, string> Sensors { get; set; }
        public static string IsAvalableRequest { get; set; }
        public static double Longitude { get; set; }
        public static double Latitude { get; set; }
        public static DateTime CreatedAtSensors { get; set; }
        public static Dictionary<string, string> IMEIs { get; set; }

        static StaticBox()
        {
            Imitators = new Dictionary<string, string>()
            {
                {"359783086364286","mashtgbr/esp/led2" },
                {"355973100307031","mashtgbr/esp/led" }
            };

            Sensors = new Dictionary<string, string>()
            {

                {"Температура","" },
                {"Влажность","" },
                {"Освещенность","" },
                {"Вес груза","" },
                {"Уровень заряда аккумулятора","" },
                {"Уровень сигнала","" },
                {"Состояние дверей","" },
                {"Состояние контейнера","" },
                {"Местоположение контейнера","" }
            };

            IMEIs = new Dictionary<string, string>()
            {
                {"1","359783086364286" },
                {"2","355973100307031" },
                {"3","866588031322024" },
                {"4","866588031322032" },
                {"5","866588031322022" },
                {"6","866234568843453" },
                {"7","366528803143432" },
                {"8","166588033434132" }
            };
        }

        static public List<ContainerResponse> Objects { get; set; }
        public static int CameraOpenOrNo { get; set; }
        public static bool IsStoppedGeo { get; set; }
        public static DateTime CurrentDate { get; set; }
    }
}
