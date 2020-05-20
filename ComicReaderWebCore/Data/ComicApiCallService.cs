using ComicReaderWebCore.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace ComicReaderWebCore.Data
{
    class ComicApiCallService : IComicApiCallService
    {

        private readonly IConfiguration _config;

        public ComicApiCallService(IConfiguration config)
        {
            _config = config;
            ApiLocation = _config.GetValue<string>("ApiLocation");
            PageLimit = _config.GetValue<int>("PageLimit");
            DefaultComicSize = _config.GetValue<int>("DefaultComicSize");
            SiteRoot = _config.GetValue<string>("SiteRoot");
        }

        public async Task<ComicListModel> GetComicInfo(string comic)
        {
            HttpClient client = new HttpClient
            {
                BaseAddress = new Uri(_config.GetValue<string>("ApiLocation"))
            };
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var request = $"{client.BaseAddress}comic{comic}/comicInfo";
            HttpResponseMessage response = await client.GetAsync(request);
            response.EnsureSuccessStatusCode();
            JObject jObject = JsonConvert.DeserializeObject<JObject>(response.Content.ReadAsStringAsync().Result);
            if (jObject != null)
            {
                ComicListModel returnList = new ComicListModel(jObject, _config);
                client.Dispose();
                return returnList;
            }
            else
            {
                ComicListModel emptyResponseList = new ComicListModel(_config);
                client.Dispose();
                return emptyResponseList;
            }
        }

        public async Task<ComicListModel> GetFolderListAsync(int? page = 1)
        {
            HttpClient client = new HttpClient
            {
                BaseAddress = new Uri(_config.GetValue<string>("ApiLocation"))
            };
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var request = $"{client.BaseAddress}folder/comic/{page}?pagelimit={_config.GetValue<int>("PageLimit")}";
            HttpResponseMessage response = await client.GetAsync(request);
            response.EnsureSuccessStatusCode();
            JObject jObject = JsonConvert.DeserializeObject<JObject>(response.Content.ReadAsStringAsync().Result);
            if (jObject != null)
            {
                ComicListModel returnList = new ComicListModel(jObject, _config);
                client.Dispose();
                return returnList;
            }
            else
            {
                ComicListModel emptyResponseList = new ComicListModel(_config);
                client.Dispose();
                return emptyResponseList;
            }
        }

        public string ApiLocation { get { return _config.GetValue<string>("ApiLocation"); } set { } }
        public string SiteRoot { get { return _config.GetValue<string>("SiteRoot"); } set { } }
        public int PageLimit { get { return _config.GetValue<int>("PageLimit"); } set { } }
        public int DefaultComicSize { get { return _config.GetValue<int>("DefaultComicSize"); } set { } }

    }
}
