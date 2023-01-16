using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.Xml;
using System.Xml.Linq;

namespace BuzzerWolf.BBAPI
{
    public class BBAPIClient : IBBAPIClient
    {
        private readonly HttpClient _client;
        private readonly IHttpContextAccessor _context;

        public BBAPIClient(HttpClient client, IHttpContextAccessor context)
        {
            _client = client;
            _context = context;
        }

        public async Task<IActionResult> Login(string userName, string accessKey)
        {
            var response = await _client.GetAsync($"login.aspx?login={userName}&code={accessKey}");

            XElement content = XElement.Load(await response.Content.ReadAsStreamAsync());

            if (content.Descendants("loggedIn").FirstOrDefault() != null)
            {
                IEnumerable<string>? values;
                if (response.Headers.TryGetValues("Set-Cookie", out values))
                {
                    _context.HttpContext?.Response.Cookies.Append(".ASPXAUTH", values.First().Remove(0,10));
                }
                return new OkResult();
            }
            else
            {
                return new StatusCodeResult(401);
            }
        }

        public async Task GetTeam(string? id = null)
        {
            //if (!_client.DefaultRequestHeaders.Contains(".ASPXAUTH"))
            //{
            //    throw new BadHttpRequestException("Must be logged in to access team data", 401);
            //}

            string requestPath = "teaminfo.aspx";

            if (id != null)
            {
                requestPath += $"?teamid={id}";
            }

            var response = await _client.GetAsync(requestPath);
            XElement content = XElement.Load(await response.Content.ReadAsStreamAsync());
        }
    }
}
