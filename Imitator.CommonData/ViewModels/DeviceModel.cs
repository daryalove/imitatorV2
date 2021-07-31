using System;
using System.Collections.Generic;
using System.Text;

namespace Imitator.CommonData.ViewModels
{
    public class DeviceModel: BaseModel
    {
        public string ModelName { get; set; }
        public string Manufacturer { get; set; }
        public string Name { get; set; }
        public string Version { get; set; }
        public string Platform { get; set; }
        public string IMEI { get; set; }
        public string Idiom { get; set; }
    }
}
