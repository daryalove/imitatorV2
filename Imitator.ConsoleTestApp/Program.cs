using Imitator.CommonData.DataModels;
using Imitator.CommonData.ViewModels;
using Imitator.CommonData.ViewModels.Responses;
using Imitator.WebServices;
using Imitator.WebServices.Account;
using Imitator.WebServices.Device;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Imitator.ConsoleTestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                UserModel model = new UserModel
                {
                    UserFIO = "Иван Иванов",
                    Email = "nich@yandex.ru",  
                    Password = "123456789"
                };
                //StaticBox.DeviceId = "866588031322022";
                bool showMenu = true;
                while (showMenu)
                {
                    showMenu = MainMenu(model);
                }

                Console.Read();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Something is wrong: " + ex.Message);
            }
        }

        /// <summary>
        /// Главное меню.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private static bool MainMenu(UserModel model)
        {
            Console.Clear();
            Console.WriteLine("============================");
            Console.WriteLine("Choose an option:");
            Console.WriteLine("(1) Login User");
            Console.WriteLine("(2) Register User");
            Console.WriteLine("(3) Register Device");
            Console.WriteLine("(4) Register Box");
            Console.WriteLine("(5) Update Sensors");
            Console.WriteLine("(6) Get Sensors");
            Console.WriteLine("(7) Set GPS");
            Console.WriteLine("(8) Set GPS Main");
            Console.WriteLine("(9) Update Photo Request");
            Console.WriteLine("(10) Check out photo request");
            Console.WriteLine("(11) Cancel Alarm");
            Console.WriteLine("(12) Set Alarm");
            Console.WriteLine("(13) Log out");
            Console.WriteLine("(14) Shut down app");
            Console.WriteLine("============================");
            Console.Write("\r\nSelect an option: ");

            switch (Console.ReadLine())
            {
                case "1":
                    {
                        LoginUser(model.Email, model.Password).Wait();
                        System.Threading.Thread.Sleep(20000);
                        return true;
                    }
                case "2":
                    {
                        RegisterUser(model).Wait();
                        System.Threading.Thread.Sleep(20000);
                        return true;
                    }
                case "3":
                    {
                        RegisterDevice().Wait();
                        System.Threading.Thread.Sleep(20000);
                        return true;
                    }
                case "4":
                    {
                        RegisterBox().Wait();
                        System.Threading.Thread.Sleep(20000);
                        return true;
                    }
                case "5":
                    {
                        Console.Clear();
                        UpdateSensors().Wait();
                        System.Threading.Thread.Sleep(20000);
                        return true;
                    }
                case "6":
                    {
                        Console.Clear();
                        GetSensors().Wait();
                        System.Threading.Thread.Sleep(20000);
                        return true;
                    }
                case "7":
                    {
                        Console.Clear();
                        SetGps().Wait();
                        System.Threading.Thread.Sleep(20000);
                        return true;
                    }
                case "8":
                    {
                        Console.Clear();
                        SetGpsMain().Wait();
                        System.Threading.Thread.Sleep(20000);
                        return true;
                    }
                case "9":
                    {
                        Console.Clear();
                        UpdatePhotoRequest().Wait();
                        System.Threading.Thread.Sleep(20000);
                        return true;
                    }
                case "10":
                    {
                        Console.Clear();
                        SearchCommandPhoto().Wait();
                        System.Threading.Thread.Sleep(20000);
                        return true;
                    }
                case "11":
                    {
                        Console.Clear();
                        CancelAlarm().Wait();
                        System.Threading.Thread.Sleep(20000);
                        return true;
                    }
                case "12":
                    {
                        Console.Clear();
                        SetAlarm().Wait();
                        System.Threading.Thread.Sleep(20000);
                        return true;
                    }
                case "13":
                    {
                        Console.Clear();
                        LogOut().Wait();
                        System.Threading.Thread.Sleep(20000);
                        return true;
                    }
                case "14":
                    {
                        return false;
                    }
                default:
                    return true;
            }
        }

        /// <summary>
        /// Установка тревоги.
        /// </summary>
        /// <returns></returns>
        private async static Task SetAlarm()
        {
            var o_data = await SensorsService.MakeRequestAlarm("2");

            if (o_data.Status.ToString() == "0")
            {

                Console.WriteLine(o_data.ResponseData.Message);
            }
            else
                Console.WriteLine("Something is wrong: " + o_data.Message);
        }

        /// <summary>
        /// Отмена тревоги.
        /// </summary>
        /// <returns></returns>
        private async static Task CancelAlarm()
        {
            var o_data = await SensorsService.CancelAlarm("2");

            if (o_data.Status.ToString() == "0")
            {

                Console.WriteLine(o_data.ResponseData.Message);
            }
            else
                Console.WriteLine("Something is wrong: " + o_data.Message);
        }

        /// <summary>
        /// Поиск сторонних запросов наличия фото.
        /// </summary>
        /// <returns></returns>
        private async static Task SearchCommandPhoto()
        {
            using (var client = ClientHelper.GetClient(StaticUser.Token))
            {
                SensorsService.InitializeClient(client);
                var o_data = await SensorsService.SearchPhotoRequest(StaticBox.DeviceId);

                if (o_data.Result.ToString() == "OK")
                {
                    Console.WriteLine(o_data.SuccessInfo);
                   // Console.WriteLine((o_data.HasPhotoRequest == true)?"Появился новый запрос на фото!":"Запросов нет.");
                    Console.WriteLine("IMEI устройства: " + o_data.IMEI);
                }
                else
                    Console.WriteLine("Something is wrong: " + o_data.ErrorInfo);
            }
        }

        /// <summary>
        /// Обновление статуса наличия фото.
        /// </summary>
        /// <returns></returns>
        private async static Task UpdatePhotoRequest()
        {
            using (var client = ClientHelper.GetClient(StaticUser.Token))
            {
                SensorsService.InitializeClient(client);
                var o_data = await SensorsService.UpdatePhotoRequest("866588031322022"/*StaticBox.IMEI*/);

                if (o_data.Result.ToString() == "OK")
                {

                    Console.WriteLine(o_data.SuccessInfo);
                }
                else
                    Console.WriteLine("Something is wrong: " + o_data.ErrorInfo);
            }
        }

        /// <summary>
        /// Отправка координат на сервер Шахт.
        /// </summary>
        /// <returns></returns>
        private async static Task SetGpsMain()
        {
            BoxLocation loc = new BoxLocation
            {
                date = DateTime.Now,
                lat1 = "42.3434",
                lon1 = "33.001",
                id = StaticBox.IMEI
            };
            var o_data = await LocationService.SetLocation(loc);

            if (o_data.Status == "0")
            {

                Console.WriteLine(o_data.Message);
            }
            else
                Console.WriteLine("Something is wrong: " + o_data.Message);
        }

        /// <summary>
        /// Отправка координат на промежуточный сервер.
        /// </summary>
        /// <returns></returns>
        private async static Task SetGps()
        {
            using (var client = ClientHelper.GetClient(StaticUser.Token))
            {
                PositionModel model = new PositionModel
                {
                    CurrentDate = DateTime.Now.ToString(),
                    DeviceId = StaticBox.DeviceId,
                    Latitude = 34.333,
                    Longitude = 35.444
                };

                LocationService.InitializeClient(client);
                var o_data = await LocationService.SetGPS(model);

                if (o_data.Result.ToString() == "OK")
                {
                    Console.WriteLine(o_data.SuccessInfo);
                }
                else
                    Console.WriteLine("Something is wrong: " + o_data.ErrorInfo);
            }
        }

        /// <summary>
        /// Регистрация устройства.
        /// </summary>
        /// <returns></returns>
        private async static Task RegisterDevice()
        {
            using (var client = ClientHelper.GetClient(StaticUser.Token))
            {
                SensorsService.InitializeClient(client);
                DeviceModel model = new DeviceModel
                {
                    Idiom = "Phone",
                    IMEI = StaticBox.IMEIs["5"],
                    Manufacturer = "Samsung",
                    ModelName = "SMG-950U",
                    Name = "Motz's iPhone",
                    Platform = "Android",
                    Version = "7.0"
                };
                var o_data = await SensorsService.RegisterDevice(model);

                if (o_data.Result.ToString() == "OK")
                {
                    StaticBox.DeviceId = o_data.Id;
                    StaticBox.IMEI = o_data.IMEI;
                    Console.WriteLine(o_data.SuccessInfo);
                }
                else
                    Console.WriteLine("Something is wrong: " + o_data.ErrorInfo);
            }
        }

        /// <summary>
        /// Регистрация устройства на сервере Шахт.
        /// </summary>
        /// <returns></returns>
        private async static Task RegisterBox()
        {
            var o_data = await SensorsService.RegisterBox(StaticBox.IMEI);

            if (o_data.Result.ToString() == "OK")
            {
                
                Console.WriteLine(o_data.SuccessInfo);
            }
            else
                Console.WriteLine("Something is wrong: " + o_data.ErrorInfo);
        }

        /// <summary>
        /// Обновление показаний датчиков на сервере Шахт.
        /// </summary>
        /// <returns></returns>
        private async static Task UpdateSensors()
        {
       //< item > На складе </ item >
       //   < item > На автомобиле </ item >
       //      < item > У заказчика </ item >

            StaticBox.Sensors["Вес груза"] = "1290.34";
            StaticBox.Sensors["Температура"] = "19";
            StaticBox.Sensors["Влажность"] = "10";
            StaticBox.Sensors["Освещенность"] = "1";
            StaticBox.Sensors["Уровень заряда аккумулятора"] = "14";
            StaticBox.Sensors["Уровень сигнала"] = "-8";
            StaticBox.Sensors["Состояние дверей"] = "1";
            StaticBox.Sensors["Состояние контейнера"] = "0";
            StaticBox.Sensors["Местоположение контейнера"] = "На складе";

            EditBoxViewModel ForAnotherServer = new EditBoxViewModel
            {
                id = "866588031322022",

                Sensors = new Dictionary<string, string>
                {
                    ["Вес груза"] = StaticBox.Sensors["Вес груза"],
                    ["Температура"] = StaticBox.Sensors["Температура"],
                    ["Влажность"] = StaticBox.Sensors["Влажность"],
                    ["Освещенность"] = StaticBox.Sensors["Освещенность"],
                    ["Уровень заряда аккумулятора"] = StaticBox.Sensors["Уровень заряда аккумулятора"],
                    ["Уровень сигнала"] = StaticBox.Sensors["Уровень сигнала"],
                    ["Состояние дверей"] = StaticBox.Sensors["Состояние дверей"],
                    ["Состояние контейнера"] = StaticBox.Sensors["Состояние контейнера"],
                    ["Местоположение контейнера"] = StaticBox.Sensors["Местоположение контейнера"]
                },
            };
            var o_data = await SensorsService.EditBox(ForAnotherServer);

            if (o_data.Status == "0")
            {

                Console.WriteLine(o_data.Message);
            }
            else
                Console.WriteLine("Something is wrong: " + o_data.Message);
        }

        /// <summary>
        /// Получение данных сенсоров.
        /// </summary>
        /// <returns></returns>
        private async static Task GetSensors()
        {
            var o_data = await SensorsService.GetInfoBox(StaticBox.IMEI);

            if (o_data.Result.ToString() == "OK")
            {

                Console.WriteLine(o_data.SuccessInfo);
            }
            else
                Console.WriteLine("Something is wrong: " + o_data.ErrorInfo);
        }

        /// <summary>
        /// Реализация выхода из веб-приложения.
        /// </summary>
        /// <returns></returns>
        private async static Task LogOut()
        {
            using (var client = ClientHelper.GetClient(StaticUser.Token))
            {
                AuthService.InitializeClient(client);
                var o_data = await AuthService.LogOut();

                if (o_data.Result.ToString() == "OK")
                {
                    Console.WriteLine(o_data.SuccessInfo);
                }
                else
                    Console.WriteLine("Something is wrong: " + o_data.ErrorInfo);
            }
        }

        /// <summary>
        /// Реализация входа в веб-приложение.
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        private async static Task LoginUser(string userName, string password)
        {
            using (var client = ClientHelper.GetClient())
            {
                AuthService.InitializeClient(client);
                UserShortModel o_data = null;
                o_data = await AuthService.Login(userName, password);

                if (o_data.Result.ToString() == "OK")
                {
                    Console.WriteLine("User: " + o_data.UserFIO);
                    Console.WriteLine("Token: " + o_data.Token);
                    StaticUser.Token = o_data.Token;
                }
                else
                    Console.WriteLine("Something is wrong: " + o_data.ErrorInfo);
            }
        }

        /// <summary>
        /// Реализация регистрации пользователя в веб-приложении.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private async static Task RegisterUser(UserModel model)
        {
            using (var client = ClientHelper.GetClient())
            {
                AuthService.InitializeClient(client);
                UserShortModel o_data = null;
                o_data = await AuthService.Register(model);

                if (o_data.Result.ToString() == "OK")
                {
                    Console.WriteLine("User: " + o_data.UserFIO);
                    Console.WriteLine("Info: " + o_data.SuccessInfo);
                }
                else
                    Console.WriteLine("Something is wrong: " + o_data.ErrorInfo);
            }
        }
    }
}
