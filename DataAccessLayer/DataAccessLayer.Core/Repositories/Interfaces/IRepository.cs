namespace DataAccessLayer.Core.Repositories.Interfaces
{
    public interface IRepository<T> : IModelEditable<T>, IModelReadable<T>
    {

    }
}
