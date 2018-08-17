using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace FrostApp
{
    class Program
    {
        private static string baseUrl = "https://frost.met.no/";
        private static HttpClient client = new HttpClient();

        static void Main(string[] args)
        {
            InitClient();
            //Console.Out.WriteLine(GetSourcesByCounty("Troms").Result);
            //Console.Out.WriteLine(GetElements().Result);
            Console.Out.WriteLine(GetObservations("SN90400", "2008-05-09T13:00:00Z/2008-05-11T15:30:00Z", "min(air_temperature%20PT12H)").Result);
            Console.Out.WriteLine("Press enter to exit.");
            Console.ReadLine();
        }

        private static void InitClient()
        {
            client.BaseAddress = new Uri(baseUrl);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            AuthenticateClient();
        }

        private static void AuthenticateClient()
        {
            var credentialBytes = Encoding.ASCII.GetBytes("[ClientId]:");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(credentialBytes));
        }

        private static async Task<string> GetSourcesByCounty(string county)
        {
            HttpResponseMessage response = await client.GetAsync($"sources/v0.jsonld?county={county}");
            return await response.Content.ReadAsStringAsync();
        }

        private static async Task<string> GetElements()
        {
            HttpResponseMessage response = await client.GetAsync($"elements/v0.jsonld");
            return await response.Content.ReadAsStringAsync();
        }

        private static async Task<string> GetObservations(string sourceId, string referenceTime, string elementId)
        {
            HttpResponseMessage response = await client.GetAsync($"observations/v0.jsonld?sources={sourceId}&referencetime={referenceTime}&elements={elementId}");
            return await response.Content.ReadAsStringAsync();
        }
    }
}
