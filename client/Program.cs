using System;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text.Json;
using System.Text;

namespace client
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var http = new HttpClient();

            var group = "group_3";
            var members = new string[] {"member_1","member_2","member_3"};

            // feed
            foreach (var member in members)
            {
                var res = await http.PostAsync("http://localhost:5000/api/heartbeat/reportAlive", new StringContent(JsonSerializer.Serialize(new
                {
                    GroupId = group,
                    Id = member
                }),Encoding.UTF8, "application/json"));
                res.EnsureSuccessStatusCode();
            }

            // get result
            var timer2 = new System.Threading.Timer(async state =>
            {
                var res = await http.PostAsync("http://localhost:5000/api/heartbeat/getAlive", new StringContent(JsonSerializer.Serialize(new
                {
                    GroupId = group
                }), Encoding.UTF8,"application/json"));
                res.EnsureSuccessStatusCode();
                Console.WriteLine(await res.Content.ReadAsStringAsync());
            }, null, 0, 1000);

            // pooling
            var timer = new System.Threading.Timer(async state =>
            {
                await http.PostAsync("http://localhost:5000/api/heartbeat/reportAlive", new StringContent(JsonSerializer.Serialize(new
                {
                    GroupId = group,
                    Id = members[0]
                }), Encoding.UTF8,"application/json"));
            }, null, 0, 10*1000);


            // // pooling
            var timer1 = new System.Threading.Timer(async state =>
            {
                await http.PostAsync("http://localhost:5000/api/heartbeat/reportAlive", new StringContent(JsonSerializer.Serialize(new
                {
                    GroupId = group,
                    Id = members[1]
                }), Encoding.UTF8,"application/json"));
            }, null, 0, 20*1000);
            Console.WriteLine("Hello World!");
            Console.ReadLine();
            timer.Dispose();
            timer1.Dispose();
            timer2.Dispose();
            
        }
    }
}
