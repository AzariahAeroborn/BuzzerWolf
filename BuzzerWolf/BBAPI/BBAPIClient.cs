using BuzzerWolf.BBAPI.Exceptions;
using BuzzerWolf.BBAPI.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.Xml;
using System.Xml.Linq;

namespace BuzzerWolf.BBAPI
{
    public class BBAPIClient : IBBAPIClient
    {
        private readonly HttpClient _client;

        public BBAPIClient(HttpClient client)
        {
            _client = client;
        }

        public async Task<string> Login(string userName, string accessKey)
        {
            var response = await _client.GetAsync($"login.aspx?login={userName}&code={accessKey}");

            XElement content = XElement.Load(await response.Content.ReadAsStreamAsync());

            if (content.Descendants("loggedIn").FirstOrDefault() != null)
            {
                IEnumerable<string>? values;
                if (response.Headers.TryGetValues("Set-Cookie", out values))
                {
                    return values.First().Remove(0, 10);
                }
            }

            throw new UnauthorizedException();
        }

        public async Task<TeamInfo> GetTeam(int? id = null)
        {
            string requestPath = "teaminfo.aspx";

            if (id != null)
            {
                requestPath += $"?teamid={id}";
            }

            var response = await _client.GetAsync(requestPath);
            return new TeamInfo(XElement.Load(await response.Content.ReadAsStreamAsync()));
        }
    }
}
