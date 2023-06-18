namespace Data.Core
{
    public interface IUnitOfWork
    {
        Task<bool> Commit();
    }
}