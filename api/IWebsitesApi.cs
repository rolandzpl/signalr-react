using Domain;

internal interface IWebsitesApi
{
    Task<IEnumerable<WebsiteDto>> SendWebsiteChanged(string slug);
}
