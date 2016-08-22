using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace ReactiveSearch
{
    class SearchServiceClient
    {
        const string BaseAddress = "http://localhost.fiddler:2458/api/Search?searchTerm=";
        private HttpClient _httpClient;

        public SearchServiceClient()
        {
            

            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var t=SearchAsync("rx");//making a request to warm the server
        }
        public async Task<IEnumerable<string>> SearchAsync(string searchTerm)
        {
            var response = await _httpClient.GetAsync(BaseAddress + searchTerm);
            return await response.Content.ReadAsAsync<IEnumerable<string>>();
        }
    }
}