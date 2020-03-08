using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DemoAsyncProgramming.Client
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Press z to cancel the operation");
            var tokenSource = new CancellationTokenSource();
            var cancellationToken = tokenSource.Token;
            Task.Run(async () =>
            {
                var content = new List<Task<string>>();
                
                while (!cancellationToken.IsCancellationRequested)
                {
                    using (var client = new HttpClient())
                    {
                        var companyName = GetCompanyDetails("Name", cancellationToken);
                        var companyDescription = GetCompanyDetails("Description", cancellationToken);
                        var companySector = GetCompanyDetails("Sector", cancellationToken);

                        content.Add(companyName);
                        content.Add(companyDescription);
                        content.Add(companySector);
                        var allResult = await Task.WhenAll(content);

                        Console.WriteLine(companyName.Result + companyDescription.Result + companySector.Result);
                        Console.WriteLine("Press z to cancel the operation");
                        await Task.Delay(5000);
                    }
                }
            }, cancellationToken);

            while (true)
            {
                if (Console.Read() == 'z')
                {
                    tokenSource.Cancel();
                    Console.WriteLine("cancelled using Cancellation Token");
                }
            }
        }

        public static async Task<string> GetCompanyDetails(string loadData, CancellationToken cancellationToken)
        {
            string content = string.Empty;
            using (var client = new HttpClient())
            {
                var result = await client.GetAsync($"http://localhost:57622/company/GetCompany"+ loadData, cancellationToken);
                result.EnsureSuccessStatusCode();

                content = await result.Content.ReadAsStringAsync();
            }
            return content;
        }
    }
}
