using System;
using System.Collections.Generic;
using System.Text;

namespace Imitator.CommonData.ViewModels.Responses
{
    public class Status : BaseResponseObject
    {
        public BoxResponse status { get; set; }
        public Status()
        {
            this.status = new BoxResponse();
        }
    }
}
