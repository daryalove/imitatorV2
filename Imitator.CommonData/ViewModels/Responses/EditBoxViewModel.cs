using System;
using System.Collections.Generic;
using System.Text;

namespace Imitator.CommonData.ViewModels.Responses
{
    public class EditBoxViewModel : BaseResponseObject
    {
        public string id { get; set; }


        //public string Weight { get; set; }


        //public string Light { get; set; }


        //public string Temperature { get; set; }


        //public string Wetness { get; set; }
        //public string BatteryPower { get; set; }
        //public string SignalLevel { get; set; }
        //public DateTime date { get; set; }

        public Dictionary<string, string> Sensors { get; set; }
    }
}
