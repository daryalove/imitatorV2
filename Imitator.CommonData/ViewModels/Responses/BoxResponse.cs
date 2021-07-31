using System;
using System.Collections.Generic;
using System.Text;

namespace Imitator.CommonData.ViewModels.Responses
{
    public class BoxResponse
    {
        public string id { get; set; }
        public Dictionary<string, string> Sensors { get; set; }
        public BoxResponse()
        {
            this.Sensors = new Dictionary<string, string>()
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
        }
    }
}
