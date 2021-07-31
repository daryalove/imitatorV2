using System;
using System.Collections.Generic;
using System.Text;

namespace Imitator.CommonData.ViewModels.Responses
{
    public class ErrorResponseObject : BaseResponseObject
    {
        public List<string> Errors { get; set; }
    }
}
