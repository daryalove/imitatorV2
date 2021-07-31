using Imitator.CommonData;
using Imitator.CommonData.ViewModels;
using Imitator.CommonData.ViewModels.Responses;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Imitator.WebServices.Device
{
    public class LocationService
    {
        private static HttpClient _httpClient;

        /// <summary>
        /// Инициализация экземпляра клиента
        /// </summary>
        /// <param name="client"></param>
        public static void InitializeClient(HttpClient client)
        {
            _httpClient = client;
        }

        /// <summary>
        /// Обновление GPS координат на стороне промежуточного сервера.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static async Task<BaseModel> SetGPS(PositionModel model)
        {
            try
            {
                var formContent = new FormUrlEncodedContent(new Dictionary<string, string>
                    {
                        { "DeviceId", model.DeviceId.ToString() },
                        { "Longitude", model.Longitude.ToString()},
                        { "Latitude", model.Latitude.ToString() },
                        { "CurrentDate", model.CurrentDate.ToString()}
                    });

                HttpResponseMessage response = await _httpClient.PostAsync($"SetGPS?DeviceId={model.DeviceId}&Longitude={model.Longitude}&" +
                    $"Latitude={model.Latitude}&CurrentDate={model.CurrentDate}", formContent);
                
                string s_result;
                using (HttpContent responseContent = response.Content)
                {
                    s_result = await responseContent.ReadAsStringAsync();
                }

                BaseModel o_data = new BaseModel();

                switch (response.StatusCode)
                {
                    case HttpStatusCode.OK:
                        {
                            var response1 = JsonConvert.DeserializeObject<AuthApiData<BaseResponseObject>>(s_result);
                            
                            o_data.SuccessInfo = response1.Message;
                            o_data.Result = DefaultEnums.Result.OK;
                            return o_data;
                        }
                    case HttpStatusCode.BadRequest:
                        {
                            o_data.Error = new Exception(s_result);
                            o_data.ErrorInfo = s_result;
                            o_data.Result = DefaultEnums.Result.Error;
                            return o_data;
                        }
                    default:
                        {
                            throw new Exception("Ошибка сервера: " + response.StatusCode.ToString() + "\r\n" + s_result);
                        }
                }

            }
            catch (Exception ex)
            {
                BaseModel o_data = new BaseModel()
                {
                    Error = ex,
                    ErrorInfo = ex.Message,
                    Result = DefaultEnums.Result.Warning
                };
                return o_data;
            }
        }

        /// <summary>
        /// Обновление координат на стороне сервера Шахт.
        /// </summary>
        /// <param name="gpsLocation"></param>
        /// <returns></returns>
        public static async Task<AuthApiData<BaseResponseObject>> SetLocation(BoxLocation gpsLocation)
        {
            try
            {
                int signal = 0;

                var myHttpClient = new HttpClient();
                // var uri = new Uri("http://iot-tmc-cen.1gb.ru/api/container/setcontainerlocation?id=" + gpsLocation.id + "&lat1=" + gpsLocation.lat1 + "&lon1=" + gpsLocation.lon1 + "&date=" + gpsLocation.date);
                var uri2 = new Uri("http://smartboxcity.ru:8003/imitator/geo");


                //json структура.
                FormUrlEncodedContent formUrlEncodedContent = new FormUrlEncodedContent(new Dictionary<string, string>
                    {
                        { "Id", gpsLocation.id },
                        { "Lon1", gpsLocation.lon1.ToString().Replace(",",".")},
                        { "Lat1", gpsLocation.lat1.ToString().Replace(",",".")},
                        { "Date", DateTime.Now.ToString()}
                    });
                var formContent = formUrlEncodedContent;

                // HttpResponseMessage response = await myHttpClient.PostAsync(uri.ToString(), formContent);// !!!!
                HttpResponseMessage responseFromAnotherServer = await myHttpClient.PostAsync(uri2.ToString(), formContent);
                AuthApiData<BaseResponseObject> o_data = new AuthApiData<BaseResponseObject>();

                //string s_result;
                //using (HttpContent responseContent = response.Content)
                //{
                //    s_result = await responseContent.ReadAsStringAsync();
                //}

                string s_result_from_another_server;
                using (HttpContent responseContent = responseFromAnotherServer.Content)
                {
                    s_result_from_another_server = await responseContent.ReadAsStringAsync();
                }

                if (responseFromAnotherServer.IsSuccessStatusCode)
                {
                    o_data = JsonConvert.DeserializeObject<AuthApiData<BaseResponseObject>>(s_result_from_another_server);
                    o_data.Status = "0";
                    return o_data;
                }
                else
                {
                    ErrorResponseObject error = new ErrorResponseObject();
                    error = JsonConvert.DeserializeObject<ErrorResponseObject>(s_result_from_another_server);
                    string message = "";

                    foreach (var mes in error.Errors)
                    {
                        message += mes + "\r\n";
                    }

                    throw new Exception(message);
                }

            }
            catch (Exception ex)
            {
                AuthApiData<BaseResponseObject> o_d = new AuthApiData<BaseResponseObject>
                {
                    Message = ex.Message,
                    Status = "1"
                };
                return o_d;
            }
        }
    }
}
