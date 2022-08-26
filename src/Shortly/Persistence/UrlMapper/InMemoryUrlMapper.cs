public class InMemoryUrlMapper : IUrlMapper
{
    private Dictionary<string, string> urlMappings = new Dictionary<string, string>();

    public Task<string?> Get(string mapping)
    {
        string? url = null;
        urlMappings.TryGetValue(mapping, out url);
        return Task.FromResult(url);
    }

    public Task Save(string mapping, string url)
    {
        urlMappings.Add(mapping, url);
        return Task.CompletedTask;
    }
}