using Imitator.CommonData;
using Imitator.CommonData.DataModels;
using Imitator.CommonData.ViewModels;
using Imitator.CommonData.ViewModels.Responses;
using Imitator.CommonData.ViewModels.Responses.SmartBoxResponse;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using StaticBox = Imitator.CommonData.DataModels.StaticBox;

namespace Imitator.WebServices.Device
{
    public class SensorsService
    {
        #region Variables
        private static HttpClient _httpClient;

        //private static string registerBoxApi = "https://smartboxcity.ru:8003/imitator/create?id=";

        private static string registerBoxIotApi = "container/create?name=";

        //private static string editBoxApi = "http://smartboxcity.ru:8003/imitator/sensors?";

        private static string editBoxIotApi = "container/editsensors?id=";

        //private static string getInfoBoxApi = "http://smartboxcity.ru:8003/imitator/status?id=";

        private static string getInfoBoxIotApi = "container/getbox?IMEI=";

        //private static string makeAndCancelAlarmApi = "http://smartboxcity.ru:8003/imitator/";

        private static string makeAlarmIotApi = "container/raisealarm/?IMEI=";

        private static string cancelAlarmIotApi = "container/releasealarm/?IMEI=";

        #endregion

        /// <summary>
        /// Инициализация экземпляра клиента
        /// </summary>
        /// <param name="client"></param>
        public static void InitializeClient(HttpClient client)
        {
            _httpClient = client;
        }

        /// <summary>
        /// Регистрация устройства.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static async Task<DeviceModel> RegisterDevice(DeviceModel model)
        {
            try
            {
                var formContent = new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    { "Idiom", model.Idiom },
                    { "IMEI", model.IMEI},
                    { "Manufacturer", model.Manufacturer },
                    { "ModelName", model.ModelName},
                    { "Name", model.Name },
                    { "Platform", model.Platform},
                    { "Version", model.Version }
                });

                HttpResponseMessage response = await _httpClient.PostAsync($"imitator/registerdevice?Idiom={model.Idiom}&" +
                    $"IMEI={model.IMEI}&Manufacturer={model.Manufacturer}&ModelName={model.ModelName}&Name={model.Name}" +
                    $"&Platform={model.Platform}&Version={model.Version}", formContent);
               
                string s_result;
                using (HttpContent responseContent = response.Content)
                {
                    s_result = await responseContent.ReadAsStringAsync();
                }

                BaseModel o_data = new BaseModel();

                switch (response.StatusCode)
                {
                    //case HttpStatusCode.InternalServerError:
                    //    {
                    //        //ErrorResponseObject error = new ErrorResponseObject();
                    //        //o_data.Status = response.StatusCode;
                    //        //o_data.Message = "Внутренняя ошибка сервера 500";
                    //        //return o_data;
                    //    }

                    //case HttpStatusCode.NotFound:
                    //    {
                    //        //ErrorResponseObject error = new ErrorResponseObject();
                    //        //o_data.Status = response.StatusCode;
                    //        //o_data.Message = "Ресурс не найден 404";
                    //        //return o_data;
                    //    }
                    case HttpStatusCode.OK:
                        {
                            DeviceModel dv = JsonConvert.DeserializeObject<DeviceModel>(s_result);
                            return dv;
                        }
                    case HttpStatusCode.BadRequest:
                        {
                            o_data = JsonConvert.DeserializeObject<BaseModel>(s_result);
                            throw new Exception(response.StatusCode.ToString() + "" + o_data.ErrorInfo);
                        }
                    default:
                        {
                            throw new Exception("Ошибка сервера: " + response.StatusCode.ToString() + "\r\n" + s_result);
                        }
                }

            }
            catch (Exception ex)
            {
                DeviceModel o_data = new DeviceModel()
                {
                    Error = ex,
                    ErrorInfo = ex.Message,
                    Result = DefaultEnums.Result.Warning
                };
                return o_data;
            }
        }

        /// <summary>
        /// Добавление нового запроса.
        /// </summary>
        /// <returns></returns>
        public static async Task<BaseModel> UpdatePhotoRequest(string IMEI)
        {
            try
            {
                var formContent = new FormUrlEncodedContent(new Dictionary<string, string>
                    {
                        { "IMEI", IMEI },
                    });

                HttpResponseMessage response = await _httpClient.PostAsync($"imitator/UpdatePhotoRequest?IMEI={IMEI}", formContent);
                
                string s_result;
                using (HttpContent responseContent = response.Content)
                {
                    s_result = await responseContent.ReadAsStringAsync();
                }
                BaseModel model = new BaseModel();

                switch (response.StatusCode)
                {
                    case HttpStatusCode.OK:
                        {
                            var data = JsonConvert.DeserializeObject<AuthApiData<BaseResponseObject>>(s_result);
                            model.SuccessInfo = data.Message;
                            model.Result = DefaultEnums.Result.OK;
                            return model;
                        }
                    case HttpStatusCode.BadRequest:
                        {
                            model.Error = new Exception(s_result);
                            model.Result = DefaultEnums.Result.Error;
                            return model;
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
        /// Поиск запроса на получение фото от сторонних пользователей.
        /// </summary>
        /// <returns></returns>
        public static async Task<DeviceShortModel> SearchPhotoRequest(Guid id)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync($"imitator/SearchPhotoRequest?id={id}");
                string s_result;
                using (HttpContent responseContent = response.Content)
                {
                    s_result = await responseContent.ReadAsStringAsync();
                }
                DeviceShortModel model = new DeviceShortModel();

                switch (response.StatusCode)
                {
                    case HttpStatusCode.OK:
                        {
                            model = JsonConvert.DeserializeObject<DeviceShortModel>(s_result);
                            return model;
                        }
                    case HttpStatusCode.BadRequest:
                        {
                            model = JsonConvert.DeserializeObject<DeviceShortModel>(s_result);
                            return model;
                        }
                    default:
                        {
                            throw new Exception("Ошибка сервера: " + response.StatusCode.ToString() + "\r\n" + s_result);
                        }
                }
            }
            catch (Exception ex)
            {
                DeviceShortModel o_data = new DeviceShortModel()
                {
                    Error = ex,
                    ErrorInfo = ex.Message,
                    Result = DefaultEnums.Result.Warning
                };
                return o_data;
            }
        }

        /// <summary>
        /// Создание виртуального контейнера на сервере Шахт.
        /// </summary>
        /// <returns></returns>
        public static async Task<BaseModel> RegisterBox(string id)
        {
            try
            {
                var formContent = new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    { "name", id }
                });

                HttpResponseMessage response = await _httpClient.PostAsync(registerBoxIotApi + id, formContent);

                string s_result;
                using (HttpContent responseContent = response.Content)
                {
                    s_result = await responseContent.ReadAsStringAsync();
                }

                BaseModel md = new BaseModel();
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    md.SuccessInfo = "Создание виртуального контейнера завершено успешно.";
                    md.Result = DefaultEnums.Result.OK;
                    return md;
                }
                else
                {
                    md.Error = new Exception(s_result);
                    md.Result = DefaultEnums.Result.Error;
                    return md;
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

            #region Obsolete
            /*http://smartboxcity.ru:8003/imitator/create POST создает контейнер
             * http://iot.tmc-centert.ru/api/container/SearchCommandPhoto?name=123
http://smartboxcity.ru:8003/imitator/delete GET удаляет контейнер*/


            //try
            //{
            //    #region WebRequest Example
            //    //var formContent = new Dictionary<string, string>
            //    //    {
            //    //        { "Id", StaticBox.DeviceId }
            //    //    };

            //    //string newData = "";

            //    //foreach (string key in formContent.Keys)
            //    //{
            //    //    newData += key + "="
            //    //          + formContent[key] + "&";
            //    //}

            //    //var postData = newData.Remove(newData.Length - 1, 1);

            //    //HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://smartboxcity.ru:8003/imitator/create");
            //    //request.Method = "POST";


            //    //byte[] data = Encoding.ASCII.GetBytes(postData);

            //    //request.ContentType = "multipart/form-data";
            //    //request.ContentLength = data.Length;

            //    //Stream requestStream = request.GetRequestStream();
            //    //requestStream.Write(data, 0, data.Length);
            //    //requestStream.Close();
            //    //HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            //    //Stream responseStream = response.GetResponseStream();

            //    //StreamReader myStreamReader = new StreamReader(responseStream, Encoding.Default);

            //    //string s_result = myStreamReader.ReadToEnd();

            //    //myStreamReader.Close();
            //    //responseStream.Close();

            //    //response.Close();
            //    #endregion

            //    CreateBoxModel model = new CreateBoxModel
            //    {
            //        id = id
            //    };

            //    var myHttpClient = new HttpClient();
            //    var uri = new Uri(registerBoxApi + model.id);

            //    // Поучаю ответ об авторизации [успех или нет]
            //    HttpResponseMessage response = await myHttpClient.GetAsync(uri.ToString() /*new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json")*/);

            //    string s_result;
            //    using (HttpContent responseContent = response.Content)
            //    {
            //        s_result = await responseContent.ReadAsStringAsync();
            //    }

            //    BaseModel md = new BaseModel();
            //    if (response.StatusCode == HttpStatusCode.OK)
            //    {
            //        md.SuccessInfo = "Создание виртуального контейнера завершено успешно.";
            //        md.Result = DefaultEnums.Result.OK;
            //        return md;
            //    }
            //    else
            //    {
            //        md.Error = new Exception(s_result);
            //        md.Result = DefaultEnums.Result.Error;
            //        return md;
            //    }
            //    // AuthApiData<AuthResponseData> o_data = JsonConvert.DeserializeObject<AuthApiData<AuthResponseData>>(s_result);
            //}
            //catch (Exception ex)
            //{
            //    BaseModel o_data = new BaseModel()
            //    {
            //        Error = ex,
            //        ErrorInfo = ex.Message,
            //        Result = DefaultEnums.Result.Warning
            //    };
            //    return o_data;
            //}
            #endregion
        }

        /// <summary>
        /// Редактирование данных контейнера.
        /// </summary>
        /// <returns></returns>
        public static async Task<AuthApiData<BaseResponseObject>> EditBox(EditBoxViewModel ForAnotherServer)
        {
            try
            {
                var date = DateTime.Now;

                ForAnotherServer.Date = date;

                #region Obsolete
                //var uri = new Uri(editBoxIotApi + date + "&id=" + CrossSettings.Current.GetValueOrDefault("id", "") + "&sensors[Вес груза]=" + StaticBox.Sensors["Вес груза"]
                //+ "&sensors[Температура]=" + StaticBox.Sensors["Температура"] + "&sensors[Влажность]=" + StaticBox.Sensors["Влажность"] + "&sensors[Освещенность]=" + StaticBox.Sensors["Освещенность"]
                //+ "&sensors[Уровень заряда аккумулятора]=" + StaticBox.Sensors["Уровень заряда аккумулятора"] + "&sensors[Уровень сигнала]=" + StaticBox.Sensors["Уровень сигнала"] + "&sensors[Состояние дверей]=" + StaticBox.Sensors["Состояние дверей"]
                //+ "&sensors[Состояние контейнера]=" + StaticBox.Sensors["Состояние контейнера"] + "&sensors[Местоположение контейнера]=" + StaticBox.Sensors["Местоположение контейнера"]);
                #endregion

                string uri2 = editBoxIotApi + ForAnotherServer.id
                + "&date=" + date
                + "&sensors[Вес груза]=" + ForAnotherServer.Sensors["Вес груза"]
                + "&sensors[Температура]=" + ForAnotherServer.Sensors["Температура"] 
                + "&sensors[Влажность]=" + ForAnotherServer.Sensors["Влажность"] 
                + "&sensors[Освещенность]=" + ForAnotherServer.Sensors["Освещенность"]
                + "&sensors[Уровень заряда аккумулятора]=" + ForAnotherServer.Sensors["Уровень заряда аккумулятора"] 
                + "&sensors[Уровень сигнала]=" + ForAnotherServer.Sensors["Уровень сигнала"] 
                + "&sensors[Состояние дверей]=" + ForAnotherServer.Sensors["Состояние дверей"]
                + "&sensors[Состояние контейнера]=" + ForAnotherServer.Sensors["Состояние контейнера"] 
                + "&sensors[Местоположение контейнера]=" + ForAnotherServer.Sensors["Местоположение контейнера"];


                #region Obsolete
                //    var formContent = new FormUrlEncodedContent(new Dictionary<string, string>
                //{
                //    { "id", StaticBox.IMEI },
                //    { "date", date.ToString()},
                //    { "sensors[Вес груза]", StaticBox.Sensors["Вес груза"]},
                //    { "sensors[Температура]", StaticBox.Sensors["Температура"]},
                //    { "sensors[Влажность]", StaticBox.Sensors["Влажность"]},
                //    { "sensors[Освещенность]", StaticBox.Sensors["Освещенность"]},
                //    { "sensors[Уровень заряда аккумулятора]", StaticBox.Sensors["Уровень заряда аккумулятора"]},
                //    { "sensors[Уровень сигнала]", StaticBox.Sensors["Уровень сигнала"]},
                //    { "sensors[Состояние дверей]", StaticBox.Sensors["Состояние дверей"]},
                //    { "sensors[Состояние контейнера]", StaticBox.Sensors["Состояние контейнера"]},
                //    { "sensors[Местоположение контейнера]", StaticBox.Sensors["Местоположение контейнера"]}
                //});

                //EditBoxViewModel box = new EditBoxViewModel
                //{
                //    id = CrossSettings.Current.GetValueOrDefault("id", ""),
                //    date = date,
                //    Sensors = new Dictionary<string, string>
                //    {
                //        {"Температура",StaticBox.Sensors["Температура"] },
                //        {"Влажность",StaticBox.Sensors["Влажность"] },
                //        {"Освещенность",StaticBox.Sensors["Освещенность"] },
                //        {"Вес груза",StaticBox.Sensors["Вес груза"] },
                //        {"Уровень заряда аккумулятора",StaticBox.Sensors["Уровень заряда аккумулятора"]},
                //        {"Уровень сигнала",StaticBox.Sensors["Уровень сигнала"]},
                //        {"Состояние дверей",StaticBox.Sensors["Состояние дверей"]},
                //        {"Состояние контейнера",StaticBox.Sensors["Состояние контейнера"]},
                //        {"Местоположение контейнера",StaticBox.Sensors["Местоположение контейнера"]}
                //    }
                //};

                //var data = new StringContent(JsonConvert.SerializeObject(box));
                //HttpResponseMessage response = await myHttpClient.PostAsync(uri.ToString(), formContent);
                #endregion Obsolete

                HttpResponseMessage responseFromAnotherServer = await _httpClient.PostAsync(uri2, new StringContent(JsonConvert.SerializeObject(ForAnotherServer), Encoding.UTF8, "application/json"));

                AuthApiData<BaseResponseObject> o_data = new AuthApiData<BaseResponseObject>();

                string s_result_from_server;
                using (HttpContent responseContent = responseFromAnotherServer.Content)
                {
                    s_result_from_server = await responseContent.ReadAsStringAsync();
                }

                if (responseFromAnotherServer.IsSuccessStatusCode)
                {
                    StaticBox.CreatedAtSensors = date;
                    o_data = JsonConvert.DeserializeObject<AuthApiData<BaseResponseObject>>(s_result_from_server);
                    o_data.Status = "0";
                    o_data.Message = "Показания датчиков имитатора изменены.";
                }
                else
                {
                    ErrorResponseObject error = new ErrorResponseObject();
                    error = JsonConvert.DeserializeObject<ErrorResponseObject>(s_result_from_server);
                    o_data.Message = error.Errors[0];
                    o_data.Status = responseFromAnotherServer.StatusCode.ToString();
                }

                return o_data;
            }
            catch (Exception ex)
            {
                AuthApiData<BaseResponseObject> result = new AuthApiData<BaseResponseObject>();
                result.Status = "1";
                result.Message = ex.Message;
                return result;
            }
        }

        /// <summary>
        /// Получение информации о контейнере.
        /// </summary>
        public static async Task<BaseModel> GetInfoBox(string IMEI)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync(getInfoBoxIotApi + IMEI);

                #region Obsolete
                //var myHttpClient = new HttpClient();
                //var id1 = CrossSettings.Current.GetValueOrDefault("id", "");
                //var uri = new Uri("http://iot.tmc-centert.ru/api/container/getbox?id=" + id1);
                // HttpResponseMessage response = await myHttpClient.GetAsync(uri.ToString());
                #endregion

                string s_result;
                using (HttpContent responseContent = response.Content)
                {
                    s_result = await responseContent.ReadAsStringAsync();
                }

                if (response.IsSuccessStatusCode)
                {
                    var o_data = new AuthApiData<ListResponse<CommonData.ViewModels.Responses.SmartBoxResponse.BoxResponse>>();
                    o_data = JsonConvert.DeserializeObject<AuthApiData<ListResponse<CommonData.ViewModels.Responses.SmartBoxResponse.BoxResponse>>>(s_result);

                    if (o_data.ResponseData == null)
                        throw new Exception("Ошибка получения данных");

                    foreach (var data in o_data.ResponseData.Objects)
                    {
                        StaticBox.Sensors[data.SensorName] = data.Value;
                    }

                    #region Initialize Static Box
                    //В статик бокс закомментируй 9 свойств
                    //StaticBox.Sensors["Температура"] = o_data.status.Sensors["Температура"].Replace(".", ",");
                    //StaticBox.Sensors["Влажность"] = o_data.status.Sensors["Влажность"].Replace(".", ",");
                    //StaticBox.Sensors["Освещенность"] = o_data.status.Sensors["Освещенность"].Replace(".", ",");
                    //StaticBox.Sensors["Уровень заряда аккумулятора"] = o_data.status.Sensors["Уровень заряда аккумулятора"].Replace(".", ",");
                    //StaticBox.Sensors["Уровень сигнала"] = o_data.status.Sensors["Уровень сигнала"].Replace(".", ",");
                    //StaticBox.Sensors["Состояние дверей"] = o_data.status.Sensors["Состояние дверей"];
                    //StaticBox.Sensors["Состояние контейнера"] = o_data.status.Sensors["Состояние контейнера"];
                    //StaticBox.Sensors["Местоположение контейнера"] = o_data.status.Sensors["Местоположение контейнера"];
                    #endregion

                    if (StaticBox.Sensors["Состояние контейнера"] == "0")
                        StaticBox.Sensors["Вес груза"] = "0";
                    //else
                    //    StaticBox.Sensors["Вес груза"] = o_data.status.Sensors["Вес груза"];

                    return new BaseModel()
                    {
                        Result = DefaultEnums.Result.OK,
                        SuccessInfo = "Успешно."
                    };

                    #region Obsolete
                    //StaticBox.CreatedAtSensors = (DateTime)o_data.ResponseData.Objects[0].CreatedAt;
                    ////Заполняй остальные параметры как в этом примере
                    //int a = 0, b = 0;

                    //decimal weight, temp, signal, light, humi, akk;

                    //weight = Convert.ToDecimal(StaticBox.Sensors["Вес груза"]);
                    //temp = Convert.ToDecimal(StaticBox.Sensors["Температура"]);
                    //signal = Convert.ToDecimal(StaticBox.Sensors["Уровень сигнала"]);
                    //light = Convert.ToDecimal(StaticBox.Sensors["Освещенность"]);
                    //humi = Convert.ToDecimal(StaticBox.Sensors["Влажность"]);
                    //akk = Convert.ToDecimal(StaticBox.Sensors["Уровень заряда аккумулятора"]);

                    //s_weight.Progress = Convert.ToInt32(weight);
                    //a = Convert.ToInt32(temp);
                    //b = Convert.ToInt32(signal);
                    //s_light.Progress = Convert.ToInt32(light);
                    //s_humidity.Progress = Convert.ToInt32(humi);
                    //s_battery.Progress = Convert.ToInt32(akk);

                    //s_temperature.Progress = a + 40;
                    //s_signal_strength_1.Progress = b + 110;



                    //SmullWeight.Text = StaticBox.Sensors["Вес груза"];
                    //SmullTemperature.Text = StaticBox.Sensors["Температура"];
                    //SmullLight.Text = StaticBox.Sensors["Освещенность"];
                    //SmullHumidity.Text = StaticBox.Sensors["Влажность"];
                    //SmullBattery.Text = StaticBox.Sensors["Уровень заряда аккумулятора"];
                    //SmullNetworkSignal.Text = StaticBox.Sensors["Уровень сигнала"];
                    //TextNameBox.Text = "(" + StaticBox.IMEI + ")";

                    //Toast.MakeText(this, "Успешно!", ToastLength.Long).Show();

                    //if (StaticBox.Sensors["Состояние контейнера"] == "0")
                    //{
                    //    StaticBox.Sensors["Вес груза"] = "0";
                    //    Android.App.AlertDialog.Builder alert = new Android.App.AlertDialog.Builder(this);
                    //    alert.SetTitle("Внимание !");
                    //    alert.SetMessage("Невозможно изменить вес контейнера.(Состояние контейнера: сложен) ");
                    //    alert.SetPositiveButton("Закрыть", (senderAlert, args) => {
                    //        Toast.MakeText(this, "Предупреждение было закрыто!", ToastLength.Short).Show();
                    //    });
                    //    Dialog dialog = alert.Create();
                    //    dialog.Show();
                    //}
                    //btn_cause_alarm.Enabled = true;

                    #endregion
                }
                else
                {
                    ErrorResponseObject error = new ErrorResponseObject();
                    error = JsonConvert.DeserializeObject<ErrorResponseObject>(s_result);
                    throw new Exception(error.Errors[0]);

                    #region Obsolete
                    //Toast.MakeText(this, error.Errors[0], ToastLength.Long).Show();
                    //btn_cause_alarm.Enabled = false;

                    //var data = await EditSensors();
                    //if (data.Status == "1")
                    //{
                    //    Toast.MakeText(this, data.Message, ToastLength.Long).Show();
                    //    GetInfoAboutBox();
                    //}
                    //else
                    //{
                    //    Toast.MakeText(this, data.Message, ToastLength.Long).Show();
                    //    btn_cause_alarm.Enabled = false;
                    //    //StaticBox.CameraOpenOrNo = 1;
                    //    //Intent authActivity = new Intent(this, typeof(Auth.SensorsDataActivity));
                    //    //StartActivity(authActivity);
                    //}
                    #endregion
                }

            }
            catch (Exception ex)
            {
                BaseModel md = new BaseModel
                {
                    Error = ex,
                    ErrorInfo = ex.Message,
                    Result = DefaultEnums.Result.Warning
                };
                return md;
            }
        }

        /// <summary>
        /// Отмена тревоги.
        /// </summary>
        public static async Task<AuthApiData<AlarmResponse>> CancelAlarm(string option)
        {
            try
            {
                string uri = cancelAlarmIotApi + StaticBox.IMEI + "&option=" + option;

                HttpResponseMessage response = await _httpClient.GetAsync(uri);
                string s_result;
                using (HttpContent responseContent = response.Content)
                {
                    s_result = await responseContent.ReadAsStringAsync();
                }

                if (response.IsSuccessStatusCode)
                {
                    AlarmResponse o_data = new AlarmResponse();
                    o_data = JsonConvert.DeserializeObject<AlarmResponse>(s_result);

                    AuthApiData<AlarmResponse> o1 = new AuthApiData<AlarmResponse>
                    {
                        Status = "0",
                        ResponseData = o_data,
                        Message = o_data.Message
                    };
                    return o1;

                    #region Dialog
                    //if (o_data.Message == "Угроза имитатора изменена")
                    //{
                    //    CrossSettings.Current.AddOrUpdateValue("AlermId", "0");
                    //    btn_cause_alarm.Text = "Вызвать тревогу";
                    //}
                    //Android.App.AlertDialog.Builder alert = new Android.App.AlertDialog.Builder(this);
                    //alert.SetTitle("Тревоги");
                    //alert.SetMessage(o_data.Message);
                    //alert.SetPositiveButton("Закрыть", (senderAlert1, args1) =>
                    //{
                    //});
                    //Dialog dialog1 = alert.Create();
                    //dialog1.Show();
                    #endregion
                }
                else
                {
                    AlarmResponse error = new AlarmResponse();
                    error = JsonConvert.DeserializeObject<AlarmResponse>(s_result);
                    throw new Exception(error.Message);
                }
            }
            catch (Exception ex)
            {
                AuthApiData<AlarmResponse> o_d = new AuthApiData<AlarmResponse>
                {
                    Message = ex.Message,
                    Status = "1"
                };
                return o_d;
            }
        }

        /// <summary>
        /// Создание запроса тревоги контейнера.
        /// </summary>
        public static async Task<AuthApiData<AlarmResponse>> MakeRequestAlarm(string option)
        {
            try
            {
                string uri = makeAlarmIotApi + StaticBox.IMEI + "&option=" + option;

                HttpResponseMessage response = await _httpClient.GetAsync(uri);
                string s_result;

                using (HttpContent responseContent = response.Content)
                {
                    s_result = await responseContent.ReadAsStringAsync();
                }

                if (response.IsSuccessStatusCode)
                {
                    AlarmResponse o_data = new AlarmResponse();
                    o_data = JsonConvert.DeserializeObject<AlarmResponse>(s_result);

                    AuthApiData<AlarmResponse> o1 = new AuthApiData<AlarmResponse>
                    {
                        Status = "0",
                        ResponseData = o_data,
                        Message = o_data.Message
                    };
                    return o1;

                    #region Dialog
                    //if (o_data.Message == "Угроза имитатора изменена")
                    //{
                    //    btn_cause_alarm.Text = "Отменить тревогу";
                    //}
                    //Android.App.AlertDialog.Builder alert = new Android.App.AlertDialog.Builder(this);
                    //alert.SetTitle("Тревоги");
                    //alert.SetMessage(o_data.Message);
                    //alert.SetPositiveButton("Закрыть", (senderAlert1, args1) =>
                    //{
                    //});
                    //Dialog dialog1 = alert.Create();
                    //dialog1.Show();
                    #endregion
                }
                else
                {
                    AlarmResponse error = new AlarmResponse();
                    error = JsonConvert.DeserializeObject<AlarmResponse>(s_result);
                    throw new Exception(error.Message);
                }
            }
            catch (Exception ex)
            {
                AuthApiData<AlarmResponse> o_d = new AuthApiData<AlarmResponse>
                {
                    Message = ex.Message,
                    Status = "1"
                };
                return o_d;
            }
            
        }
    }
}
