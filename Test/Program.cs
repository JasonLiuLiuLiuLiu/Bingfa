using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using Newtonsoft.Json;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("输入回车开始测试...");
            Console.ReadKey();
            ServicePointManager.DefaultConnectionLimit = 1000;
            for (int i = 0; i < 100; i++)
            {
                Thread td = new Thread(new ParameterizedThreadStart(PostTest));
                td.Start(i);
                Thread.Sleep(new Random().Next(1,100));//随机休眠时长
            }
            Console.ReadLine();
        }
        public static void PostTest(object i)
        {
            try
            {
                string url = "http://localhost:56295/api/values/1";//获取ID为1的student的信息
                Student student = JsonConvert.DeserializeObject<Student>(RequestHandler.HttpGet(url));
                student.Age++;//对年龄进行修改
                string postData = $"Id={ student.id}&age={student.Age}&Name={student.Name}&Pwd={student.Pwd}&LastChanged={student.LastChanged.ToString("yyyy-MM-dd HH:mm:ss.fff")}";
                Console.WriteLine($"线程{i.ToString()}Post数据{postData}");
                string r = RequestHandler.HttpPost("http://localhost:56295/api/values", postData);
                Console.WriteLine($"线程{i.ToString()}Post结果{r}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }
    }
}
