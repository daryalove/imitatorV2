using System;
using System.Collections.Generic;
using System.Text;

namespace Imitator.CommonData.ViewModels.Responses
{
    public class AuthApiData<T>
       where T : BaseResponseObject, new()
    {
        /// <summary>
        /// Статус ответа.
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Сообщение ответа.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Информация о клиенте.
        /// </summary>
        public T ResponseData { get; set; }
    }
}
