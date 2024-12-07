using BuzzerWolf.BBAPI.Exceptions;
using BuzzerWolf.BBAPI.Model;
using Microsoft.AspNetCore.WebUtilities;
using System.Xml.Linq;

namespace BuzzerWolf.BBAPI
{
    public class BBAPIClient : IBBAPIClient
    {
        private readonly SocketsHttpHandler _handler;
        private readonly HttpClient _client;
        private readonly IHttpContextAccessor _accessor;
        private bool _loggedIn = false;

        public BBAPIClient(IHttpContextAccessor accessor)
        {
            _handler = new SocketsHttpHandler { PooledConnectionLifetime = TimeSpan.FromMinutes(15) };
            _client = CreateBBAPIClient();
            _accessor = accessor;
        }

        private HttpClient CreateBBAPIClient()
        {
            return new HttpClient(_handler)
            {
                BaseAddress = new Uri("https://bbapi.buzzerbeater.com")
            };
        }

        public async Task<bool> Login(string userName, string accessKey, bool secondTeam)
        {
            var bbapi = await CallAPI("login.aspx", new Dictionary<string, string?>()
            {
                { "login", userName },
                { "code", accessKey },
                { "secondTeam", secondTeam ? "1" : null }
            });

            if (bbapi.IsSuccess)
            {
                return true;
            }
            else
            {
                switch (bbapi.Error)
                {
                    case "NotAuthorized":
                        throw new UnauthorizedException();
                    case "ServerError":
                        throw new BBAPIServerErrorException();
                    default:
                        throw new UnexpectedResponseException();
                }
            }
        }

        public async Task<bool> Logout()
        {
            var bbapi = await CallAPI("logout.aspx", new Dictionary<string, string?>());

            return bbapi.IsSuccess;
        }

        public async Task<Arena> GetArena(int? teamId = null)
        {
            var bbapi = await CallAPIAuthenticated("arena.aspx", new Dictionary<string, string?>()
            {
                { "teamId", teamId?.ToString() }
            });

            if (bbapi.IsSuccess)
            {
                return new Arena(bbapi.Response);
            }
            else
            {
                throw new UnexpectedResponseException();
            }
        }

        public void GetBoxScore(int matchId)
        {
            throw new NotImplementedException();
        }

        public async Task<Economy> GetEconomy()
        {
            var bbapi = await CallAPIAuthenticated("economy.aspx", new Dictionary<string, string?>());
            
            if (bbapi.IsSuccess)
            {
                return new Economy(bbapi.Response);
            }
            else
            {
                throw new UnexpectedResponseException();
            }
        }

        public async Task<Roster> GetRoster(int? teamId = null)
        {
            var bbapi = await CallAPIAuthenticated("roster.aspx", new Dictionary<string, string?>()
            {
                { "teamid", teamId?.ToString() }
            });

            if (bbapi.IsSuccess)
            {
                return new Roster(bbapi.Response);
            }
            else
            {
                throw new UnexpectedResponseException();
            }
        }

        public async Task<Player> GetPlayer(int playerId)
        {
            var bbapi = await CallAPIAuthenticated("player.aspx", new Dictionary<string, string?>()
            {
                { "playerid", playerId.ToString() }
            });

            if (bbapi.IsSuccess)
            {
                return new Player(bbapi.Response);
            }
            else
            {
                throw new UnexpectedResponseException();
            }
        }

        public async Task<Schedule> GetSchedule(int? teamId = null, int? season = null)
        {
            var bbapi = await CallAPIAuthenticated("schedule.aspx", new Dictionary<string, string?>()
            {
                { "teamid", teamId?.ToString() },
                { "season", season?.ToString() }
            });

            if (bbapi.IsSuccess)
            {
                return new Schedule(bbapi.Response);
            }
            else
            {
                throw new UnexpectedResponseException();
            }
        }

        public async Task<SeasonList> GetSeasons()
        {
            var bbapi = await CallAPIAuthenticated("seasons.aspx", new Dictionary<string, string?>());

            if (bbapi.IsSuccess)
            {
                return new SeasonList(bbapi.Response);
            }
            else
            {
                throw new UnexpectedResponseException();
            }
        }

        public async Task<Standings> GetStandings(int? leagueId = null, int? season = null)
        {
            var bbapi = await CallAPIAuthenticated("standings.aspx", new Dictionary<string, string?>()
            {
                { "leagueid", leagueId?.ToString() },
                { "season", season?.ToString() }
            });

            if (bbapi.IsSuccess)
            {
                return new Standings(bbapi.Response);
            }
            else
            {
                throw new UnexpectedResponseException();
            }
        }

        public async Task<TeamInfo> GetTeamInfo(int? teamId = null)
        {
            var bbapi = await CallAPIAuthenticated("teaminfo.aspx", new Dictionary<string, string?>()
            {
                { "teamid", teamId?.ToString() }
            });

            if (bbapi.IsSuccess)
            {
                return new TeamInfo(bbapi.Response);
            }
            else
            {
                throw new UnexpectedResponseException();
            }
        }

        public void GetTeamStats(int? teamId = null, int? season = null, string mode = "averages")
        {
            throw new NotImplementedException();
        }

        public async Task<List<Country>> GetCountries()
        {
            var bbapi = await CallAPIAuthenticated("countries.aspx", new Dictionary<string, string?>());

            if (bbapi.IsSuccess)
            {
                var countryList = new List<Country>();
                foreach (var country in bbapi.Response.Descendants("country"))
                {
                    countryList.Add(new Country(country));
                }
                return countryList;
            }
            else
            {
                throw new UnexpectedResponseException();
            }
        }

        public async Task<List<League>> GetLeagues(int countryId, int level)
        {
            var bbapi = await CallAPIAuthenticated("leagues.aspx", new Dictionary<string, string?>()
            {
                { "countryid", countryId.ToString() },
                { "level", level.ToString() }
            });

            if (bbapi.IsSuccess)
            {
                var leaguesList = new List<League>();
                foreach (var league in bbapi.Response.Descendants("league"))
                {
                    leaguesList.Add(new League(league, countryId, level));
                }
                return leaguesList;
            }
            else
            {
                throw new UnexpectedResponseException();
            }
        }

        private async Task<BBAPIResponse> CallAPIAuthenticated(string requestPath, Dictionary<string, string?> queryParams)
        {
            if (!_loggedIn)
            {
                if (_accessor.HttpContext!.User.HasClaim(c => c.Type == "userName"))
                {
                    _loggedIn = await Login(
                        _accessor.HttpContext.User.Claims.First(c => c.Type == "userName").Value,
                        _accessor.HttpContext.User.Claims.First(c => c.Type == "accessKey").Value,
                        bool.Parse(_accessor.HttpContext.User.Claims.First(c => c.Type == "secondTeam").Value)
                    );
                }
            }
            return await CallAPI(requestPath, queryParams);
        }

        private async Task<BBAPIResponse> CallAPI(string requestPath, Dictionary<string, string?> queryParams)
        {
            var response = await _client.GetAsync(QueryHelpers.AddQueryString(requestPath, queryParams.Where(q => q.Value != null)));
            XElement bbapiResponse = XElement.Load(await response.Content.ReadAsStreamAsync());

            var error = bbapiResponse.Descendants("error").FirstOrDefault();
            if (error != null)
            {
                var errorType = error.Attribute("message")!.Value;
                switch (errorType)
                {
                    case "NotAuthorized":
                        throw new UnauthorizedException();
                    case "ServerError":
                        throw new BBAPIServerErrorException();
                    case "UnknownTeamID":
                        throw new UnexpectedResponseException("Invalid team ID provided");
                    case "UnknownPlayerID":
                        throw new UnexpectedResponseException("Invalid player ID provided");
                    case "UnknownLeagueID":
                        throw new UnexpectedResponseException("Invalid league ID provided");
                    case "UnknownSeason":
                        throw new UnexpectedResponseException("Invalid season provided");
                    default:
                        return new BBAPIResponse()
                        {
                            IsSuccess = false,
                            Error = errorType,
                            Response = bbapiResponse
                        };
                }
            }

            return new BBAPIResponse()
            {
                IsSuccess = true,
                Error = null,
                Response = bbapiResponse
            };
        }
    }
}
