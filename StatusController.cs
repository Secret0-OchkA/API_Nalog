using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace API_Nalog
{
    public class StatusController
    {
        string aderess = "https://statusnpd.nalog.ru/api/v1/tracker/taxpayer_status";

        string inn;
        DateTime requestDate;
        static HttpClient httpClient { get; }

        static StatusController()
        {
            httpClient = new HttpClient();
            httpClient.Timeout = new TimeSpan(0, 1, 0);
            httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }

        public StatusController(string inn, DateTime requestData)
        {
            this.inn = inn;
            this.requestDate = requestData;

           
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// <exception cref="TaskCanceledException">если запрос будет дольше минуты</exception>
        public async Task<NalogResponse?> GetContentAsync()
        {
            var RequestBody = new JsonObject();
            RequestBody.Add(new KeyValuePair<string, JsonNode?>("inn", inn));
            RequestBody.Add(new KeyValuePair<string, JsonNode?>("requestDate", requestDate));

            NalogResponse? result = new NalogResponse(false, "");

            try
            {
                Uri uri = new Uri(aderess);
                var content = new StringContent(RequestBody.ToJsonString(), Encoding.UTF8, "application/json");

                HttpResponseMessage responseMessage = await httpClient.PostAsync(uri, content);

                using (Stream stream = await responseMessage.Content.ReadAsStreamAsync())
                {
                    result = await JsonSerializer.DeserializeAsync<NalogResponse>(stream);
                }
            }
            catch (TaskCanceledException ex) 
            { throw ex; }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }

            return result;
        }
    }

}

