using Imitator.CommonData;
using Imitator.CommonData.ViewModels;
using Imitator.CommonData.ViewModels.Responses;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Imitator.WebServices.Account
{
    public class AuthService
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
        /// Выполнение входа.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static async Task<UserShortModel> Login(string username, string userPassword)
        {
            try
            {
                var formContent = new FormUrlEncodedContent(new Dictionary<string, string>
                    {
                        { "username", username },
                        { "userpassword", userPassword},
                    });

                var password = WebUtility.UrlEncode(userPassword); 
              
                HttpResponseMessage response = await _httpClient.PostAsync($"imitator/loginuser?email={username}&userpassword={userPassword}", formContent);

                string s_result;
                using (HttpContent responseContent = response.Content)
                {
                    s_result = await responseContent.ReadAsStringAsync();
                }

                UserShortModel o_data = new UserShortModel();

                switch (response.StatusCode)
                {
                    case HttpStatusCode.OK:
                        {
                            o_data = JsonConvert.DeserializeObject<UserShortModel>(s_result);
                            return o_data;
                        }
                    case HttpStatusCode.BadRequest:
                        {
                            o_data = JsonConvert.DeserializeObject<UserShortModel>(s_result);
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
                UserShortModel o_data = new UserShortModel()
                {
                    Error = ex,
                    ErrorInfo = ex.Message,
                    Result = DefaultEnums.Result.Warning
                };
                return o_data;
            }
        }

        /// <summary>
        /// Выход из приложения.
        /// </summary>
        /// <returns></returns>
        public static async Task<BaseModel> LogOut()
        {
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync($"imitator/logoutuser");
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
        /// Регистрация физического лица.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static async Task<UserShortModel> Register(UserModel model)
        {
            try
            {
                //status & message
                var formContent = new FormUrlEncodedContent(new Dictionary<string, string>
                    {
                        { "UserFIO", model.UserFIO },
                        { "Email", model.Email},
                        { "Password", model.Password}
                    });
            
               
                HttpResponseMessage response = await _httpClient.PostAsync($"imitator/registeruser?UserFIO={model.UserFIO}&Email={model.Email}&Password={ model.Password}", formContent);

                string s_result;
                using (HttpContent responseContent = response.Content)
                {
                    s_result = await responseContent.ReadAsStringAsync();
                }

                UserShortModel o_data = new UserShortModel();

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
                            o_data = JsonConvert.DeserializeObject<UserShortModel>(s_result);
                            return o_data;
                        }
                    case HttpStatusCode.BadRequest:
                        {
                            o_data = JsonConvert.DeserializeObject<UserShortModel>(s_result);
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
                UserShortModel o_data = new UserShortModel()
                {
                    Error = ex,
                    ErrorInfo = ex.Message,
                    Result = DefaultEnums.Result.Warning
                };
                return o_data;
            }
        }
    }
}
