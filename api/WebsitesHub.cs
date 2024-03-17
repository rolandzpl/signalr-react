using Domain;
using Microsoft.AspNetCore.SignalR;

internal class WebsitesHub : Hub<IWebsitesApi>
{
    public async Task<IEnumerable<WebsiteDto>> SendWebsiteChanged(string slug)
    {
        return await Clients.All.SendWebsiteChanged(slug);
    }
}
