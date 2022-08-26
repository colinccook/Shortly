using HashidsNet;

public class HashIdsDotNet : IHasher
{
    private Hashids hashIds;

    public HashIdsDotNet(IConfiguration configuration)
    {
        hashIds = new Hashids(configuration.GetValue<string>("HashingSalt"));
    }
    
    public string GenerateHash(long count)
    {
        return hashIds.EncodeLong(count);
    }
}