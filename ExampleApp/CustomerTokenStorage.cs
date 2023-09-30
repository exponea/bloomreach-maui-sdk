using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ExampleApp
{
    public sealed class CustomerTokenStorage
    {
        private static readonly CustomerTokenStorage instance = new CustomerTokenStorage();
        static CustomerTokenStorage()
        {

        }
        public static CustomerTokenStorage INSTANCE
        {
            get
            {
                return instance;
            }
        }

        private string host = null;
        private string projectToken = null;
        private string publicKey = null;
        private IDictionary<string, string> customerIds = null;
        private int? expiration = null;
        private string tokenCache = null;
        private long lastTokenRequestTime = 0;

        public void Configure(
            string host = null,
            string projectToken = null,
            string publicKey = null,
            IDictionary<string, string> customerIds = null
        )
        {
            this.host = host ?? this.host;
            this.projectToken = projectToken ?? this.projectToken;
            this.publicKey = publicKey ?? this.publicKey;
            this.customerIds = customerIds ?? this.customerIds;
        }

        public string RetrieveJwtToken()
        {
            long now = DateTime.Now.Ticks / TimeSpan.TicksPerSecond;
            long timeDiffMinutes = Math.Abs(now - lastTokenRequestTime) / 60;
            if (timeDiffMinutes < 5)
            {
                // allows request for token once per 5 minutes, doesn't care if cache is NULL
                return tokenCache;
            }
            lastTokenRequestTime = now;
            if (tokenCache != null)
            {
                // return cached value
                return tokenCache;
            }
            tokenCache = LoadJwtTokenAsync();
            return tokenCache;
        }

        private string LoadJwtTokenAsync()
        {
            if (
                this.host == null
                || this.projectToken == null
                || this.publicKey == null
                || this.customerIds == null
                || this.customerIds.Count == 0
                )
            {
                Console.WriteLine("CustomerTokenStorage not configured yet");
                return null;
            }
            var client = new HttpClient();
            client.BaseAddress = new Uri(this.host);
            var content = new StringContent(JsonConvert.SerializeObject(new {
                project_id = this.projectToken,
                kid = this.publicKey,
                sub = customerIds
            }), Encoding.UTF8, "application/json");
            Task<HttpResponseMessage> call = client.PostAsync("/webxp/exampleapp/customertokens", content);
            call.Wait();
            HttpResponseMessage response = call.Result;
            int statusCode = (int)response.StatusCode;
            if (statusCode == 404)
            {
                Console.WriteLine("BE has no token retrieval endpoint");
                return null;
            }
            if (statusCode >= 300 && statusCode < 599)
            {
                Console.WriteLine("Example token receiver returns " + statusCode);
                return null;
            }
            Task<string> responseTask = response.Content.ReadAsStringAsync();
            responseTask.Wait();
            string responseJson = responseTask.Result;
            TokenResponse responseData = JsonConvert.DeserializeObject<TokenResponse>(responseJson);
            if (responseData == null || responseData.customer_token == null)
            {
                Console.WriteLine("Example token received NULL");
                return null;
            }
            Console.WriteLine("Example token received " + responseData.customer_token);
            return responseData.customer_token;
        }

        public class TokenResponse
        {
            public string customer_token;
        }
    }
}
