namespace DataAccessLayer.Core.Interfaces.Repositories
{
    public interface IRepository<T> : IModelEditable<T>, IModelReadable<T>
    {

    }
}
