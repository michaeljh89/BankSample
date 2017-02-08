using AttributeRoutingSample.Models;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace AttributeRoutingSample.Controllers
{
    public interface IcountryService
    {
        Task<List<Country>> getWorldBankCountryList();

    }

    public class countryService : IcountryService
    {

        private static string _address = "http://api.worldbank.org/countries?format=json";

        public async Task<List<Country>> getWorldBankCountryList()
        {
            HttpClient client = new HttpClient();

            // Send a request asynchronously and continue when complete
            HttpResponseMessage response = await client.GetAsync(_address);

            // Check that response was successful or throw exception
            response.EnsureSuccessStatusCode();

            // Read response asynchronously as JToken and write out top facts for each country
            JArray content = await response.Content.ReadAsAsync<JArray>();
            
            var cList = new List<Country>();

            foreach (var iCountry in content[1])
            {
                var country = new Country() { Name = iCountry.Value<string>("name") };
                cList.Add(country);

                //Console.WriteLine("   {0}, Country Code: {1}, Capital: {2}, Latitude: {3}, Longitude: {4}",
                //    iCountry.Value<string>("name"),
                //    iCountry.Value<string>("iso2Code"),
                //    iCountry.Value<string>("capitalCity"),
                //    iCountry.Value<string>("latitude"),
                //    iCountry.Value<string>("longitude"));
            }
            return cList;
        }

    }
}