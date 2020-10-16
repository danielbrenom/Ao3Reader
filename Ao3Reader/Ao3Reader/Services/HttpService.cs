using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Ao3Reader.Extensions;
using Ao3Reader.Interfaces;
using Newtonsoft.Json;
using Xamarin.Forms.Internals;

namespace Ao3Reader.Services
{
    public class HttpService : IHttpService
    {
        private readonly HttpClient _client;

        public HttpService(IHttpClientFactory httpClientFactory, IConfigurationManager configurationManager)
        {
            _client = httpClientFactory.CreateClient();
            _client.Timeout = TimeSpan.FromSeconds(100);
            BaseUrl = configurationManager.GetConfigKey("BaseUrl");
            Headers = new Dictionary<string, string>();
            Query = new Dictionary<string, string>();
            FormBody = new Dictionary<string, string>();
        }

        protected string BaseUrl;
        protected string Path { get; set; }
        protected HttpMethod Method { get; set; }
        protected Dictionary<string, string> Headers { get; }
        protected Dictionary<string, string> Query { get; }
        public Dictionary<string, string> FormBody { get; }

        public object Body { get; set; }

        public async Task<T> ExecuteAsync<T>()
        {
            var responseCode = HttpStatusCode.InternalServerError;
            var responseBody = string.Empty;
            try
            {
                var request = MakeRequest();
                var response = await _client.SendAsync(request, new CancellationToken(false));
                ClearParameters();
                responseBody = await response.Content.ReadAsStringAsync();
                responseCode = response.StatusCode;
                if (response.IsSuccessStatusCode || response.StatusCode.Equals(HttpStatusCode.OK))
                {
                    var resp = default(T);
                    if (typeof(T) != typeof(string) && response.Content.Headers.ContentType != null &&
                        response.Content.Headers.ContentType.MediaType.Contains("application/json"))
                        resp = (T) JsonConvert.DeserializeObject(responseBody, typeof(T),
                            new JsonSerializerSettings {NullValueHandling = NullValueHandling.Ignore});
                    if (responseBody.StartsWith("\"") && response.Content.Headers.ContentType != null &&
                        response.Content.Headers.ContentType.MediaType.Contains("application/json"))
                        resp = (T) JsonConvert.DeserializeObject(responseBody, typeof(T),
                            new JsonSerializerSettings {NullValueHandling = NullValueHandling.Ignore});
                    if (resp is null && typeof(T) == typeof(string))
                        resp = responseBody.getAs<T>();
                    return resp;
                }

                var exception = response.StatusCode switch
                {
                    (HttpStatusCode) 555 => new HttpRequestException(responseBody, null),
                    HttpStatusCode.BadRequest => new HttpRequestException(responseBody, null),
                    _ => new HttpRequestException(responseBody, null)
                };
                throw exception;
            }
            catch (Exception e) when (e is HttpRequestException)
            {
                throw new Exception(e.Message);
            }
        }

        public IHttpService AddHeader(string name, object value)
        {
            AddParameter("Header", name, value);
            return this;
        }

        public IHttpService AddBody(string name, object value)
        {
            AddParameter("Body", name, value);
            return this;
        }

        public IHttpService AddQuery(string name, object value)
        {
            AddParameter("Query", name, value);
            return this;
        }

        public IHttpService AddPath(string name, object value)
        {
            throw new NotImplementedException();
        }

        public IHttpService Get(string url)
        {
            Method = HttpMethod.Get;
            Path = url;
            return this;
        }

        public IHttpService Post(string url)
        {
            Method = HttpMethod.Post;
            Path = url;
            return this;
        }

        public IHttpService AddParameter(string Type, string name, object value)
        {
            var adaptedValue =
                value is decimal ? value.ToString().Replace(".", "").Replace(",", ".") : value.ToString();
            switch (Type)
            {
                case "Body":
                    Body = value;
                    break;
                case "FormBody":
                    FormBody.Add(name, adaptedValue);
                    break;
                case "Header":
                    Headers.Add(name, adaptedValue);
                    break;
                case "Query":
                    Query.Add(name, adaptedValue);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(Type), Type);
            }
            return this;
        }

        private HttpRequestMessage MakeRequest()
        {
            var request = new HttpRequestMessage();
            if (Method is null)
                throw new Exception("Method not set");
            request.Method = Method;
            if (Body != null || FormBody != null && FormBody.Count > 0)
                request.Content = Body is null
                    ? new FormUrlEncodedContent(FormBody)
                    : (HttpContent) new StringContent(Body.ToString(), Encoding.UTF8, "application/json");
            Headers.ForEach(header => request.Headers.Add(header.Key, header.Value));
            var queryString = GenerateQueryString(Query);
            var uri = Uri.EscapeUriString($"{BaseUrl}{Path}?{queryString}");
            request.RequestUri = new Uri(uri);
            return request;
        }

        private static string GenerateQueryString(Dictionary<string, string> query)
        {
            var queryString = new StringBuilder();
            var qtd = 0;
            query.ForEach(parameter =>
            {
                queryString.Append(parameter.Key).Append("=").Append(parameter.Value);
                if (++qtd < query.Count)
                    queryString.Append("&");
            });
            return queryString.ToString();
        }

        private void ClearParameters()
        {
            Body = null;
            Query.Clear();
            Headers.Clear();
            FormBody.Clear();
        }
    }
}