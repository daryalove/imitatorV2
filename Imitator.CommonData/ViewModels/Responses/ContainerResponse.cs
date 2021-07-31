using System;
using System.Collections.Generic;
using System.Text;

namespace Imitator.CommonData.ViewModels.Responses
{
    public class ContainerResponse: BaseResponseObject
    {
        /// <summary>
        /// Id контейнера.
        /// </summary>
        public string SmartBoxId { get; set; }


        /// <summary>
        /// Наименование контейнера.
        /// </summary>
        public string Name { get; set; }
    }
}
