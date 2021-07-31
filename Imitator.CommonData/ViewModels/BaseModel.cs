using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Imitator.CommonData.ViewModels
{
    public class BaseModel
    {
        public Guid Id { get; set; }
        
        [JsonIgnore]
        public Exception Error { get; set; }

        public string ErrorInfo { get; set; }

        public string SuccessInfo { get; set; }
        public DefaultEnums.Result Result { get; set; }
        public BaseModel()
        {
            Result = DefaultEnums.Result.OK;
        }
        public BaseModel(Exception exp)
        {
            Error = exp;
            Result = DefaultEnums.Result.Error;
        }
        public static BaseModel ErrorFormat(Exception exp)
        {
            return new BaseModel(exp);
        }


    }

    public static class BaseModelUtilities<T>
       where T : BaseModel, new()
    {
        public static T DataFormat(T CurrentData)
        {
            CurrentData.Result = DefaultEnums.Result.OK;
            return CurrentData;
        }

        public static T ErrorFormat(Exception exp)
        {
            return new T() { Error = exp, Result = DefaultEnums.Result.Error };
        }
    }
}
