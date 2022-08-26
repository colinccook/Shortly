interface IUrlMapper {
    public Task Save(string mapping, string url);
    public Task<string?> Get(string mapping);
}