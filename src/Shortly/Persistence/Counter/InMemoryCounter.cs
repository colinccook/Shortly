public class InMemoryCounter : ICounter
{
    private long counter = 0;

    public Task<long> GetNext()
    {
        return Task.FromResult<long>(++counter);
    }
}