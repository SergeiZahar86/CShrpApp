using System;
using System.Threading;
using DSAccess;
using Newtonsoft.Json.Linq;

namespace CShrpTest
{
    class Program
    {
       static void Main(string[] args)
        {

            DSAccessLib agent = DSAccessLib.getInstance();

            // Подключение к очереди
            while(agent.Init()==false)
            { 
                Console.WriteLine("[Init] "+agent.getLastError());
                Thread.Sleep(2000);
            }

            // Выполнение запросов
            while (true)
            {
                {   // login =====================================================
                    string session = null;
                    try
                    {
                        Console.WriteLine("[Запрос] login");
                        session = agent.login("www", "www");
                        Console.WriteLine(session + "  - строка сессии");

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        continue;
                    }
                    Thread.Sleep(1000);
                    try
                    {
                        JObject data = agent.getResult(session, 5000);
                        Console.WriteLine(data + "  - строка JSON data");
                        int code = int.Parse(data["code"].ToString());
                        Console.WriteLine(code + "  - получили код");

                        if (code != 0)
                        {
                            Console.WriteLine("[Ошибка] " + data["data"]);
                        }
                        else
                        {
                            Console.WriteLine("[Ответ] Имя: {0} {1}", data["data"]["description"], data["data"]["expiration"]);
                        }

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                Thread.Sleep(200000);


                {   // Change ====================================================
                    string session;
                    try
                    {
                        session = agent.change("www", "www", "", 4000);
                        Console.WriteLine("[Запрос]: change");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        continue;
                    }
                    Thread.Sleep(1000);

                    try
                    {
                        JObject data = agent.getResult(session, 2000);
                        Console.WriteLine("[Ответ] " + data["data"]);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                Thread.Sleep(100000);
            }
        } 
      }
}
