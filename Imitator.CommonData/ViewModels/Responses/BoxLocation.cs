using System;
using System.Collections.Generic;
using System.Text;

namespace Imitator.CommonData.ViewModels.Responses
{
    public class BoxLocation
    {
        /// <summary>
        /// ID клиента.
        /// </summary>
        public string id { get; set; }

        /// <summary>
        /// Долгота
        /// </summary>
        public string lon1 { get; set; }

        /// <summary>
        /// Широта
        /// </summary>
        public string lat1 { get; set; }

        public DateTime date { get; set; }
    }
}
