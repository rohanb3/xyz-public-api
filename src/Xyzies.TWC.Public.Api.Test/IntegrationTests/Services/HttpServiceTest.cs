using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xyzies.TWC.Public.Api.Models;
using Xyzies.TWC.Public.Api.Models.Options;
using Xyzies.TWC.Public.Api.Tests.Models.User;

namespace Xyzies.TWC.Public.Api.Tests.IntegrationTests.Services
{
    public class HttpServiceTest : IHttpServiceTest
    {
        private readonly string _identityServiceUsrl = null;
        private readonly string _publicApiServiceUrl = null;

        private readonly ILogger<HttpServiceTest> _logger = null;

        public HttpServiceTest(IOptions<RelationOptions> options, ILogger<HttpServiceTest> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            _identityServiceUsrl = options?.Value?.IdentityServiceUrl ?? throw new InvalidOperationException("Missing identity service url");
        }

        public async Task<TokenModel> GetAuthorizationToken(UserLoginOption userLogin)
        {
            var uri = new Uri($"{_identityServiceUsrl}/authorize/token");
            var responseString = await SendRequestAsync(uri, HttpMethod.Post, JsonConvert.SerializeObject(userLogin));

            return DeserializeResultFromResponseString<TokenModel>(responseString);
        }

        public async Task<User> GetUserProfile(TokenModel token)
        {
            var uri = new Uri($"{_identityServiceUsrl}/users/profile");
            var responseString = await SendRequestAsync(uri, HttpMethod.Get, token: token);

            return DeserializeResultFromResponseString<User>(responseString);
        }

        public async Task<User> CreateNewTestUser(CreateUserModel user, TokenModel token)
        {
            var uri = new Uri($"{_identityServiceUsrl}/users");
            var responseString = await SendRequestAsync(uri, HttpMethod.Post, JsonConvert.SerializeObject(user), token);

            return DeserializeResultFromResponseString<User>(responseString);
        }

        public async Task DeleteTestUser(Guid userId, TokenModel token)
        {
            var uri = new Uri($"{_identityServiceUsrl}/users/{userId.ToString()}");
            var responseString = await SendRequestAsync(uri, HttpMethod.Delete, token: token);
        }
        private async Task<string> SendRequestAsync(Uri uri, HttpMethod method, string body = null, TokenModel token = null)
        {
            using (HttpClient client = new HttpClient())
            {

                if (token != null && !string.IsNullOrWhiteSpace(token.TokenType) && !string.IsNullOrWhiteSpace(token.AccessToken))
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(token.TokenType, token.AccessToken);
                }

                HttpRequestMessage requestMessage = new HttpRequestMessage(method, uri);

                if (!string.IsNullOrWhiteSpace(body))
                {
                    requestMessage.Content = new StringContent(body, Encoding.UTF8, "application/json");
                }

                var response = await client.SendAsync(requestMessage);

                _logger.LogInformation($"[SendRequestAsync] request = {body}{Environment.NewLine}responseCode = {response.StatusCode}; responseMessage = {await response.Content.ReadAsStringAsync()}");

                response.EnsureSuccessStatusCode();

                var responseString = await response.Content.ReadAsStringAsync();

                return responseString;
            }
        }

        private T DeserializeResultFromResponseString<T>(string responseString)
        {
            if (string.IsNullOrWhiteSpace(responseString))
            {
                return default(T);
            }
            return JsonConvert.DeserializeObject<T>(responseString);
        }
    }
}
