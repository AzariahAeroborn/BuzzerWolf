using Microsoft.AspNetCore.Mvc;

namespace BuzzerWolf.BBAPI
{
    public interface IBBAPIClient
    {
        Task<IActionResult> Login(string userName, string accessKey);
        Task GetTeam(string? teamId = null);
    }
}