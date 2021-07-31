using System;
using System.Collections.Generic;
using System.Text;

namespace Imitator.CommonData.ViewModels
{
    public class DeviceShortModel: BaseModel
    {
        public string IMEI { get; set; }
        public bool HasPhotoRequest { get; set; }
    }
}
