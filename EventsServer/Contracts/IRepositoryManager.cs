namespace Contracts
{
    public interface IRepositoryManager
    {
        IEventRepository Event { get; }
        Task SaveAsync();
    }
}
