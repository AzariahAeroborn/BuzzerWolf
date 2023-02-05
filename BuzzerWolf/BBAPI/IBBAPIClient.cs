using BuzzerWolf.BBAPI.Model;

namespace BuzzerWolf.BBAPI
{
    public interface IBBAPIClient
    {
        Task<string> Login(string userName, string accessKey);
        Task<TeamInfo> GetTeam(int? teamId = null);
    }
}