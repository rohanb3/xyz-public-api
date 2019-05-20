using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Xyzies.TWC.Public.Api.Models;
using Xyzies.TWC.Public.Api.Models.Calls;
using Xyzies.TWC.Public.Api.Models.Options;

namespace Xyzies.TWC.Public.Api.Managers.Relation
{
    /// <inheritdoc />
    public class RelationService : IRelationService
    {
        private readonly string _identityServiceUrl = null;
        private readonly string _identityServiceStaticToken = null;
        private readonly string _vspVideoServiceUrl = null;

        /// <summary>
        /// Constructor with DI
        /// </summary> 
        public RelationService(IOptionsMonitor<RelationOptions> optionsMonitor)
        {
            var options = optionsMonitor?.CurrentValue ?? throw new ArgumentNullException(nameof(optionsMonitor));

            _identityServiceUrl = options?.IdentityServiceUrl ?? throw new ArgumentNullException(nameof(_identityServiceUrl));
            _vspVideoServiceUrl = options?.VspVideoServiceUrl ?? throw new ArgumentNullException(nameof(_vspVideoServiceUrl));
            _identityServiceStaticToken = options?.IdentityStaticToken ?? throw new ArgumentNullException(nameof(_identityServiceStaticToken));
        }

        /// <inheritdoc /> 
        public async Task<User> GetAzureUserByCPUserIdAsync(int cpUserId)
        {
            var responseString = await SendGetRequest(new Uri(_identityServiceUrl + "/users/" + _identityServiceStaticToken + "/cp/" + cpUserId.ToString()));
            return responseString != null
                ? GetIdentityResponse<User>(responseString)
                : null;
        }

        /// <inheritdoc /> 
        public async Task<User> GetAzureUserByObjectIdAsync(string objectId)
        {
            var responseString = await SendGetRequest(new Uri(_identityServiceUrl + "/users/" +  _identityServiceStaticToken + "/trusted/" + objectId));
            return responseString != null
                ? GetIdentityResponse<User>(responseString)
                : null;
        }

        /// <inheritdoc /> 
        public async Task<User> GetUserOnCallWithIdAsync(int cpUserId)
        {
            var azureOperator = await GetAzureUserByCPUserIdAsync(cpUserId);
            var responseString = await SendGetRequest(new Uri(_vspVideoServiceUrl + "/active-call-salesrep/" + azureOperator.ObjectId));

            var activeCall = !string.IsNullOrEmpty(responseString)
                                ? GetIdentityResponse<ActiveCall>(responseString)
                                : null;

            var user = activeCall != null
                ? await GetAzureUserByObjectIdAsync(activeCall.SalesRepId)
                : null;

            return user;
        }

        private T GetIdentityResponse<T>(string responseString)
        {
            if (string.IsNullOrWhiteSpace(responseString))
            {
                return default(T);
            }
            var data = JToken.Parse(responseString);
            return (data as JObject) != null && data["result"] != null
                ? data["result"].ToObject<T>()
                : JsonConvert.DeserializeObject<T>(responseString);
        }

        private async Task<string> SendGetRequest(Uri uri, string token = null)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = uri;
                if (!string.IsNullOrWhiteSpace(token))
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }
                var response = await client.GetAsync(client.BaseAddress);
                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    return null;
                }
                var responseString = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    throw new ApplicationException(responseString);
                }

                return responseString;
            }
        }
    }
}
