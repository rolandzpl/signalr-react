using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

static class WebsiteAPI
{
    public static WebApplication UseWebsitesAPI(this WebApplication app)
    {
        app.MapGet("/api/website/list", ([FromServices] IEnumerable<WebsiteDto> websites) =>
        {
            return websites.ToList();
        })
        .WithName("GetWebsiteList")
        .WithOpenApi();

        app.MapPost("/api/website/{slug}", async (string slug, [FromBody] WebsiteDto website, [FromServices] ICollection<WebsiteDto> websites, [FromServices] IHubContext<WebsitesHub> hub) =>
        {
            var exists = websites.Any(_ => _.Slug == slug);
            if (exists)
            {
                throw new AlreadyExistsException();
            }
            websites.Add(website);
            await hub.Clients.All.SendAsync("SendWebsiteChanged", slug);
        })
        .WithName("CreateWebsite")
        .WithOpenApi();

        app.MapPut("/api/website/{slug}", async (string slug, [FromBody] WebsiteDto website, [FromServices] ICollection<WebsiteDto> websites, [FromServices] IHubContext<WebsitesHub> hub) =>
        {
            var existing = websites.Where(_ => _.Slug == slug).ToList();
            foreach (var w in existing)
            {
                websites.Remove(w);
            }
            websites.Add(website);
            await hub.Clients.All.SendAsync("SendWebsiteChanged", slug);
        })
        .WithName("UpdateWebsite")
        .WithOpenApi();
        return app;
    }
}
